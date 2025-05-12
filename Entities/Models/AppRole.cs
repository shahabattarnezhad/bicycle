using Entities.Models.Base;
using Microsoft.AspNetCore.Identity;

namespace Entities.Models;

public class AppRole : IdentityRole
{
    public ICollection<AppUserRole>? UserRoles { get; set; }
}
