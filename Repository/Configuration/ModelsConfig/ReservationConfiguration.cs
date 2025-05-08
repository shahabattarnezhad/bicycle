using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration.ModelsConfig;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.TotalCost)
               .HasPrecision(12, 2);

        builder.HasOne(entity => entity.AppUser)
               .WithMany(u => u.Reservations)
               .HasForeignKey(r => r.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(entity => entity.Bicycle)
               .WithMany(b => b.Reservations)
               .HasForeignKey(r => r.BicycleId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(entity => entity.Payment)
               .WithOne(p => p.Reservation)
               .HasForeignKey<Payment>(p => p.ReservationId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
