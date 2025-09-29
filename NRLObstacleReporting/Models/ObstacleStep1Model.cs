using System.ComponentModel.DataAnnotations;
namespace NRLObstacleReporting.Models;

public class ObstacleStep1Model
{
    public int ObstacleId { get; set; }
    public bool IsDraft { get; set; } = true;
    public bool SaveDraft { get; set; }
   
    [Range(0, 150)]
    public int ObstacleHeightMeter { get; set; }

    public ObstacleCompleteModel.ObstacleTypes ObstacleType { get; set; }
    
}