using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration.ModelsConfig;

public class StationConfiguration : IEntityTypeConfiguration<Station>
{
    public void Configure(EntityTypeBuilder<Station> builder)
    {
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Name)
               .IsRequired()
               .HasMaxLength(60);

        builder.Property(entity => entity.Latitude)
               .HasPrecision(12, 8);

        builder.Property(entity => entity.Longitude)
               .HasPrecision(12, 8);

        builder.HasMany(entity => entity.Bicycles)
               .WithOne(x => x.CurrentStation)
               .HasForeignKey(x => x.CurrentStationId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
