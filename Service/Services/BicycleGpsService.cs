using Application.Contracts.Base;
using AutoMapper;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts.Base;
using Service.Contracts.Interfaces;
using Service.Services.Helpers;
using Shared.Consts;
using Shared.DTOs.BicycleGps;
using Shared.DTOs.Station;
using Shared.Helpers;
using Shared.Requests;
using Shared.Responses;

namespace Service.Services;

internal sealed class BicycleGpsService : IBicycleGpsService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly IMemoryCacheService _cache;

    public BicycleGpsService(IRepositoryManager repository, IMapper mapper, IMemoryCacheService cache)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<ApiResponse<IEnumerable<BicycleGpsDto>>> GetAllAsync(BicycleGpsParameters parameters, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var prefix = BicycleGpsCacheKeyHelper.BicycleGpsPrefix;
        var cacheKey =
            BicycleGpsCacheKeyHelper.GetPagedKey( parameters.PageNumber, parameters.PageSize);

        var pagedBicycleGpsList = await _cache.GetOrCreateAsync(
            cacheKey,
            async () =>
            {
                return await _repository.BicycleGps.GetAllAsync(parameters, trackChanges, cancellationToken);
            },
            TimeSpan.FromMinutes(30),
            CacheKeyPrefixes.BicycleGps
        );

        var entitiesDto =
            _mapper.Map<IEnumerable<BicycleGpsDto>>(pagedBicycleGpsList);

        return new ApiResponse<IEnumerable<BicycleGpsDto>>(entitiesDto, "BicycleGps List retrieved successfully", pagedBicycleGpsList.Count());
    }

    public async Task<ApiResponse<BicycleGpsDto>>? GetAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var prefix = BicycleGpsCacheKeyHelper.BicycleGpsPrefix;
        var cacheKey = BicycleGpsCacheKeyHelper.GetEntityKey(entityId);

        var entity = await _cache.GetOrCreateAsync(
            cacheKey,
            async () => await FindEntity(entityId, trackChanges, cancellationToken),
            TimeSpan.FromMinutes(10),
            CacheKeyPrefixes.BicycleGps
        );

        var entityDto =
            _mapper.Map<BicycleGpsDto>(entity);

        return new ApiResponse<BicycleGpsDto>(entityDto, "BicycleGps retrieved successfully");
    }

    public async Task<ApiResponse<BicycleGpsDto>> CreateAsync(BicycleGpsForCreationDto entityForCreation, CancellationToken cancellationToken = default)
    {
        var entity =
            _mapper.Map<BicycleGps>(entityForCreation);

        _repository.BicycleGps.CreateEntity(entity);
        await _repository.SaveAsync(cancellationToken);

        _cache.RemoveByPrefix(BicycleGpsCacheKeyHelper.BicycleGpsPrefix);

        var entityToReturn =
            _mapper.Map<BicycleGpsDto>(entity);

        return new ApiResponse<BicycleGpsDto>(entityToReturn, "BicycleGps created successfully");
    }

    public async Task<ApiResponse<string>> DeleteAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity =
            await FindEntity(entityId, trackChanges, cancellationToken);

        _repository.BicycleGps.DeleteEntity(entity);
        await _repository.SaveAsync(cancellationToken);

        _cache.Remove(BicycleGpsCacheKeyHelper.GetEntityKey( entityId));
        _cache.RemoveByPrefix(BicycleGpsCacheKeyHelper.BicycleGpsPrefix);

        return new ApiResponse<string>(null, "BicycleGps deleted successfully");
    }

    private async Task<BicycleGps> FindEntity(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var bicycleGps =
            await _repository.BicycleGps.GetAsync(entityId, trackChanges, cancellationToken);

        return bicycleGps ?? throw new EntityNotFoundException(nameof(bicycleGps), entityId);
    }
}
