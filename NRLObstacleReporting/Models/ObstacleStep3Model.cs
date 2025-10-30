namespace NRLObstacleReporting.Models;
using System.ComponentModel.DataAnnotations;
public class ObstacleStep3Model
{
    public int ObstacleId { get; set; }
     public ObstacleCompleteModel.ObstacleStatus Status { get; set; } 
     public ObstacleCompleteModel.Illumination Illuminated { get; set; }
    //public int Status {get; set;}
    //public int Illuminated { get; set; }
    public string? Name { get; set; }
    [MaxLength(100)]
    public string? Description { get; set; }
}