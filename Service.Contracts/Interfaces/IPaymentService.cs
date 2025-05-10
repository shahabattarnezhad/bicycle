using Shared.DTOs.Payment;
using Shared.Requests;
using Shared.Responses;

namespace Service.Contracts.Interfaces;

public interface IPaymentService
{
    Task<ApiResponse<IEnumerable<PaymentDto>>> GetAllAsync(PaymentParameters parameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<ApiResponse<PaymentDto>>? GetAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default);

    Task<ApiResponse<PaymentDto>> CreateAsync(PaymentForCreationDto entityForCreation, CancellationToken cancellationToken = default);

    Task<ApiResponse<string>> DeleteAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default);
}
