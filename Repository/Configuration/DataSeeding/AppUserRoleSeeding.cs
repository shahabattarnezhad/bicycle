using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities.Models;

namespace Repository.Configuration.DataSeeding;

public class AppUserRoleSeeding : IEntityTypeConfiguration<AppUserRole>
{
    public void Configure(EntityTypeBuilder<AppUserRole> builder)
    {
        var userRole = new AppUserRole()
        {
            UserId = "43362001-32cd-482e-a7c0-e4d2e528c94e",
            RoleId = "b09cf6dc-a3aa-4cdd-b1e9-f22a1d5d3149"
        };

        builder.HasData(userRole);
    }
}
