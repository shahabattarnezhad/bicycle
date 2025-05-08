using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration.ModelsConfig;

public class BicycleConfiguration : IEntityTypeConfiguration<Bicycle>
{
    public void Configure(EntityTypeBuilder<Bicycle> builder)
    {
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.SerialNumber)
               .IsRequired()
               .HasMaxLength(50);

        builder.HasMany(entity => entity.BicycleGpsRecords)
               .WithOne(x => x.Bicycle)
               .HasForeignKey(x => x.BicycleId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(entity => entity.Reservations)
               .WithOne(x => x.Bicycle)
               .HasForeignKey(x => x.BicycleId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(entity => entity.CurrentStation)
               .WithMany(u => u.Bicycles)
               .HasForeignKey(r => r.CurrentStationId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
