using Application.Contracts.Base;
using AutoMapper;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Contracts.Base;
using Service.Contracts.Interfaces;
using Service.Contracts.Interfaces.Auth;
using Service.Contracts.Interfaces.Helpers;
using Service.Services;
using Service.Services.Auth;

namespace Service.Base;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IAuthService> _authService;
    private readonly Lazy<IStationService> _stationService;
    private readonly Lazy<IBicycleService> _bicycleService;
    private readonly Lazy<IDocumentService> _documentService;
    private readonly Lazy<IReservationService> _reservationService;
    private readonly Lazy<IBicycleGpsService> _bicycleGpsService;
    private readonly Lazy<IPaymentService> _paymentService;

    public ServiceManager(
        IRepositoryManager repositoryManager,
        IMapper mapper,
        IMemoryCacheService cache,
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        SignInManager<AppUser> signInManager,
        IConfiguration configuration,
        IFileService fileService,
        IUserContextService userContextService)
    {
        _authService = new Lazy<IAuthService>(() =>
                            new AuthService(mapper, userManager, signInManager, configuration, roleManager));

        _stationService = new Lazy<IStationService>(() =>
                          new StationService(repositoryManager, mapper, cache));

        _bicycleService = new Lazy<IBicycleService>(() =>
                            new BicycleService(repositoryManager, mapper, cache));

        _documentService = new Lazy<IDocumentService>(() =>
                            new DocumentService(repositoryManager, mapper, cache, fileService, userContextService));

        _reservationService = new Lazy<IReservationService>(() =>
                            new ReservationService(repositoryManager, mapper, cache, userContextService));

        _bicycleGpsService = new Lazy<IBicycleGpsService>(() =>
                            new BicycleGpsService(repositoryManager, mapper, cache));

        _paymentService = new Lazy<IPaymentService>(() =>
                            new PaymentService(repositoryManager, mapper, cache, userContextService));
    }

    public IAuthService AuthService => _authService.Value;
    public IStationService StationService => _stationService.Value;
    public IBicycleService BicycleService => _bicycleService.Value;
    public IDocumentService DocumentService => _documentService.Value;
    public IReservationService ReservationService => _reservationService.Value;
    public IBicycleGpsService BicycleGpsService => _bicycleGpsService.Value;
    public IPaymentService PaymentService => _paymentService.Value;
}
