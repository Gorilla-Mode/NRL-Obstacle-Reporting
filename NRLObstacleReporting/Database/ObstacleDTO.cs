namespace NRLObstacleReporting.Database;

public class ObstacleDto
{
    public string ObstacleId { get; set; }
    
    public int Type { get; set; }               // FK to ObstacleTypeDatamodel
    public int Status { get; set; }               // FK to Status
    public int Marking { get; set; }               //FK to Marking
    public int HeightMeter { get; set; }
    
    public string? GeometryGeoJson { get; set; }
    
    public string? Name { get; set; }
    
    public string? Description { get; set; }
    
    public int Illuminated { get; set; }
    
    public DateTime CreationTime { get; set; }
    
    public DateTime UpdatedTime { get; set; }
}