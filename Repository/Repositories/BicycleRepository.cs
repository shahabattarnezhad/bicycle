using Application.Contracts.Interfaces;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Data;
using Repository.Extensions;
using Shared.Enums;
using Shared.Requests;
using Shared.Requests.Base;

namespace Repository.Repositories;

public class BicycleRepository : RepositoryBase<Bicycle>, IBicycleRepository
{
    public BicycleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<int> CountAsync(Guid stationId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(entity => entity.CurrentStationId.Equals(stationId), trackChanges)
                     .CountAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid stationId, Guid bicycleId, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(entity => entity.CurrentStationId.Equals(stationId) &&
                                              entity.Id.Equals(bicycleId), false)
                     .AnyAsync(cancellationToken);
    }

    public async Task<PagedList<Bicycle>> GetAllAsync(Guid stationId, BicycleParameters bicycleParameters, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var query = FindByCondition(entity => entity.CurrentStationId.Equals(stationId), trackChanges)
                                    .Filter(bicycleParameters.SearchTerm)
                                    .Sort(bicycleParameters.OrderBy);

        return await query.ToPagedListAsync(bicycleParameters.PageNumber, bicycleParameters.PageSize, cancellationToken);
    }

    public async Task<IEnumerable<Bicycle>> GetAllForStationAsync(Guid stationId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(entity => entity.CurrentStationId.Equals(stationId), trackChanges)
                     .ToListAsync(cancellationToken);
    }

    public async Task<Bicycle?> GetAsync(Guid bicycleId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(entity => entity.Id.Equals(bicycleId), trackChanges)
                     .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<Bicycle?> GetAsync(Guid stationId, Guid bicycleId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(entity => entity.CurrentStationId.Equals(stationId) &&
                                              entity.Id.Equals(bicycleId), trackChanges)
                     .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<Bicycle>> GetAvailableAsync(Guid stationId, BicycleParameters bicycleParameters, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var query = FindByCondition(entity => entity.CurrentStationId.Equals(stationId) &&
                                                           entity.BicycleStatus.Equals(BicycleStatus.Available), trackChanges)
                                    .Filter(bicycleParameters.SearchTerm)
                                    .Sort(bicycleParameters.OrderBy);

        return await query.ToPagedListAsync(bicycleParameters.PageNumber, bicycleParameters.PageSize, cancellationToken);
    }

    public async Task<PagedList<Bicycle>> GetInActiveAsync(Guid stationId, BicycleParameters bicycleParameters, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var query = FindByCondition(entity => entity.CurrentStationId.Equals(stationId) &&
                                                                    !entity.IsActive, trackChanges)
                                    .Filter(bicycleParameters.SearchTerm)
                                    .Sort(bicycleParameters.OrderBy);

        return await query.ToPagedListAsync(bicycleParameters.PageNumber, bicycleParameters.PageSize, cancellationToken);
    }

    public async Task<Bicycle?> GetWithDetailsAsync(Guid stationId, Guid bicycleId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(entity => entity.CurrentStationId.Equals(stationId) &&
                                              entity.Id.Equals(bicycleId), trackChanges)
                     .Include(entity => entity.CurrentStation)
                     .Include(entity => entity.Reservations)
                     .Include(entity => entity.BicycleGpsRecords)
                     .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<Bicycle>> GetWithGpsRecordsAsync(Guid stationId, BicycleParameters bicycleParameters, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var query =
            FindByCondition(entity => entity.CurrentStationId.Equals(stationId), trackChanges)
            .Include(entity => entity.BicycleGpsRecords)
            .Where(entity => entity.BicycleGpsRecords!.Any())
            .Filter(bicycleParameters.SearchTerm)
            .Sort(bicycleParameters.OrderBy);

        return await query.ToPagedListAsync(bicycleParameters.PageNumber, bicycleParameters.PageSize, cancellationToken);
    }

    public void CreateBicycle(Bicycle bicycle)
    {
        Create(bicycle);
    }

    public void DeleteBicycle(Bicycle bicycle) => Delete(bicycle);

    public void UpdateBicycle(Bicycle bicycle) => Update(bicycle);

    public async Task ActivateBicycleAsync(Guid stationId, Guid bicycleId, CancellationToken cancellationToken = default)
    {
        var bicycle =
            await GetAsync(stationId, bicycleId, true, cancellationToken);

        bicycle.IsActive = true;
        bicycle.BicycleStatus = BicycleStatus.Available;
        Update(bicycle);
    }

    public async Task DeactivateBicycleAsync(Guid stationId, Guid bicycleId, CancellationToken cancellationToken = default)
    {
        var bicycle =
            await GetAsync(stationId, bicycleId, true, cancellationToken);

        bicycle.IsActive = false;
        bicycle.BicycleStatus = BicycleStatus.Inavailable;
        Update(bicycle);
    }

    public async Task<PagedList<Bicycle>> GetElectricBicyclesAsync(Guid stationId, BicycleParameters bicycleParameters, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var query = FindByCondition(entity => entity.CurrentStationId.Equals(stationId) &&
                                                           entity.BicycleType.Equals(BicycleType.Electric), trackChanges)
                                    .Filter(bicycleParameters.SearchTerm)
                                    .Sort(bicycleParameters.OrderBy);
        return await query.ToPagedListAsync(bicycleParameters.PageNumber, bicycleParameters.PageSize, cancellationToken);
    }

    public async Task<PagedList<Bicycle>> GetStandardBicyclesAsync(Guid stationId, BicycleParameters bicycleParameters, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var query = FindByCondition(entity => entity.CurrentStationId.Equals(stationId) &&
                                                           entity.BicycleType.Equals(BicycleType.Standard), trackChanges)
                                    .Filter(bicycleParameters.SearchTerm)
                                    .Sort(bicycleParameters.OrderBy);
        return await query.ToPagedListAsync(bicycleParameters.PageNumber, bicycleParameters.PageSize, cancellationToken);
    }

    public async Task<Bicycle?> GetBySerialNumberAsync(string SerialNumber, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(entity => entity.SerialNumber.Equals(SerialNumber), trackChanges)
                     .SingleOrDefaultAsync(cancellationToken);
    }
}
