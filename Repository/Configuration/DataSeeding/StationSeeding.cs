using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities.Models;
using Repository.Configuration.DataSeeding.Constants;

namespace Repository.Configuration.DataSeeding;

public class StationSeeding : IEntityTypeConfiguration<Station>
{
    public void Configure(EntityTypeBuilder<Station> builder)
    {
        var station = new Station()
        {
            Id = SeedConstants.StationAId,
            Name = "Station A",
            Latitude = 35.6895,
            Longitude = 139.6917,
            Capacity = 20,
            OpenTime = new TimeOnly(8, 0, 0),
            CloseTime = new TimeOnly(20, 0, 0),
            CreatedAt = DateTime.UtcNow,
        };

        builder.HasData(station);
    }
}
