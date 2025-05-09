using Application.Contracts.Interfaces;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Data;
using Repository.Extensions;
using Shared.Requests;
using Shared.Requests.Base;

namespace Repository.Repositories;

public class StationRepository : RepositoryBase<Station>, IStationRepository
{
    public StationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PagedList<Station>> GetAllAsync(StationParameters parameters, bool trackChanges, CancellationToken cancellationToken)
    {
        var stations = FindAll(trackChanges)
                                        .Filter(parameters.SearchTerm)
                                        .Sort(parameters.OrderBy);


        return await stations.ToPagedListAsync(parameters.PageNumber, parameters.PageSize, cancellationToken);
    }

    public async Task<IEnumerable<Station>> GetAllAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FindAll(trackChanges)
                     .OrderBy(c => c.Name)
                     .ToListAsync(cancellationToken);
    }

    public async Task<Station>? GetAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(entity => entity.Id.Equals(entityId), trackChanges)
                     .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<int> CountAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FindAll(trackChanges).CountAsync(cancellationToken);
    }

    public void CreateEntity(Station station) => Create(station);

    public void UpdateEntity(Station station) => Update(station);

    public void DeleteEntity(Station station) => Delete(station);
}
