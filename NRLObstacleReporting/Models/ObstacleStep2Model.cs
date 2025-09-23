namespace NRLObstacleReporting.Models;
using System.ComponentModel.DataAnnotations;

public class ObstacleStep2Model
{
    //Må byttes ut med geoJSON
    [MaxLength(200)] 
    public string? ObstacleCoordinates { get; set; }
}