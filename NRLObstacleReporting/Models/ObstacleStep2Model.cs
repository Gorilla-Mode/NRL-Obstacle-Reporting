using System.ComponentModel.DataAnnotations;

namespace NRLObstacleReporting.Models;

public class ObstacleStep2Model
{
    public int ObstacleId { get; set; }
    public bool IsDraft { get; set; } = true;
    public bool SaveDraft { get; set; }
    [Required]
    public string? GeometryGeoJson { get; set; }
}