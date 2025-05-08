using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities.Models;

namespace Repository.Configuration.DataSeeding;

public class RoleSeeding : IEntityTypeConfiguration<AppRole>
{
    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        var roles = new List<AppRole>()
        {
            new AppRole()
            {
                Id = "b09cf6dc-a3aa-4cdd-b1e9-f22a1d5d3149",
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new AppRole()
            {
                Id = "cbf2fd6c-60ba-4bb7-a066-b04cb9b5ea85",
                Name = "Employee",
                NormalizedName = "EMPLOYEE"
            },
            new AppRole()
            {
                Id = "70f7c142-4ba0-43bb-966a-d4b8f7ea4eb6",
                Name = "Customer",
                NormalizedName = "CUSTOMER"
            }
        };

        builder.HasData(roles);
    }
}
