namespace Shared.DTOs.Reservation;

public record ReservationForReturnDto
{
    public Guid ReservationId { get; init; }

    public Guid ReturnStationId { get; init; }
}
