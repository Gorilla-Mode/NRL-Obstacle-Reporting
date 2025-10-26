namespace NRLObstacleReporting.Models;
using System.ComponentModel.DataAnnotations;
public class ObstacleStep3Model
{
    public int ObstacleId { get; set; }
    public bool IsDraft { get; set; } = true;
    public int Illuminated { get; set; }
    public string? Name { get; set; }
    [MaxLength(100)]
    public string? Description { get; set; }
}