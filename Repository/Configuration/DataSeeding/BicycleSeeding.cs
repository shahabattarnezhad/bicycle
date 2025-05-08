using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities.Models;
using Repository.Configuration.DataSeeding.Constants;
using Shared.Enums;

namespace Repository.Configuration.DataSeeding;

public class BicycleSeeding : IEntityTypeConfiguration<Bicycle>
{
    public void Configure(EntityTypeBuilder<Bicycle> builder)
    {
        var bicycle = new Bicycle()
        {
            Id = SeedConstants.BicycleAId,
            BicycleStatus = BicycleStatus.Available,
            BicycleType = BicycleType.Standard,
            SerialNumber = "SN-U3URPROK",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CurrentStationId = SeedConstants.StationAId,
        };

        builder.HasData(bicycle);
    }
}
