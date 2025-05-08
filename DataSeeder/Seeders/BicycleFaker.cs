using Bogus;
using Entities.Models;
using Shared.Enums;

namespace DataSeeder.Seeders;

public static class BicycleFaker
{
    public static List<Bicycle> GenerateBicycles(int count, List<Station> stations)
    {
        var stationIds = stations.Select(s => s.Id).ToList();

        var bicycleFaker = new Faker<Bicycle>()
            .RuleFor(b => b.Id, f => Guid.NewGuid())
            .RuleFor(b => b.BicycleStatus, f => f.PickRandom<BicycleStatus>())
            .RuleFor(b => b.BicycleType, f => f.PickRandom<BicycleType>())
            .RuleFor(b => b.SerialNumber, f => $"SN-{f.Random.AlphaNumeric(8).ToUpper()}")
            .RuleFor(b => b.IsActive, f => f.Random.Bool(0.8f))
            .RuleFor(b => b.CurrentStationId, f => f.PickRandom(stationIds))
            .RuleFor(b => b.CreatedAt, f => f.Date.Past());

        return bicycleFaker.Generate(count);
    }
}
