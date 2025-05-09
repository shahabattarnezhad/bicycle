namespace Shared.DTOs.Reservation;

public record ReservationForCreationDto
{
    public DateTime StartTime { get; init; }

    public DateTime EndTime { get; init; }

    public Guid BicycleId { get; init; }
}
