using Shared.Enums;
using Shared.Requests.Base;

namespace Shared.Requests;

public class DocumentParameters : RequestParameters
{
    public string? SearchTerm { get; set; }

    public string? OrderBy { get; set; } = "uploadedAt";

    public DocumentStatus? Status { get; set; }

    public string? UserId { get; set; }
}
