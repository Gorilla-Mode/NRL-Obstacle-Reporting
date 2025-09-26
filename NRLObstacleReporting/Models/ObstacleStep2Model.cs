namespace NRLObstacleReporting.Models;
using System.ComponentModel.DataAnnotations;

public class ObstacleStep2Model
{

    public int ObstacleId { get; set; }
    public bool IsDraft { get; set; } = true;
    public string? GeometryGeoJson { get; set; }
}