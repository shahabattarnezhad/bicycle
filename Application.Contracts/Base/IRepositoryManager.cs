using Application.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Contracts.Base;

public interface IRepositoryManager
{
    IStationRepository Station { get; }
    IBicycleRepository Bicycle { get; }
    IDocumentRepository Document { get; }
    IReservationRepository Reservation { get; }

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(CancellationToken cancellationToken = default);
}
