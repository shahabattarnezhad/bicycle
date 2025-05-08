using Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Bicycle;

public record BicycleForManipulationDto
{
    [Range(1, int.MaxValue, ErrorMessage = "The bicycle status is required")]
    public BicycleStatus BicycleStatus { get; init; }

    [Range(1, int.MaxValue, ErrorMessage = "The bicycle type is required")]
    public BicycleType BicycleType { get; set; }

    [Required(ErrorMessage = "The serial number is required")]
    [MaxLength(50, ErrorMessage = "Maximum length character is 50")]
    public string SerialNumber { get; set; } = string.Empty;

    public bool IsActive { get; init; }

    public Guid? CurrentStationId { get; init; }
}
