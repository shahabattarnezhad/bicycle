using Bogus;
using Entities.Models;

namespace DataSeeder.Seeders;

public static class StationFaker
{
    private static readonly List<(string Name, double Latitude, double Longitude)> IranianCities = new()
    {
        ("تهران", 35.6892, 51.3890),
        ("مشهد", 36.2605, 59.6168),
        ("اصفهان", 32.6539, 51.6660),
        ("شیراز", 29.5918, 52.5836),
        ("تبریز", 38.0962, 46.2738),
        ("اهواز", 31.3203, 48.6692),
        ("کرمان", 30.2839, 57.0834),
        ("رشت", 37.2808, 49.5832),
        ("کرج", 35.8400, 50.9391),
        ("ارومیه", 37.5485, 45.0728)
    };

    public static List<Station> GenerateStations(int count)
    {
        var faker = new Faker("fa");

        var stationFaker = new Faker<Station>("fa")
            .RuleFor(s => s.Id, f => Guid.NewGuid())
            .RuleFor(s => s.CreatedAt, f => f.Date.Past())
            .RuleFor(s => s.UpdatedAt, f => DateTime.Now)
            .RuleFor(s => s.Capacity, f => f.Random.Int(5, 20))
            .RuleFor(s => s.AvailableBicycles, (f, s) => f.Random.Int(0, s.Capacity))
            .RuleFor(s => s.OpenTime, _ => new TimeOnly(6, 0))
            .RuleFor(s => s.CloseTime, _ => new TimeOnly(22, 0))
            .RuleFor(s => s.Name, (f, s) =>
            {
                var city = f.PickRandom(IranianCities);
                s.Latitude = city.Latitude + f.Random.Double(-0.01, 0.01);
                s.Longitude = city.Longitude + f.Random.Double(-0.01, 0.01);
                return $"ایستگاه {city.Name}";
            });

        return stationFaker.Generate(count);
    }
}
