using Entities.Models.Base;
using Shared.Enums;

namespace Entities.Models;

public class Reservation : BaseEntity<Guid>
{
    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public DateTime? ActualEndTime { get; set; }

    public ReservationStatus ReservationStatus { get; set; }

    public decimal TotalCost { get; set; }

    public string UserId { get; set; } = default!;
    public AppUser? AppUser { get; set; }

    public Guid BicycleId { get; set; }
    public Bicycle? Bicycle { get; set; }

    public Payment? Payment { get; set; }
}
