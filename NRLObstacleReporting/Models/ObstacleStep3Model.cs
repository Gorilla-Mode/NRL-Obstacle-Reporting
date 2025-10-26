namespace NRLObstacleReporting.Models;
using System.ComponentModel.DataAnnotations;
public class ObstacleStep3Model
{
    public int ObstacleId { get; set; }
    public bool IsDraft { get; set; } = true;
    public bool? ObstacleIlluminated { get; set; }
    public string? ObstacleName { get; set; }
    [MaxLength(1000)]
    public string? ObstacleDescription { get; set; }
}