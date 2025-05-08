using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Station;

public abstract record StationForManipulationDto
{
    [Required(ErrorMessage = "The name is required")]
    [MaxLength(60, ErrorMessage = "The maximum lenght characters is 60.")]
    public string Name { get; init; } = string.Empty;


    [Required(ErrorMessage = "The latitude is required")]
    public double? Latitude { get; init; }


    [Required(ErrorMessage = "The longitude is required")]
    public double? Longitude { get; init; }


    [Required(ErrorMessage = "The capacity is required")]
    public int? Capacity { get; init; }


    [Required(ErrorMessage = "The open time is required")]
    public TimeOnly? OpenTime { get; init; }


    [Required(ErrorMessage = "The close time is required")]
    public TimeOnly? CloseTime { get; init; }
}
