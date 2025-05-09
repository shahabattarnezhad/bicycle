using Shared.DTOs.Base;
using Shared.DTOs.Bicycle;
using Shared.DTOs.Payment;
using Shared.DTOs.User;
using Shared.Enums;

namespace Shared.DTOs.Reservation;

public record ReservationDto : BaseEntityDto<Guid>
{
    public DateTime StartTime { get; init; }

    public DateTime EndTime { get; init; }

    public DateTime? ActualEndTime { get; init; }

    public ReservationStatus ReservationStatus { get; init; }

    public decimal TotalCost { get; init; }

    public string? UserId { get; init; }
    public AppUserDto? AppUser { get; init; }

    public Guid BicycleId { get; init; }
    public BicycleDto? Bicycle { get; init; }

    public PaymentDto? Payment { get; init; }
}
