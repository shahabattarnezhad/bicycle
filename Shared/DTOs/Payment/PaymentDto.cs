using Shared.DTOs.Base;
using Shared.DTOs.Reservation;
using Shared.DTOs.User;
using Shared.Enums;

namespace Shared.DTOs.Payment;

public record PaymentDto : BaseEntityDto<Guid>
{
    public decimal Amount { get; init; }

    public DateTime PaymentDate { get; init; }

    public PaymentMethod PaymentMethod { get; init; }

    public Guid ReservationId { get; init; }
    public ReservationDto? Reservation { get; init; }

    public string? AppUserId { get; init; }
    public AppUserDto? AppUser { get; init; }
}
