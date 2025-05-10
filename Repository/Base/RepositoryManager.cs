using Application.Contracts.Base;
using Application.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Repository.Data;
using Repository.Repositories;
using System.Threading;

namespace Repository.Base;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly ApplicationDbContext _context;

    private readonly Lazy<IStationRepository> _stationRepository;
    private readonly Lazy<IBicycleRepository> _bicycleRepository;
    private readonly Lazy<IDocumentRepository> _documentRepository;
    private readonly Lazy<IReservationRepository> _reservationRepository;
    private readonly Lazy<IBicycleGpsRepository> _bicycleGpsRepository;
    private readonly Lazy<IPaymentRepository> _paymentRepository;

    public RepositoryManager(ApplicationDbContext context)
    {
        _context = context;

        _stationRepository = new Lazy<IStationRepository>(() =>
            new StationRepository(_context));

        _bicycleRepository = new Lazy<IBicycleRepository>(() =>
            new BicycleRepository(_context));

        _documentRepository = new Lazy<IDocumentRepository>(() =>
            new DocumentRepository(_context));

        _reservationRepository = new Lazy<IReservationRepository>(() =>
            new ReservationRepository(_context));

        _bicycleGpsRepository = new Lazy<IBicycleGpsRepository>(() =>
            new BicycleGpsRepository(_context));

        _paymentRepository = new Lazy<IPaymentRepository>(() =>
            new PaymentRepository(_context));
    }

    public IStationRepository Station => _stationRepository.Value;
    public IBicycleRepository Bicycle => _bicycleRepository.Value;
    public IDocumentRepository Document => _documentRepository.Value;
    public IReservationRepository Reservation => _reservationRepository.Value;
    public IBicycleGpsRepository BicycleGps => _bicycleGpsRepository.Value;
    public IPaymentRepository Payment => _paymentRepository.Value;

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        => await _context.Database.BeginTransactionAsync(cancellationToken);

    public async Task SaveAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}
