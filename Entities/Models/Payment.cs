using Entities.Models.Base;
using Shared.Enums;

namespace Entities.Models;

public class Payment : BaseEntity<Guid>
{
    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public Guid ReservationId { get; set; }
    public Reservation? Reservation { get; set; }

    public string AppUserId { get; set; } = default!;
    public AppUser? AppUser { get; set; }
}
