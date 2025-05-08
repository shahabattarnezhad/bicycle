using Entities.Models;
using Shared.Requests;
using Shared.Requests.Base;

namespace Application.Contracts.Interfaces;

public interface IBicycleRepository
{
    Task<PagedList<Bicycle>> GetAllAsync(Guid stationId, BicycleParameters bicycleParameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<PagedList<Bicycle>> GetAvailableAsync(Guid stationId, BicycleParameters bicycleParameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<PagedList<Bicycle>> GetWithGpsRecordsAsync(Guid stationId, BicycleParameters bicycleParameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<PagedList<Bicycle>> GetInActiveAsync(Guid stationId, BicycleParameters bicycleParameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<PagedList<Bicycle>> GetElectricBicyclesAsync(Guid stationId, BicycleParameters bicycleParameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<PagedList<Bicycle>> GetStandardBicyclesAsync(Guid stationId, BicycleParameters bicycleParameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<IEnumerable<Bicycle>> GetAllForStationAsync(Guid stationId, bool trackChanges, CancellationToken cancellationToken = default);

    Task<Bicycle?> GetAsync(Guid bicycleId, bool trackChanges, CancellationToken cancellationToken = default);

    Task<Bicycle?> GetAsync(Guid stationId, Guid bicycleId, bool trackChanges, CancellationToken cancellationToken = default);

    Task<Bicycle?> GetBySerialNumberAsync(string SerialNumber, bool trackChanges, CancellationToken cancellationToken = default);

    Task<Bicycle?> GetWithDetailsAsync(Guid stationId, Guid bicycleId, bool trackChanges, CancellationToken cancellationToken = default);

    Task<int> CountAsync(Guid stationId, bool trackChanges, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Guid stationId, Guid bicycleId, CancellationToken cancellationToken = default);



    void CreateBicycle(Bicycle bicycle);

    void UpdateBicycle(Bicycle bicycle);

    void DeleteBicycle(Bicycle bicycle);

    Task DeactivateBicycleAsync(Guid stationId, Guid bicycleId, CancellationToken cancellationToken = default);

    Task ActivateBicycleAsync(Guid stationId, Guid bicycleId, CancellationToken cancellationToken = default);
}
