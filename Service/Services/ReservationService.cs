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
        var prefix = ReservationCacheKeyHelper.ReservationPrefix;
        var cacheKey = ReservationCacheKeyHelper.GenerateReservationListKey(parameters);

        var pagedReservations = await _cache.GetOrCreateAsync(
            cacheKey,
            async () =>
            {
                return await _repository.Reservation.GetAllAsync(parameters, trackChanges, cancellationToken);
            },
            TimeSpan.FromMinutes(10),
            prefix
        );

        var entitiesDto =
            _mapper.Map<IEnumerable<ReservationDto>>(pagedReservations);

        return new ApiResponse<IEnumerable<ReservationDto>>(entitiesDto, "Reservations retrieved successfully", pagedReservations.Count());
    }


    public async Task<ApiResponse<ReservationDto>> CreateAsync(ReservationForCreationDto entityForCreation, CancellationToken cancellationToken = default)
    {

        if (_userContextService.UserStatus != UserStatus.Approved.ToString())
            //throw new GeneralBadRequestException("User is not approved");
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
            _cache.RemoveByPrefix(ReservationCacheKeyHelper.ReservationPrefix);

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
        var cacheKey = ReservationCacheKeyHelper.GenerateReservationKey(entityId);

        var entity = await _cache.GetOrCreateAsync(
            cacheKey,
            async () => await FindEntity(entityId, trackChanges, cancellationToken),
            TimeSpan.FromMinutes(10),
            ReservationCacheKeyHelper.ReservationPrefix
        );

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

        _cache.Remove(ReservationCacheKeyHelper.GenerateReservationKey(entityId), ReservationCacheKeyHelper.ReservationPrefix);
        _cache.RemoveByPrefix(ReservationCacheKeyHelper.ReservationPrefix);

        return new ApiResponse<string>(null, "Station deleted successfully");
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
