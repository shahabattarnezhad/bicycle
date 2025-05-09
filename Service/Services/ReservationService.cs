using Application.Contracts.Base;
using AutoMapper;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Service.Contracts.Base;
using Service.Contracts.Interfaces;
using Service.Contracts.Interfaces.Helpers;
using Service.Services.Helpers;
using Shared.Consts;
using Shared.DTOs.Document;
using Shared.DTOs.Reservation;
using Shared.Enums;
using Shared.Helpers;
using Shared.Requests;
using Shared.Responses;
using System.Security.Claims;

namespace Service.Services;

internal sealed class ReservationService : IReservationService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly IMemoryCacheService _cache;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ReservationService(
        IRepositoryManager repository,
        IMapper mapper,
        IMemoryCacheService cache,
        IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
        _httpContextAccessor = httpContextAccessor;
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
        string? userId = GetCurrentUserId();

        var entity = new Reservation
        {
            UserId = userId,
            BicycleId = entityForCreation.BicycleId,
            StartTime = entityForCreation.StartTime,
            EndTime = entityForCreation.EndTime,
            CreatedAt = DateTime.UtcNow,
            ReservationStatus = ReservationStatus.Active,
            TotalCost = CalculateCost(entityForCreation.StartTime, entityForCreation.EndTime)
        };

        _repository.Reservation.CreateEntity(entity);
        await _repository.SaveAsync(cancellationToken);

        _cache.RemoveByPrefix(ReservationCacheKeyHelper.ReservationPrefix);

        var entityToReturn =
            _mapper.Map<ReservationDto>(entity);

        return new ApiResponse<ReservationDto>(entityToReturn, "Reservation created successfully");
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

    private string GetCurrentUserId()
    {
        var userId =
                    _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            throw new UserNotFoundException("User not found");
        return userId;
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
