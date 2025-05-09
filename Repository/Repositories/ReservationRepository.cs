using Application.Contracts.Interfaces;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Data;
using Shared.Requests.Base;
using Shared.Requests;
using Repository.Extensions;

namespace Repository.Repositories;

public class ReservationRepository : RepositoryBase<Reservation>, IReservationRepository
{
    public ReservationRepository(ApplicationDbContext context) : base(context) { }

    public async Task<PagedList<Reservation>> GetAllAsync(ReservationParameters parameters, bool trackChanges, CancellationToken cancellationToken)
    {
        var reservations = FindAll(trackChanges)
                                              .FilterByStatus(parameters.Status)
                                              .FilterByUserId(parameters.UserId)
                                              .FilterByBicycleId(parameters.BicycleId)
                                              .FilterByStartDateRange(parameters.StartDateFrom, parameters.StartDateTo)
                                              .Sort(parameters.OrderBy);


        return await reservations.ToPagedListAsync(parameters.PageNumber, parameters.PageSize, cancellationToken);
    }

    public async Task<Reservation?> GetAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FindByCondition(entity => entity.Id.Equals(entityId), trackChanges)
                     .SingleOrDefaultAsync(cancellationToken);
    }

    public void CreateEntity(Reservation reservation) => Create(reservation);

    public void UpdateEntity(Reservation reservation) => Update(reservation);

    public void DeleteEntity(Reservation reservation) => Delete(reservation);
}
