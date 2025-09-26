using System.ComponentModel.DataAnnotations;
namespace NRLObstacleReporting.Models;

public class ObstacleStep1Model
{
    public int ObstacleId { get; set; }
    public bool IsDraft { get; set; } = true;
    [Required(ErrorMessage = "This field is required")]
    [MaxLength(200)]
    public string? ObstacleType { get; set; }

    [Required(ErrorMessage = "This field is required")]
    [Range(0, 150)]
    public int ObstacleHeightMeter { get; set; }
}