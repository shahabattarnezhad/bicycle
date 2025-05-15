using Application.Contracts.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DataSeeder.Seeders;

public class SeederRunner
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SeederRunner> _logger;

    public SeederRunner(IServiceProvider serviceProvider, ILogger<SeederRunner> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;

        _logger.LogInformation("Starting seeding process...");

        var repository = scopedServices.GetRequiredService<IRepositoryManager>();

        var stationCount = await repository.Station.CountAsync(trackChanges: false, cancellationToken);
        if (stationCount > 1)
        {
            _logger.LogInformation("Seeding skipped. Database already contains data.");
            return;
        }

        var stations = StationFaker.GenerateStations(10);
        foreach (var station in stations)
            repository.Station.CreateEntity(station);

        await repository.SaveAsync(cancellationToken);

        var bicycles = BicycleFaker.GenerateBicycles(20, stations);
        foreach (var bicycle in bicycles)
            repository.Bicycle.CreateBicycle(bicycle);

        await repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Seeding completed.");
    }
}
