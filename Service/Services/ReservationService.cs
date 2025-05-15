using Application.Contracts.Base;
using AutoMapper;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts.Base;
using Service.Contracts.Interfaces;
using Service.Contracts.Interfaces.Auth;
using Service.Services.Helpers;
using Shared.DTOs.Reservation;
using Shared.Enums;
using Shared.Helpers;
using Shared.Requests;
using Shared.Responses;

namespace Service.Services;

internal sealed class ReservationService : IReservationService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly IMemoryCacheService _cache;
    private readonly IUserContextService _userContextService;

    public ReservationService(
        IRepositoryManager repository,
        IMapper mapper,
        IMemoryCacheService cache,
        IUserContextService userContextService)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
        _userContextService = userContextService;
    }

    public async Task<ApiResponse<IEnumerable<ReservationDto>>> GetAllAsync(ReservationParameters parameters, bool trackChanges, CancellationToken cancellationToken = default)
    {
        //var prefix = ReservationCacheKeyHelper.ReservationPrefix;
        //var cacheKey = ReservationCacheKeyHelper.GenerateReservationListKey(parameters);

        //var pagedReservations = await _cache.GetOrCreateAsync(
        //    cacheKey,
        //    async () =>
        //    {
        //        return await _repository.Reservation.GetAllAsync(parameters, trackChanges, cancellationToken);
        //    },
        //    TimeSpan.FromMinutes(10),
        //    prefix
        //);

        var pagedReservations = await _repository.Reservation.GetAllAsync(parameters, trackChanges, cancellationToken);

        var entitiesDto =
            _mapper.Map<IEnumerable<ReservationDto>>(pagedReservations);

        return new ApiResponse<IEnumerable<ReservationDto>>(entitiesDto, "Reservations retrieved successfully", pagedReservations.Count());
    }


    public async Task<ApiResponse<ReservationDto>> CreateAsync(ReservationForCreationDto entityForCreation, CancellationToken cancellationToken = default)
    {

        if (_userContextService.UserStatus != UserStatus.Approved.ToString())
            throw new NoAccessException("Your account is not approved yet.");

        var userId = _userContextService.UserId;

        // 2. بررسی رزرو فعال کاربر
        bool hasActiveReservation =
            await _repository.Reservation.ExistsActiveStatusByUserAsync(userId, false, cancellationToken);

        if (hasActiveReservation)
            throw new GeneralBadRequestException("You already have an active reservation.");

        // 3. بررسی زمان اجاره
        if (entityForCreation.StartTime < DateTime.UtcNow)
            throw new GeneralBadRequestException("Start time cannot be in the past.");

        if ((entityForCreation.EndTime - entityForCreation.StartTime).TotalHours > 6)
            throw new GeneralBadRequestException("Reservations cannot exceed 6 hours.");

        // شروع تراکنش
        using var transaction = await _repository.BeginTransactionAsync(cancellationToken);

        try
        {
            // 4. بررسی دوچرخه و ایستگاه
            var bicycle =
                await _repository.Bicycle.GetAsync(entityForCreation.BicycleId, trackChanges: true, cancellationToken);

            if (bicycle == null || bicycle.BicycleStatus != BicycleStatus.Available)
                throw new GeneralBadRequestException("Selected bicycle is not available.");

            var station =
                await _repository.Station.GetAsync(bicycle.CurrentStationId, trackChanges: true, cancellationToken);

            if (station == null || station.AvailableBicycles <= 0)
                throw new GeneralBadRequestException("No available bikes at the station.");

            // 5. ساخت رزرو
            var reservation = new Reservation
            {
                UserId = userId,
                BicycleId = entityForCreation.BicycleId,
                StartTime = entityForCreation.StartTime,
                EndTime = entityForCreation.EndTime,
                CreatedAt = DateTime.UtcNow,
                ReservationStatus = ReservationStatus.Active,
                TotalCost = CalculateCost(entityForCreation.StartTime, entityForCreation.EndTime)
            };

            _repository.Reservation.CreateEntity(reservation);

            // 6. آپدیت دوچرخه و ایستگاه
            bicycle.BicycleStatus = BicycleStatus.Rented;
            station.AvailableBicycles--;

            // 7. ذخیره همه تغییرات
            await _repository.SaveAsync(cancellationToken);

            // 8. commit تراکنش
            await transaction.CommitAsync(cancellationToken);

            // 9. پاکسازی کش
            //_cache.RemoveByPrefix(ReservationCacheKeyHelper.ReservationPrefix);

            var dtoToReturn = _mapper.Map<ReservationDto>(reservation);
            return new ApiResponse<ReservationDto>(dtoToReturn, "Reservation created successfully.");
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }


    public async Task<ApiResponse<ReservationDto>>? GetAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        //var cacheKey = ReservationCacheKeyHelper.GenerateReservationKey(entityId);

        //var entity = await _cache.GetOrCreateAsync(
        //    cacheKey,
        //    async () => await FindEntity(entityId, trackChanges, cancellationToken),
        //    TimeSpan.FromMinutes(10),
        //    ReservationCacheKeyHelper.ReservationPrefix
        //);

        var entity =
            await FindEntity(entityId, trackChanges, cancellationToken);

        var entityDto =
            _mapper.Map<ReservationDto>(entity);

        return new ApiResponse<ReservationDto>(entityDto, "Reservation retrieved successfully");
    }

    public async Task<ApiResponse<string>> DeleteAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity =
            await FindEntity(entityId, trackChanges, cancellationToken);

        _repository.Reservation.DeleteEntity(entity);
        await _repository.SaveAsync(cancellationToken);

        //_cache.Remove(ReservationCacheKeyHelper.GenerateReservationKey(entityId), ReservationCacheKeyHelper.ReservationPrefix);
        //_cache.RemoveByPrefix(ReservationCacheKeyHelper.ReservationPrefix);

        return new ApiResponse<string>(null, "Station deleted successfully");
    }

    public async Task<ApiResponse<string>> ReturnBikeAsync(ReservationForReturnDto dto, CancellationToken cancellationToken = default)
    {
        var userId = _userContextService.UserId;

        // شروع تراکنش
        using var transaction = await _repository.BeginTransactionAsync(cancellationToken);

        try
        {
            // 1. پیدا کردن رزرو فعال
            var reservation = 
                await _repository.Reservation.GetAsync(dto.ReservationId, trackChanges: true, cancellationToken);

            if (reservation == null || reservation.ReservationStatus != ReservationStatus.Active || reservation.UserId != userId)
                throw new GeneralBadRequestException("No active reservation found for return.");

            // 2. بررسی زمان تحویل
            var now = DateTime.UtcNow;
            if (now < reservation.StartTime)
                throw new GeneralBadRequestException("Return time cannot be before reservation start time.");

            // 3. محاسبه هزینه نهایی
            var finalCost = CalculateCost(reservation.StartTime, now);
            reservation.EndTime = now;
            reservation.TotalCost = finalCost;
            reservation.ReservationStatus = ReservationStatus.Completed;

            // 4. بروزرسانی دوچرخه
            var bicycle = 
                await _repository.Bicycle.GetAsync(reservation.BicycleId, trackChanges: true, cancellationToken);

            if (bicycle == null)
                throw new GeneralBadRequestException("Bicycle not found.");

            bicycle.BicycleStatus = BicycleStatus.Available;
            bicycle.CurrentStationId = dto.ReturnStationId;

            // 5. افزایش موجودی ایستگاه مقصد
            var returnStation = 
                await _repository.Station.GetAsync(dto.ReturnStationId, trackChanges: true, cancellationToken);

            if (returnStation == null)
                throw new GeneralBadRequestException("Return station not found.");

            returnStation.AvailableBicycles++;

            // 6. ذخیره همه تغییرات
            await _repository.SaveAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            // 7. پاکسازی کش
            //_cache.RemoveByPrefix(ReservationCacheKeyHelper.ReservationPrefix);
            //_cache.Remove(ReservationCacheKeyHelper.GenerateReservationKey(dto.ReservationId));
            //_cache.RemoveByPrefix(BicycleCacheKeyHelper.BicyclePrefix(returnStation.Id));
            //_cache.RemoveByPrefix(BicycleCacheKeyHelper.BicyclePrefix(bicycle.CurrentStationId));
            //_cache.RemoveByPrefix(StationCacheKeyHelper.StationPrefix);

            return new ApiResponse<string>(null, $"Bike returned successfully. Final cost: {finalCost} Toman.");
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task<Reservation> FindEntity(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var reservation =
            await _repository.Reservation.GetAsync(entityId, trackChanges, cancellationToken);

        return reservation ?? throw new EntityNotFoundException(nameof(reservation), entityId);
    }

    private decimal CalculateCost(DateTime start, DateTime end)
    {
        var totalHours = (decimal)(end - start).TotalHours;

        if (totalHours <= 2)
            return 2;
        if (totalHours <= 4)
            return 6;
        return 10;
    }
}
