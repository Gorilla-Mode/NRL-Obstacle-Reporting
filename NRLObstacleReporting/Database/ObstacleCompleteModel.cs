namespace NRLObstacleReporting.Database;

public class ObstacleCompleteModel
{
    public bool IsDraft { get; set; } 
    public int ObstacleId { get; set; }
    
    public ObstacleTypes ObstacleType { get; set; }
    public enum ObstacleTypes
    {
        PowerLine,
        Pole,
        Building,
        Construction,
        Natural,
        Other
    }
    
    public int ObstacleHeightMeter { get; set; }
    
    public string? GeometryGeoJson { get; set; }
    
    public string? ObstacleName { get; set; }
    
    public string? ObstacleDescription { get; set; }
    
    public bool? ObstacleIlluminated { get; set; }
}
