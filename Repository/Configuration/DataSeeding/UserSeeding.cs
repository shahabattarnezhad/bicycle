using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities.Models;
using Shared.Enums;

namespace Repository.Configuration.DataSeeding;

public class UserSeeding : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        var user = new AppUser()
        {
            Id = "43362001-32cd-482e-a7c0-e4d2e528c94e",
            Email = "atarnezhad@gmail.com",
            FirstName = "Shahab",
            LastName = "Attarnejad",
            UserName = "atarnezhad",
            NormalizedUserName = "ATARNEZHAD",
            NormalizedEmail = "ATARNEZHAD@GMAIL.COM",
            Status = UserStatus.Approved,
            LockoutEnabled = true,
            EmailConfirmed = true,
            SecurityStamp = string.Empty,
            PasswordHash = new PasswordHasher<AppUser>()
                               .HashPassword(null!, "Comet@123"),
        };

        builder.HasData(user);
    }
}
