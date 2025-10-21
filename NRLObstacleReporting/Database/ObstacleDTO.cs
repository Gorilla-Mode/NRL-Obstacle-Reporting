namespace NRLObstacleReporting.Database;

public class ObstacleDto
{
    public int ObstacleId { get; set; }
    
    public int ObstacleTypeId { get; set; }               // FK til ObstacleTypeDatamodel
    public ObstacleTypeDto? ObstacleType { get; set; } // navigasjon
    
    public int ObstacleHeightMeter { get; set; }
    
    public string? GeometryGeoJson { get; set; }
    
    public string? ObstacleName { get; set; }
    
    public string? ObstacleDescription { get; set; }
    
    public bool? ObstacleIlluminated { get; set; }
}