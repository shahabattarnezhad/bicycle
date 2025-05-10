using Application.Contracts.Interfaces;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Data;
using Repository.Extensions;
using Shared.Requests;
using Shared.Requests.Base;

namespace Repository.Repositories;

public class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
{
    public PaymentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PagedList<Payment>> GetAllAsync(PaymentParameters parameters, bool trackChanges, CancellationToken cancellationToken)
    {
        var payments = FindAll(trackChanges)
                                        .Filter(parameters)
                                        .Sort(parameters.OrderBy);


        return await payments.ToPagedListAsync(parameters.PageNumber, parameters.PageSize, cancellationToken);
    }

    public async Task<Payment>? GetAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(entity => entity.Id.Equals(entityId), trackChanges)
                     .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<int> CountAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FindAll(trackChanges).CountAsync(cancellationToken);
    }

    public void CreateEntity(Payment payment) => Create(payment);

    public void UpdateEntity(Payment payment) => Update(payment);

    public void DeleteEntity(Payment payment) => Delete(payment);
}
