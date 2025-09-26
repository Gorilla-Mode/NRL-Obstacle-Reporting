using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.db;
public static class Localdatabase
{
    private static List<Models.ObstacleCompleteModel> _obstacles = new List<Models.ObstacleCompleteModel>();

    public static void AddObstacle(Models.ObstacleCompleteModel obstacleCompleteModel)
    {
        _obstacles.Add(obstacleCompleteModel);
        Console.WriteLine($"Obstacle {obstacleCompleteModel.GetHashCode()} added to database");
    }

    public static void EditObstacleCoordinates(int obstacleId, string? coordinates)
    {
        foreach (var report in _obstacles)
        {
            if (report.ObstacleId == obstacleId)
            {
                report.ObstacleCoordinates = coordinates;
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
        return null;
    }

    public static void RemoveObstacleAtIndex(int index)
    {
        _obstacles.RemoveAt(index);
    }

    public static List<Models.ObstacleCompleteModel> GetDatabase()
    {
        return _obstacles;
    }
}