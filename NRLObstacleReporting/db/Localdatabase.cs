using System.Runtime.InteropServices;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.db;
public static class Localdatabase
{
    private static List<ObstacleCompleteModel> _obstacles = new List<Models.ObstacleCompleteModel>();

    public static void AddObstacle(ObstacleCompleteModel obstacleCompleteModel)
    {
        _obstacles.Add(obstacleCompleteModel);
        Console.WriteLine($"Obstacle {obstacleCompleteModel.GetHashCode()} added to database");
    }

    public static void EditObstacleCoordinates(int obstacleId, string? geometryGeoJson)
    {
        for (var i = 0; i < _obstacles.Count; i++)
        {
            if (_obstacles[i].ObstacleId == obstacleId)
            {
                _obstacles[i].GeometryGeoJson = geometryGeoJson;
            }
        }
    }

    public static void UpdateObstacle(ObstacleCompleteModel editedObstacle)
    {
        for (int i = 0; i < _obstacles.Count(); i++)
        {
            if (_obstacles[i].ObstacleId == editedObstacle.ObstacleId)
            {
                _obstacles[i] = editedObstacle;
            }
        }
    }
    
    public static ObstacleCompleteModel GetObstacleCompleteModel(int obstacleId)
    {
        foreach (var report in _obstacles)
        {
            if (report.ObstacleId == obstacleId)
            {
                return report;
            }
        }
        throw new Exception("Obstacle not found");
    }

    public static void RemoveObstacleAtIndex(int index)
    {
        _obstacles.RemoveAt(index);
    }

    public static List<ObstacleCompleteModel> GetDatabase()
    {
        return _obstacles;
    }
}