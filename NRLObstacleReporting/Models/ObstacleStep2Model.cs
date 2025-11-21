using System.ComponentModel.DataAnnotations;

namespace NRLObstacleReporting.Models;

public class ObstacleStep2Model
{
    public string? ObstacleId { get; set; }
    public string? UserId { get; set; }
    public ObstacleCompleteModel.ObstacleStatus Status { get; set; } = 0;

    public bool SaveDraft { get; set; }
    [Required]
    [MaxLength(5000)]
    public string? GeometryGeoJson { get; set; }
    
    public DateTime UpdatedTime { get; set; } = DateTime.Now;
}