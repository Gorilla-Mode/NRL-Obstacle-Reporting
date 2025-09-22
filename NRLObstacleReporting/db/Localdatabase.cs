namespace NRLObstacleReporting.db;

public class Localdatabase
{
    private static List<Models.ObstacleModel> _obstacles = new List<Models.ObstacleModel>();

    public static void AddObstacle(Models.ObstacleModel obstacleModel)
    {
        _obstacles.Add(obstacleModel);
        Console.WriteLine($"Obstacle {obstacleModel.GetHashCode()} added to database");
    }

    public static List<Models.ObstacleModel> GetDatabase()
    {
        return _obstacles;
    }
}