using Application.Contracts.Interfaces;

namespace Application.Contracts.Base;

public interface IRepositoryManager
{
    IStationRepository Station { get; }
    IBicycleRepository Bicycle { get; }
    IDocumentRepository Document { get; }
    IReservationRepository Reservation { get; }

    Task SaveAsync(CancellationToken cancellationToken = default);
}
