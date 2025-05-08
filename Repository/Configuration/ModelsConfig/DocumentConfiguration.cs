using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration.ModelsConfig;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Path)
               .IsRequired()
               .HasMaxLength(500);

        builder.HasOne(entity => entity.AppUser)
               .WithMany(u => u.Documents)
               .HasForeignKey(r => r.AppUserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(entity => entity.VerifiedByAppUser)
               .WithMany(u => u.VerifiedDocuments)
               .HasForeignKey(r => r.VerifiedById)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
