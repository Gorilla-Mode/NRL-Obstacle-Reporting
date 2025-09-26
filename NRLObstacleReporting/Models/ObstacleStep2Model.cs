namespace NRLObstacleReporting.Models;
using System.ComponentModel.DataAnnotations;

public class ObstacleStep2Model
{
<<<<<<< HEAD
    public int ObstacleId { get; set; }
    //Må byttes ut med geoJSON
    [MaxLength(200)] 
    public string? ObstacleCoordinates { get; set; }
=======
    public string? GeometryGeoJson { get; set; }
>>>>>>> obstacle-reporting
}