using Shared.DTOs.Base;
using Shared.DTOs.Bicycle;

namespace Shared.DTOs.BicycleGps;

public record BicycleGpsDto : BaseEntityDto<Guid>
{
    public decimal Latitude { get; init; }

    public decimal Longitude { get; init; }

    public DateTime Timestamp { get; init; }

    public Guid BicycleId { get; init; }
    public BicycleDto? Bicycle { get; init; }
}
