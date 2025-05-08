using Entities.Models.Base;

namespace Entities.Models;

public class Station : BaseEntity<Guid>
{
    public string Name { get; set; } = string.Empty;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public int Capacity { get; set; }

    public TimeOnly OpenTime { get; set; }

    public TimeOnly CloseTime { get; set; }

    public ICollection<Bicycle>? Bicycles { get; set; }
}
