using Shared.Enums;
using Shared.Requests.Base;

namespace Shared.Requests;

public class PaymentParameters : RequestParameters
{
    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public decimal? MinAmount { get; set; }

    public decimal? MaxAmount { get; set; }

    public PaymentMethod? PaymentMethod { get; set; }

    public string? AppUserId { get; set; }

    public Guid? ReservationId { get; set; }

    public string? OrderBy { get; set; }
}
