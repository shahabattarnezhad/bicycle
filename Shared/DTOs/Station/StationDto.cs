using Shared.DTOs.Base;

namespace Shared.DTOs.Station;

public record StationDto : BaseEntityDto<Guid>
{
    public string Name { get; init; } = string.Empty;

    public double Latitude { get; init; }

    public double Longitude { get; init; }

    public int Capacity { get; init; }

    public TimeOnly OpenTime { get; init; }

    public TimeOnly CloseTime { get; init; }
}
