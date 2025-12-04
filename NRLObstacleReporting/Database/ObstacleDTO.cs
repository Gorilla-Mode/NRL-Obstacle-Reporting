namespace NRLObstacleReporting.Database;

/// <summary>
/// Data-transfer object targeting Obstacle table
/// </summary>
public record ObstacleDto
{
    public required string ObstacleId { get; init; }
    public required string UserId { get; init; }
    
    public int HeightMeter { get; init; }
    public string? GeometryGeoJson { get; init; }
    public int Type { get; init; }  
    
    public int Status { get; init; }               
    public int Marking { get; init; }               
    public string? Name { get; init; }
    public string? Description { get; init; }
    
    public int Illuminated { get; init; }
    public DateTime? CreationTime { get; init; }
    public DateTime? UpdatedTime { get; init; }
}