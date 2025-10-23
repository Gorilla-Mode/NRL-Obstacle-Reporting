namespace NRLObstacleReporting.Models;
using System.ComponentModel.DataAnnotations;

public class ObstacleCompleteModel
{
    public enum ObstacleTypes
    {
        PowerLine,
        Pole,
        Building,
        Construction,
        Natural,
        Other
    }
    public enum Illumination
    {
        Unknown,
        NotIlluminated,
        Illuminated
    }
    public bool IsDraft { get; set; } = true;
    public int ObstacleId { get; set; }
    
    //Step 1 felt
    public ObstacleTypes ObstacleType { get; set; }
    
    [Required(ErrorMessage = "This field is required")]
    [Range(0, 150)]
    public int ObstacleHeightMeter { get; set; }

    //Step 2 felt
    [Required(ErrorMessage = "This field is required")]
    public string? GeometryGeoJson { get; set; }
    //Step 3 felt
    [MaxLength(100)]
    public string? ObstacleName { get; set; }
    
    [MaxLength(100)]
    public string? ObstacleDescription { get; set; }
    
    public Illumination ObstacleIlluminated { get; set; }
    
}