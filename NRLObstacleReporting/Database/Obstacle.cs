namespace NRLObstacleReporting.Database;

public class Obstacle 
{
    public bool IsDraft { get; set; } 
    public int ObstacleId { get; set; }
    public int ObstacleType { get; set; }
    
    public int ObstacleHeightMeter { get; set; }
    
    public string? GeometryGeoJson { get; set; }
    
    public string? ObstacleName { get; set; }
    
    public string? ObstacleDescription { get; set; }
    
    public bool? ObstacleIlluminated { get; set; }
}
