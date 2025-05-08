using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration.ModelsConfig;

public class BicycleGpsConfiguration : IEntityTypeConfiguration<BicycleGps>
{
    public void Configure(EntityTypeBuilder<BicycleGps> builder)
    {
        builder.HasKey(entity => entity.Id);

        builder.HasIndex(entity => entity.Timestamp);

        builder.Property(entity => entity.Latitude)
               .HasPrecision(12, 8);

        builder.Property(entity => entity.Longitude)
               .HasPrecision(12, 8);

        builder.HasOne(entity => entity.Bicycle)
               .WithMany(u => u.BicycleGpsRecords)
               .HasForeignKey(r => r.BicycleId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
