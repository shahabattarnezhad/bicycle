using Shared.Enums;

namespace Shared.DTOs.Payment;

public record PaymentForCreationDto
{
    public decimal Amount { get; init; }

    public DateTime PaymentDate { get; init; }

    public PaymentMethod PaymentMethod { get; init; }

    public Guid ReservationId { get; init; }

    public string AppUserId { get; init; } = default!;
}
