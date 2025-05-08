using Application.Contracts.Base;
using AutoMapper;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts.Base;
using Service.Contracts.Interfaces;
using Shared.Consts;
using Shared.DTOs.Station;
using Shared.Requests;
using Shared.Responses;

namespace Service.Services;

internal sealed class StationService : IStationService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly IMemoryCacheService _cache;

    public StationService(IRepositoryManager repository, IMapper mapper, IMemoryCacheService cache)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<ApiResponse<IEnumerable<StationDto>>> GetAllAsync(StationParameters parameters, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var pagedStations = await _cache.GetOrCreateAsync(
            "All",
            async () =>
            {
                return await _repository.Station.GetAllAsync(parameters, trackChanges, cancellationToken);
            },
            TimeSpan.FromMinutes(30),
            CacheKeyPrefixes.Station
        );

        var entitiesDto =
            _mapper.Map<IEnumerable<StationDto>>(pagedStations);

        return new ApiResponse<IEnumerable<StationDto>>(entitiesDto, "Stations retrieved successfully", pagedStations.Count());
    }

    public async Task<ApiResponse<StationDto>>? GetAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var prefix = "stations";
        var cacheKey = $"{prefix}_{entityId}";

        var entity = await _cache.GetOrCreateAsync(
            cacheKey,
            async () => await FindEntity(entityId, trackChanges, cancellationToken),
            TimeSpan.FromMinutes(10),
            prefix
        );

        var entityDto =
            _mapper.Map<StationDto>(entity);

        return new ApiResponse<StationDto>(entityDto, "Station retrieved successfully");
    }

    public async Task<ApiResponse<StationDto>> CreateAsync(StationForCreationDto entityForCreation, CancellationToken cancellationToken = default)
    {
        var entity =
            _mapper.Map<Station>(entityForCreation);

        _repository.Station.CreateEntity(entity);
        await _repository.SaveAsync(cancellationToken);

        _cache.RemoveByPrefix("stations");

        var entityToReturn =
            _mapper.Map<StationDto>(entity);

        return new ApiResponse<StationDto>(entityToReturn, "Station created successfully");
    }

    public async Task<ApiResponse<string>> DeleteAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity =
            await FindEntity(entityId, trackChanges, cancellationToken);

        _repository.Station.DeleteEntity(entity);
        await _repository.SaveAsync(cancellationToken);

        _cache.Remove($"stations_{entityId}");
        _cache.RemoveByPrefix("stations");

        return new ApiResponse<string>(null, "Station deleted successfully");
    }

    public async Task<ApiResponse<string>> UpdateAsync(Guid entityId, StationForUpdationDto entityForUpdation, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity =
           await FindEntity(entityId, trackChanges, cancellationToken);

        _mapper.Map(entityForUpdation, entity);

        await _repository.SaveAsync(cancellationToken);

        _cache.Remove($"stations_{entityId}");
        _cache.RemoveByPrefix("stations");

        return new ApiResponse<string>(null, "Station updated successfully");
    }

    private async Task<Station> FindEntity(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var station =
            await _repository.Station.GetAsync(entityId, trackChanges, cancellationToken);

        return station ?? throw new EntityNotFoundException(nameof(station), entityId);
    }
}
