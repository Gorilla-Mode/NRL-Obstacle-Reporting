namespace NRLObstacleReporting.Database;

public class ObstacleDto
{
    public int ObstacleId { get; set; }
    
    public int Type { get; set; }               // FK til ObstacleTypeDatamodel
    public int HeightMeter { get; set; }
    
    public string? GeometryGeoJson { get; set; }
    
    public string? Name { get; set; }
    
    public string? Description { get; set; }
    
    public int Illuminated { get; set; }
    
}