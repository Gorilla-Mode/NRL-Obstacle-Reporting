using System.ComponentModel.DataAnnotations;
namespace NRLObstacleReporting.Models;

public class ObstacleStep1Model
{
    public string? ObstacleId { get; set; }
    public string? UserId { get; set; }
    public ObstacleCompleteModel.ObstacleStatus Status { get; set; } = 0;
    public bool SaveDraft { get; set; }
   
    [Range(0, 150)]
    public int HeightMeter { get; set; }
    
    [Required] 
    public ObstacleCompleteModel.ObstacleTypes? Type { get; set; }
    
    public string? GeometryGeoJson { get; set; }
    
    public DateTime CreationTime { get; set; } = DateTime.Now;
    
    public DateTime UpdatedTime { get; set; } = DateTime.Now;

}