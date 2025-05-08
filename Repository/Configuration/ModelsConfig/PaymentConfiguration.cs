using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration.ModelsConfig;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Amount)
               .HasPrecision(12, 2);

        builder.HasOne(entity => entity.AppUser)
               .WithMany(u => u.Payments)
               .HasForeignKey(r => r.AppUserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
