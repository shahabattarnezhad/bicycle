namespace Shared.DTOs.BicycleGps;

public record BicycleGpsForCreationDto
{
    public decimal Latitude { get; init; }

    public decimal Longitude { get; init; }

    public DateTime Timestamp { get; init; }

    public Guid BicycleId { get; init; }
}
