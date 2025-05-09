using Bogus;
using Entities.Models;

namespace DataSeeder.Seeders;

public static class StationFaker
{
    public static List<Station> GenerateStations(int count)
    {
        var stationFaker = new Faker<Station>()
            .RuleFor(s => s.Id, f => Guid.NewGuid())
            .RuleFor(s => s.Name, f => f.Address.City() + " Station")
            .RuleFor(s => s.Latitude, f => f.Address.Latitude())
            .RuleFor(s => s.Longitude, f => f.Address.Longitude())
            .RuleFor(s => s.Capacity, f => f.Random.Int(5, 20))
            .RuleFor(s => s.AvailableBicycles, f => f.Random.Int(0, 20))
            .RuleFor(s => s.OpenTime, _ => new TimeOnly(6, 0))
            .RuleFor(s => s.CloseTime, _ => new TimeOnly(22, 0))
            .RuleFor(s => s.CreatedAt, f => f.Date.Past());

        return stationFaker.Generate(count);
    }
}
