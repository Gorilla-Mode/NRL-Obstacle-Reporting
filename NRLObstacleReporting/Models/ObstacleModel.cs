namespace NRLObstacleReporting.Models;
using System.ComponentModel.DataAnnotations;

public class ObstacleModel
{
    [Required(ErrorMessage = "This field is required")]
    [Range(0,150)]
    public int ObstacleHeightMeter { get; set; }

    [MaxLength(200)] [Required(ErrorMessage = "This field is required")]
    public string? ObstacleCoordinates { get; set; }

    [Required(ErrorMessage = "This field is required")]
    [MaxLength(200)]
    public string? ObstacleType { get; set; }
    
    public string? ObstacleIlluminated { get; set; }
}