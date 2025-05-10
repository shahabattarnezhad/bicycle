using Shared.DTOs.BicycleGps;
using Shared.Requests;
using Shared.Responses;

namespace Service.Contracts.Interfaces;

public interface IBicycleGpsService
{
    Task<ApiResponse<IEnumerable<BicycleGpsDto>>> GetAllAsync(BicycleGpsParameters parameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<ApiResponse<BicycleGpsDto>>? GetAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default);

    Task<ApiResponse<BicycleGpsDto>> CreateAsync(BicycleGpsForCreationDto entityForCreation, CancellationToken cancellationToken = default);

    Task<ApiResponse<string>> DeleteAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default);
}
