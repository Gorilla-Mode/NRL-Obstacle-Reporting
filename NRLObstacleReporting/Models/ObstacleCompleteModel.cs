namespace NRLObstacleReporting.Models;
using System.ComponentModel.DataAnnotations;

public class ObstacleCompleteModel
{
    //Step 1 felt
    [Required(ErrorMessage = "This field is required")]
    [MaxLength(200)]
    public string? ObstacleType { get; set; }
    
    [Required(ErrorMessage = "This field is required")]
    [Range(0, 150)]
    public int ObstacleHeightMeter { get; set; }
    
    //Step 2 felt
    //Skal gjøres til geoJSON
    [MaxLength(200)] 
    public string? ObstacleCoordinates { get; set; }
    
    //Step 3 felt
    [MaxLength(100)]
    public string? ObstacleName { get; set; }
    
    [MaxLength(100)]
    public string? ObstacleDescription { get; set; }
    public bool? ObstacleIlluminated { get; set; }
}