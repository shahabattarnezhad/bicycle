using Application.Contracts.Interfaces;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Data;
using Repository.Extensions;
using Shared.Requests;
using Shared.Requests.Base;

namespace Repository.Repositories;

public class BicycleGpsRepository : RepositoryBase<BicycleGps>, IBicycleGpsRepository
{
    public BicycleGpsRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PagedList<BicycleGps>> GetAllAsync(BicycleGpsParameters parameters, bool trackChanges, CancellationToken cancellationToken)
    {
        var bicycleGpsList = FindAll(trackChanges)
                                               .Filter(parameters)
                                               .Sort(parameters.OrderBy);


        return await bicycleGpsList.ToPagedListAsync(parameters.PageNumber, parameters.PageSize, cancellationToken);
    }

    public async Task<BicycleGps>? GetAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(entity => entity.Id.Equals(entityId), trackChanges)
                     .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<int> CountAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FindAll(trackChanges).CountAsync(cancellationToken);
    }

    public void CreateEntity(BicycleGps bicycleGps) => Create(bicycleGps);

    public void UpdateEntity(BicycleGps bicycleGps) => Update(bicycleGps);

    public void DeleteEntity(BicycleGps bicycleGps) => Delete(bicycleGps);
}
