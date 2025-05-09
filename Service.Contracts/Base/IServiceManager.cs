using Service.Contracts.Interfaces;
using Service.Contracts.Interfaces.Auth;

namespace Service.Contracts.Base;

public interface IServiceManager
{
    IAuthService AuthService { get; }
    IStationService StationService { get; }
    IBicycleService BicycleService { get; }
    IDocumentService DocumentService { get; }
    IReservationService ReservationService { get; }
}
