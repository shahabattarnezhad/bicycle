using Entities.Models.Base;
using Shared.Enums;

namespace Entities.Models;

public class Bicycle : BaseEntity<Guid>
{
    public BicycleStatus BicycleStatus { get; set; }

    public BicycleType BicycleType { get; set; }

    public string SerialNumber { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public Guid CurrentStationId { get; set; }
    public Station? CurrentStation { get; set; }

    public ICollection<BicycleGps>? BicycleGpsRecords { get; set; }
    public ICollection<Reservation>? Reservations { get; set; }
}
