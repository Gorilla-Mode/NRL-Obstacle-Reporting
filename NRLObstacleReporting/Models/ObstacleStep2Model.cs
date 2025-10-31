using System.ComponentModel.DataAnnotations;

namespace NRLObstacleReporting.Models;

public class ObstacleStep2Model
{
    public int ObstacleId { get; set; }
    public ObstacleCompleteModel.ObstacleStatus Status { get; set; } = 0;

    public bool SaveDraft { get; set; }
    [Required]
    public string? GeometryGeoJson { get; set; }
}