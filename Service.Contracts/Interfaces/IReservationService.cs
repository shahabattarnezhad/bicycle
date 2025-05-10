using Shared.DTOs.Reservation;
using Shared.Requests;
using Shared.Responses;

namespace Service.Contracts.Interfaces;

public interface IReservationService
{
    Task<ApiResponse<IEnumerable<ReservationDto>>> GetAllAsync(ReservationParameters parameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<ApiResponse<ReservationDto>>? GetAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default);

    Task<ApiResponse<ReservationDto>> CreateAsync(ReservationForCreationDto entityForCreation, CancellationToken cancellationToken = default);

    Task<ApiResponse<string>> ReturnBikeAsync(ReservationForReturnDto dto, CancellationToken cancellationToken = default);

    Task<ApiResponse<string>> DeleteAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default);
}
