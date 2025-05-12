using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.Configuration.DataSeeding;
using Repository.Configuration.ModelsConfig;

namespace Repository.Data;

//public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, string>
public class ApplicationDbContext : IdentityDbContext<
                                                      AppUser,
                                                      AppRole,
                                                      string,
                                                      IdentityUserClaim<string>,
                                                      AppUserRole,
                                                      IdentityUserLogin<string>,
                                                      IdentityRoleClaim<string>,
                                                      IdentityUserToken<string>
                                                     >
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new AppUserRoleConfiguration());
        builder.ApplyConfiguration(new BicycleConfiguration());
        builder.ApplyConfiguration(new BicycleGpsConfiguration());
        builder.ApplyConfiguration(new DocumentConfiguration());
        builder.ApplyConfiguration(new PaymentConfiguration());
        builder.ApplyConfiguration(new ReservationConfiguration());
        builder.ApplyConfiguration(new StationConfiguration());

        builder.ApplyConfiguration(new RoleSeeding());
        builder.ApplyConfiguration(new UserSeeding());
        builder.ApplyConfiguration(new AppUserRoleSeeding());
        builder.ApplyConfiguration(new BicycleSeeding());
        builder.ApplyConfiguration(new StationSeeding());
    }

    public DbSet<Bicycle>? Bicycles { get; set; }
    public DbSet<BicycleGps>? BicycleGps { get; set; }
    public DbSet<Document>? Documents { get; set; }
    public DbSet<Payment>? Payments { get; set; }
    public DbSet<Reservation>? Reservations { get; set; }
    public DbSet<Station>? Stations { get; set; }
}
