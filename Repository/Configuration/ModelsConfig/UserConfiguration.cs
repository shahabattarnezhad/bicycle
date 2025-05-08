using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration.ModelsConfig;

public class UserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(entity => entity.FirstName)
               .HasMaxLength(60);

        builder.Property(entity => entity.LastName)
               .HasMaxLength(60);

        builder.HasMany(entity => entity.Reservations)
               .WithOne(x => x.AppUser)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(entity => entity.Documents)
               .WithOne(d => d.AppUser)
               .HasForeignKey(d => d.AppUserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(entity => entity.VerifiedDocuments)
               .WithOne(d => d.VerifiedByAppUser)
               .HasForeignKey(d => d.VerifiedById)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(entity => entity.Payments)
               .WithOne(x => x.AppUser)
               .HasForeignKey(x => x.AppUserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

