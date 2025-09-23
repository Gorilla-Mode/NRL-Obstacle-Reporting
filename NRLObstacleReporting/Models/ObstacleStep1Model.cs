using System.ComponentModel.DataAnnotations;
namespace NRLObstacleReporting.Models;

public class ObstacleStep1Model
{
    [Required(ErrorMessage = "This field is required")]
    [MaxLength(200)]
    public string? ObstacleType { get; set; }

    [Required(ErrorMessage = "This field is required")]
    [Range(0, 150)]
    public int ObstacleHeightMeter { get; set; }
}