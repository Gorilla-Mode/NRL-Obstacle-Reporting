using Microsoft.AspNetCore.Identity;

namespace NRLObstacleReporting.Database;

/// <summary>
/// Datatransfer object targeting view view_ObstacleUser
/// </summary>
public class ViewObstacleUserDto : IdentityUser
{
    public required string ObstacleId { get; init; }
    
    public required string UserId { get; init; }
    
    public int Type { get; set; }               // FK to ObstacleTypeDatamodel
    public int Status { get; set; }               // FK to Status
    public int Marking { get; set; }               //FK to Marking
    public int HeightMeter { get; init; }
    
    public string? GeometryGeoJson { get; init; }
    
    public string? Name { get; init; }
    
    public string? Description { get; init; }
    
    public int Illuminated { get; init; }
    
    public DateTime CreationTime { get; init; }
    
    public DateTime UpdatedTime { get; init; }
    
}