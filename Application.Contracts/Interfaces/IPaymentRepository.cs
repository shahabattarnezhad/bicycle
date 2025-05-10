using Entities.Models;
using Shared.Requests;
using Shared.Requests.Base;

namespace Application.Contracts.Interfaces;

public interface IPaymentRepository
{
    Task<PagedList<Payment>> GetAllAsync(PaymentParameters parameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<Payment?> GetAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default);

    Task<int> CountAsync(bool trackChanges, CancellationToken cancellationToken = default);

    void CreateEntity(Payment payment);

    void UpdateEntity(Payment payment);

    void DeleteEntity(Payment payment);
}
