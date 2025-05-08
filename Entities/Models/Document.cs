using Entities.Models.Base;
using Shared.Enums;

namespace Entities.Models;

public class Document : BaseEntity<Guid>
{
    public string Path { get; set; } = string.Empty;

    public DateTime UploadedAt { get; set; }

    public DateTime VerifiedAt { get; set; }

    public string? VerifiedById { get; set; }
    public AppUser? VerifiedByAppUser { get; set; }

    public string? AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    public DocumentStatus Status { get; set; } = DocumentStatus.Pending;

    public string? Notes { get; set; }
}
