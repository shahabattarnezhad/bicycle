using Shared.Requests.Base;

namespace Shared.Requests;

public class BicycleGpsParameters : RequestParameters
{
    public Guid? BicycleId { get; set; }

    public DateTime? FromTimestamp { get; set; }

    public DateTime? ToTimestamp { get; set; }

    public string? OrderBy { get; set; }
}
