using Shared.DTOs.Station;
using Shared.Requests;
using Shared.Responses;

namespace Service.Contracts.Interfaces;

public interface IStationService
{
    Task<ApiResponse<IEnumerable<StationDto>>> GetAllAsync(StationParameters parameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<ApiResponse<StationDto>>? GetAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default);

    Task<ApiResponse<StationDto>> CreateAsync(StationForCreationDto entityForCreation, CancellationToken cancellationToken = default);

    Task<ApiResponse<string>> DeleteAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default);

    Task<ApiResponse<string>> UpdateAsync(Guid entityId, StationForUpdationDto entityForUpdation, bool trackChanges, CancellationToken cancellationToken = default);
}
