using Entities.Models.Base;

namespace Entities.Models;

public class BicycleGps : BaseEntity<Guid>
{
    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public DateTime Timestamp { get; set; }

    public Guid BicycleId { get; set; }
    public Bicycle? Bicycle { get; set; }
}
