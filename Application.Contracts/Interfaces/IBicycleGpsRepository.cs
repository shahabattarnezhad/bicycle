using Entities.Models;
using Shared.Requests;
using Shared.Requests.Base;

namespace Application.Contracts.Interfaces;

public interface IBicycleGpsRepository
{
    Task<PagedList<BicycleGps>> GetAllAsync(BicycleGpsParameters parameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<BicycleGps?> GetAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default);

    Task<int> CountAsync(bool trackChanges, CancellationToken cancellationToken = default);

    void CreateEntity(BicycleGps bicycleGps);

    void UpdateEntity(BicycleGps bicycleGps);

    void DeleteEntity(BicycleGps bicycleGps);
}
