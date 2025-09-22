namespace NRLObstacleReporting.Models;
using System.ComponentModel.DataAnnotations;

public class ObstacleModel
{
    
    [MaxLength(100)]
    public string ObstacleName { get; set; }

   
    [MaxLength(100)]
    public string ObstacleDescription { get; set; }

    [Required(ErrorMessage = "This field is required")]
    [Range(0, 150)]
    public int ObstacleHeightMeter { get; set; }

    [MaxLength(200)] 
    public string? ObstacleCoordinates { get; set; }

    [Required(ErrorMessage = "This field is required")]
    [MaxLength(200)]
    public string? ObstacleType { get; set; }
    
    public bool? ObstacleIlluminated { get; set; }
}