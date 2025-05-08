using Shared.DTOs.Base;
using Shared.DTOs.Station;
using Shared.Enums;

namespace Shared.DTOs.Bicycle;

public record BicycleDto : BaseEntityDto<Guid>
{
    public BicycleStatus BicycleStatus { get; init; }

    public BicycleType BicycleType { get; set; }

    public string SerialNumber { get; set; } = string.Empty;

    public bool IsActive { get; init; }

    public Guid CurrentStationId { get; init; }
    public StationDto? CurrentStation { get; init; }
}
