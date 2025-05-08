using Shared.DTOs.Bicycle;
using Shared.Requests;
using Shared.Requests.Base;
using Shared.Responses;

namespace Service.Contracts.Interfaces;

public interface IBicycleService
{
    Task<ApiResponse<IEnumerable<BicycleDto>>> GetAllAsync(Guid stationId, BicycleParameters bicycleParameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<ApiResponse<IEnumerable<BicycleDto>>> GetAvailableAsync(Guid stationId, BicycleParameters bicycleParameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<ApiResponse<IEnumerable<BicycleDto>>> GetWithGpsRecordsAsync(Guid stationId, BicycleParameters bicycleParameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<ApiResponse<IEnumerable<BicycleDto>>> GetInActiveAsync(Guid stationId, BicycleParameters bicycleParameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<ApiResponse<IEnumerable<BicycleDto>>> GetAllForStationAsync(Guid stationId, bool trackChanges, CancellationToken cancellationToken = default);

    Task<ApiResponse<IEnumerable<BicycleDto>>> GetElectricBicyclesAsync(Guid stationId, BicycleParameters bicycleParameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<ApiResponse<IEnumerable<BicycleDto>>> GetStandardBicyclesAsync(Guid stationId, BicycleParameters bicycleParameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<ApiResponse<BicycleDto?>> GetAsync(Guid stationId, Guid bicycleId, bool trackChanges, CancellationToken cancellationToken = default);

    Task<ApiResponse<BicycleDto?>> GetWithDetailsAsync(Guid stationId, Guid bicycleId, bool trackChanges, CancellationToken cancellationToken = default);

    Task<ApiResponse<BicycleDto?>> GetBySerialNumberAsync(string serialNumber, bool trackChanges, CancellationToken cancellationToken = default);

    Task<ApiResponse<int>> CountAsync(Guid stationId, bool trackChanges, CancellationToken cancellationToken = default);



    Task<ApiResponse<BicycleDto>> CreateAsync(Guid stationId, BicycleForCreationDto bicycle, CancellationToken cancellationToken = default);

    Task<ApiResponse<string>> UpdateAsync(Guid stationId, Guid entityId, BicycleForUpdationDto entityForUpdation, bool trackChanges, CancellationToken cancellationToken = default);

    Task<ApiResponse<string>> DeleteAsync(Guid stationId, Guid entityId, bool trackChanges, CancellationToken cancellationToken = default);

    Task<ApiResponse<string>> DeactivateBicycleAsync(Guid stationId, Guid bicycleId, CancellationToken cancellationToken = default);

    Task<ApiResponse<string>> ActivateBicycleAsync(Guid stationId, Guid bicycleId, CancellationToken cancellationToken = default);
}
