using Microsoft.AspNetCore.Identity;
using Shared.Enums;

namespace Entities.Models;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public UserStatus Status { get; set; }

    public ICollection<Document>? Documents { get; set; }
    public ICollection<Document>? VerifiedDocuments { get; set; }
    public ICollection<Reservation>? Reservations { get; set; }
    public ICollection<Payment>? Payments { get; set; }
}
