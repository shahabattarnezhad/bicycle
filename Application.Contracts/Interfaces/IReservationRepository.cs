using Entities.Models;
using Shared.Requests;
using Shared.Requests.Base;

namespace Application.Contracts.Interfaces;

public interface IReservationRepository
{
    Task<PagedList<Reservation>> GetAllAsync(ReservationParameters parameters, bool trackChanges, CancellationToken cancellationToken = default);

    Task<Reservation?> GetAsync(Guid entityId, bool trackChanges, CancellationToken cancellationToken = default);

    Task<bool> ExistsActiveStatusByUserAsync(string userId, bool trackChanges, CancellationToken cancellationToken = default);

    void CreateEntity(Reservation reservation);

    void UpdateEntity(Reservation reservation);

    void DeleteEntity(Reservation reservation);
}
