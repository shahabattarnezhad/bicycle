using Entities.Models;
using Shared.Requests;
using Shared.Requests.Base;

namespace Application.Contracts.Interfaces;

public interface IStationRepository
{
    Task<PagedList<Station>> GetAllAsync(StationParameters parameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<IEnumerable<Station>> GetAllAsync(bool trackChanges, CancellationToken cancellationToken = default);

    Task<Station?> GetAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default);

    void CreateEntity(Station station);

    void UpdateEntity(Station station);

    void DeleteEntity(Station station);
}
