using Shared.Enums;
using Shared.Requests.Base;

namespace Shared.Requests;

public class ReservationParameters : RequestParameters
{
    public string? SearchTerm { get; set; }

    public ReservationStatus? Status { get; set; }

    public DateTime? StartDateFrom { get; set; }

    public DateTime? StartDateTo { get; set; }

    public string? UserId { get; set; }

    public Guid? BicycleId { get; set; }

    public string? OrderBy { get; set; } = "startTime";
}
