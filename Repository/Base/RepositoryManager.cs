using Application.Contracts.Base;
using Application.Contracts.Interfaces;
using Repository.Data;
using Repository.Repositories;

namespace Repository.Base;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly ApplicationDbContext _context;

    private readonly Lazy<IStationRepository> _stationRepository;
    private readonly Lazy<IBicycleRepository> _bicycleRepository;
    private readonly Lazy<IDocumentRepository> _documentRepository;

    public RepositoryManager(ApplicationDbContext context)
    {
        _context = context;

        _stationRepository = new Lazy<IStationRepository>(() =>
            new StationRepository(_context));

        _bicycleRepository = new Lazy<IBicycleRepository>(() =>
            new BicycleRepository(_context));

        _documentRepository = new Lazy<IDocumentRepository>(() =>
            new DocumentRepository(_context));
    }

    public IStationRepository Station =>  _stationRepository.Value;
    public IBicycleRepository Bicycle => _bicycleRepository.Value;
    public IDocumentRepository Document => _documentRepository.Value;

    public async Task SaveAsync(CancellationToken cancellationToken = default) 
        => await _context.SaveChangesAsync(cancellationToken);
}
