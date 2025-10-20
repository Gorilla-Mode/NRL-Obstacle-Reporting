namespace NRLObstacleReporting.Database;

public class ObstacleDTO
{
    public bool IsDraft { get; set; } 
    public int ObstacleId { get; set; }
    
    public int ObstacleTypeId { get; set; }               // FK til ObstacleTypeDatamodel
    public ObstacleTypeDTO? ObstacleType { get; set; } // navigasjon
    
    public int ObstacleHeightMeter { get; set; }
    
    public string? GeometryGeoJson { get; set; }
    
    public string? ObstacleName { get; set; }
    
    public string? ObstacleDescription { get; set; }
    
    public bool? ObstacleIlluminated { get; set; }
}