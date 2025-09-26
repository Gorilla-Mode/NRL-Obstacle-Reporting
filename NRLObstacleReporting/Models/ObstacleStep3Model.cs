namespace NRLObstacleReporting.Models;
using System.ComponentModel.DataAnnotations;
public class ObstacleStep3Model
{
    public int ObstacleId { get; set; }
    public bool? ObstacleIlluminated { get; set; }
    public string? ObstacleName { get; set; }
    [MaxLength(100)]
    public string? ObstacleDescription { get; set; }
}