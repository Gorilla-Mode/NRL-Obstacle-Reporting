namespace NRLObstacleReporting.Models;
using System.ComponentModel.DataAnnotations;
public class ObstacleStep3Model
{
    public int ObstacleId { get; set; }
    public bool IsDraft { get; set; } = true;
    public int ObstacleIlluminated { get; set; }
    public string? ObstacleName { get; set; }
    [MaxLength(100)]
    public string? ObstacleDescription { get; set; }
}