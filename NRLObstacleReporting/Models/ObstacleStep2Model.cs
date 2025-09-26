namespace NRLObstacleReporting.Models;
using System.ComponentModel.DataAnnotations;

public class ObstacleStep2Model
{

    public int ObstacleId { get; set; }
    //Må byttes ut med geoJSON
    public string? GeometryGeoJson { get; set; }
}