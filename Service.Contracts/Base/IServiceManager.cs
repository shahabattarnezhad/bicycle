using Service.Contracts.Interfaces;
using Service.Contracts.Interfaces.Auth;

namespace Service.Contracts.Base;

public interface IServiceManager
{
    IStationService StationService { get; }
    IBicycleService BicycleService { get; }
    IDocumentService DocumentService { get; }
    IAuthService AuthService { get; }
}
