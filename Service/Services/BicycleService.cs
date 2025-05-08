
using Application.Contracts.Base;
using AutoMapper;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.Extensions.Logging;
using Service.Contracts.Base;
using Service.Contracts.Interfaces;
using Shared.DTOs.Bicycle;
using Shared.Requests;
using Shared.Responses;

namespace Service.Services;

internal sealed class BicycleService : IBicycleService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly IMemoryCacheService _cache;
    //private readonly ILogger<BicycleService> _logger;


    //public BicycleService(IRepositoryManager repository, IMapper mapper, ILogger<BicycleService> logger)
    //{
    //    _repository = repository;
    //    _mapper = mapper;
    //    _logger = logger;
    //}

    public BicycleService(IRepositoryManager repository, IMapper mapper, IMemoryCacheService cache)
    {
        _repository = repository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<ApiResponse<string>> ActivateBicycleAsync(Guid stationId, Guid bicycleId, CancellationToken cancellationToken = default)
    {
        await EnsureStationAndBicycleExistAsync(stationId, bicycleId, false, cancellationToken);

        await _repository.Bicycle.ActivateBicycleAsync(stationId, bicycleId, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _cache.RemoveByPrefix($"bicycles_station_{stationId}");
        _cache.Remove($"bicycles_station_{stationId}_bicycle_{bicycleId}");

        return new ApiResponse<string>(null, "Bicycle activated successfully");
    }

    public async Task<ApiResponse<int>> CountAsync(Guid stationId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        await EnsureStationExistsAsync(stationId, false, cancellationToken);

        var count =
            await _repository.Bicycle.CountAsync(stationId, trackChanges, cancellationToken);

        return new ApiResponse<int>(count, "Bicycles count retrieved successfully");
    }

    public async Task<ApiResponse<BicycleDto>> CreateAsync(Guid stationId, BicycleForCreationDto bicycle, CancellationToken cancellationToken = default)
    {
        //_logger.LogInformation("Start creating bicycle for station {StationId}", stationId);

        await EnsureStationExistsAsync(stationId, false, cancellationToken);

        var bicycleEntity = _mapper.Map<Bicycle>(bicycle);

        bicycleEntity.CurrentStationId = stationId;

        _repository.Bicycle.CreateBicycle(bicycleEntity);
        await _repository.SaveAsync(cancellationToken);

        _cache.RemoveByPrefix($"bicycles_station_{stationId}");
        _cache.Remove($"bicycles_station_{stationId}_bicycle_{bicycleEntity.Id}");

        //_logger.LogInformation("Bicycle with serial number {SerialNumber} created for station {StationId}", bicycle.SerialNumber, stationId);

        var bicycleToReturn = _mapper.Map<BicycleDto>(bicycleEntity);

        return new ApiResponse<BicycleDto>(bicycleToReturn, "Bicycle created successfully");
    }

    public async Task<ApiResponse<string>> DeactivateBicycleAsync(Guid stationId, Guid bicycleId, CancellationToken cancellationToken = default)
    {
        await EnsureStationAndBicycleExistAsync(stationId, bicycleId, false, cancellationToken);

        await _repository.Bicycle.DeactivateBicycleAsync(stationId, bicycleId, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _cache.RemoveByPrefix($"bicycles_station_{stationId}");
        _cache.Remove($"bicycles_station_{stationId}_bicycle_{bicycleId}");

        return new ApiResponse<string>(null, "Bicycle deactivated successfully");
    }

    public async Task<ApiResponse<string>> DeleteAsync(Guid stationId, Guid bicycleId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        //_logger.LogInformation("Start deleting bicycle for station {StationId}", stationId);

        await EnsureStationAndBicycleExistAsync(stationId, bicycleId, trackChanges, cancellationToken);

        var bicycle = await FindBicycleForStation(stationId, bicycleId, trackChanges, cancellationToken);

        _repository.Bicycle.DeleteBicycle(bicycle);
        await _repository.SaveAsync(cancellationToken);

        _cache.RemoveByPrefix($"bicycles_station_{stationId}");
        _cache.Remove($"bicycles_station_{stationId}_bicycle_{bicycleId}");

        //_logger.LogInformation("Bicycle deleted successfully");

        return new ApiResponse<string>(null, "Bicycle deleted successfully");
    }

    public async Task<ApiResponse<IEnumerable<BicycleDto>>> GetAllAsync(Guid stationId, BicycleParameters bicycleParameters, bool trackChanges, CancellationToken cancellationToken = default)
    {
        await EnsureStationExistsAsync(stationId, trackChanges, cancellationToken);

        var prefix = $"bicycles_station_{stationId}";
        var cacheKey = $"{prefix}_page_{bicycleParameters.PageNumber}_size_{bicycleParameters.PageSize}";

        //var pagedBicycles =
        //    await _repository.Bicycle.GetAllAsync(stationId, bicycleParameters, trackChanges, cancellationToken);

        var pagedBicycles = await _cache.GetOrCreateAsync(
        cacheKey,
        async () =>
        {
            return await _repository.Bicycle
                .GetAllAsync(stationId, bicycleParameters, trackChanges, cancellationToken);
        },
        TimeSpan.FromMinutes(10),
        prefix
    );

        var entitiesDto =
            _mapper.Map<IEnumerable<BicycleDto>>(pagedBicycles);

        return new ApiResponse<IEnumerable<BicycleDto>>(entitiesDto, "Bicycles retrieved successfully", pagedBicycles.Count());
    }

    public async Task<ApiResponse<IEnumerable<BicycleDto>>> GetAllForStationAsync(Guid stationId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        await EnsureStationExistsAsync(stationId, trackChanges, cancellationToken);

        var bicycles =
            await _repository.Bicycle.GetAllForStationAsync(stationId, trackChanges, cancellationToken);

        var entitiesDto =
            _mapper.Map<IEnumerable<BicycleDto>>(bicycles);

        return new ApiResponse<IEnumerable<BicycleDto>>(entitiesDto, "Bicycles retrieved successfully", bicycles.Count());
    }

    public async Task<ApiResponse<BicycleDto?>> GetAsync(Guid stationId, Guid bicycleId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        await EnsureStationAndBicycleExistAsync(stationId, bicycleId, trackChanges, cancellationToken);
        var cacheKey = $"bicycles_station_{stationId}_bicycle_{bicycleId}";

        var bicycle = await _cache.GetOrCreateAsync(
            cacheKey,
            async () => await FindBicycleForStation(stationId, bicycleId, trackChanges, cancellationToken),
            TimeSpan.FromMinutes(10),
            $"bicycles_station_{stationId}"
        );

        var entityDto = _mapper.Map<BicycleDto>(bicycle);
        return new ApiResponse<BicycleDto?>(entityDto, "Bicycle retrieved successfully");
    }

    public async Task<ApiResponse<IEnumerable<BicycleDto>>> GetAvailableAsync(Guid stationId, BicycleParameters bicycleParameters, bool trackChanges, CancellationToken cancellationToken = default)
    {
        await EnsureStationExistsAsync(stationId, trackChanges, cancellationToken);
        var cacheKey = $"bicycles_station_{stationId}_available";

        var bicycles = await _cache.GetOrCreateAsync(
            cacheKey,
            async () =>
            {
                var available =
                await _repository.Bicycle.GetAvailableAsync(stationId, bicycleParameters, trackChanges, cancellationToken);
                return _mapper.Map<IEnumerable<BicycleDto>>(available);
            },
            TimeSpan.FromMinutes(10),
            $"bicycles_station_{stationId}"
        );

        return new ApiResponse<IEnumerable<BicycleDto>>(bicycles, "Available bicycles retrieved successfully", bicycles.Count());
    }

    public async Task<ApiResponse<BicycleDto?>> GetBySerialNumberAsync(string serialNumber, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var bicycle =
            await _repository.Bicycle.GetBySerialNumberAsync(serialNumber, trackChanges, cancellationToken);

        if (bicycle is null)
            throw new BicycleNotFoundException(serialNumber);

        var entityDto = _mapper.Map<BicycleDto>(bicycle);

        return new ApiResponse<BicycleDto?>(entityDto, "Bicycle retrieved successfully");
    }

    public async Task<ApiResponse<IEnumerable<BicycleDto>>> GetElectricBicyclesAsync(Guid stationId, BicycleParameters bicycleParameters, bool trackChanges, CancellationToken cancellationToken = default)
    {
        await EnsureStationExistsAsync(stationId, trackChanges, cancellationToken);
        var cacheKey = $"bicycles_station_{stationId}_electric";

        var bicycles = await _cache.GetOrCreateAsync(
            cacheKey,
            async () =>
            {
                var electrics =
                await _repository.Bicycle.GetElectricBicyclesAsync(stationId, bicycleParameters, trackChanges, cancellationToken);
                return _mapper.Map<IEnumerable<BicycleDto>>(electrics);
            },
            TimeSpan.FromMinutes(10),
            $"bicycles_station_{stationId}"
        );

        return new ApiResponse<IEnumerable<BicycleDto>>(bicycles, "Electric bicycles retrieved successfully", bicycles.Count());
    }

    public async Task<ApiResponse<IEnumerable<BicycleDto>>> GetInActiveAsync(Guid stationId, BicycleParameters bicycleParameters, bool trackChanges, CancellationToken cancellationToken = default)
    {
        await EnsureStationExistsAsync(stationId, trackChanges, cancellationToken);
        var cacheKey = $"bicycles_station_{stationId}_inactive";

        var bicycles = await _cache.GetOrCreateAsync(
            cacheKey,
            async () =>
            {
                var inactives =
                await _repository.Bicycle.GetInActiveAsync(stationId, bicycleParameters, trackChanges, cancellationToken);
                return _mapper.Map<IEnumerable<BicycleDto>>(inactives);
            },
            TimeSpan.FromMinutes(10),
            $"bicycles_station_{stationId}"
        );

        return new ApiResponse<IEnumerable<BicycleDto>>(bicycles, "Inactive bicycles retrieved successfully", bicycles.Count());
    }

    public async Task<ApiResponse<IEnumerable<BicycleDto>>> GetStandardBicyclesAsync(Guid stationId, BicycleParameters bicycleParameters, bool trackChanges, CancellationToken cancellationToken = default)
    {
        await EnsureStationExistsAsync(stationId, trackChanges, cancellationToken);

        var pagedBicycles =
            await _repository.Bicycle.GetStandardBicyclesAsync(stationId, bicycleParameters, trackChanges, cancellationToken);

        var entitiesDto = _mapper.Map<IEnumerable<BicycleDto>>(pagedBicycles);

        return new ApiResponse<IEnumerable<BicycleDto>>(entitiesDto, "Standard bicycles retrieved successfully", pagedBicycles.Count());
    }

    public async Task<ApiResponse<BicycleDto?>> GetWithDetailsAsync(Guid stationId, Guid bicycleId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        await EnsureStationAndBicycleExistAsync(stationId, bicycleId, trackChanges, cancellationToken);

        var bicycle =
            await _repository.Bicycle.GetWithDetailsAsync(stationId, bicycleId, trackChanges, cancellationToken);

        if (bicycle is null)
            throw new EntityNotFoundException(nameof(bicycle), bicycleId);

        var entityDto = _mapper.Map<BicycleDto>(bicycle);

        return new ApiResponse<BicycleDto?>(entityDto, "Bicycle with details retrieved successfully");
    }

    public async Task<ApiResponse<IEnumerable<BicycleDto>>> GetWithGpsRecordsAsync(Guid stationId, BicycleParameters bicycleParameters, bool trackChanges, CancellationToken cancellationToken = default)
    {
        await EnsureStationExistsAsync(stationId, trackChanges, cancellationToken);

        var pagedBicycles =
            await _repository.Bicycle.GetWithGpsRecordsAsync(stationId, bicycleParameters, trackChanges, cancellationToken);

        var entitiesDto = _mapper.Map<IEnumerable<BicycleDto>>(pagedBicycles);

        return new ApiResponse<IEnumerable<BicycleDto>>(entitiesDto, "Bicycles with GPS records retrieved successfully", pagedBicycles.Count());
    }

    public async Task<ApiResponse<string>> UpdateAsync(Guid stationId, Guid bicycleId, BicycleForUpdationDto entityForUpdation, bool trackChanges, CancellationToken cancellationToken = default)
    {
        await EnsureStationAndBicycleExistAsync(stationId, bicycleId, trackChanges, cancellationToken);

        var bicycle = await FindBicycleForStation(stationId, bicycleId, trackChanges, cancellationToken);

        _mapper.Map(entityForUpdation, bicycle);
        await _repository.SaveAsync(cancellationToken);

        _cache.RemoveByPrefix($"bicycles_station_{stationId}");
        _cache.Remove($"bicycles_station_{stationId}_bicycle_{bicycleId}");

        return new ApiResponse<string>(null, "Bicycle updated successfully");
    }

    private async Task EnsureStationAndBicycleExistAsync(Guid stationId, Guid bicycleId, bool trackChanges, CancellationToken cancellationToken)
    {
        await EnsureStationExistsAsync(stationId, trackChanges, cancellationToken);
        await EnsureBicycleExistsAsync(bicycleId, trackChanges, cancellationToken);
    }

    private async Task EnsureStationExistsAsync(Guid stationId, bool trackChanges, CancellationToken cancellationToken)
    {
        var station =
                    await _repository.Station.GetAsync(stationId, trackChanges, cancellationToken);

        if (station is null)
            throw new EntityNotFoundException(nameof(station), stationId);
    }

    private async Task EnsureBicycleExistsAsync(Guid bicycleId, bool trackChanges, CancellationToken cancellationToken)
    {
        var bicycle =
                    await _repository.Bicycle.GetAsync(bicycleId, trackChanges, cancellationToken);

        if (bicycle is null)
            throw new EntityNotFoundException(nameof(bicycle), bicycleId);
    }

    private async Task<Bicycle?> FindBicycleForStation(Guid stationId, Guid bicycleId, bool trackChanges, CancellationToken cancellationToken)
    {
        var bicycle = await _repository.Bicycle.GetAsync(stationId, bicycleId, trackChanges, cancellationToken);

        if (bicycle is null)
            throw new EntityNotFoundException(nameof(bicycle), bicycleId);

        return bicycle;
    }
}
