namespace NRLObstacleReporting.db;
public static class Localdatabase
{
    private static List<Models.ObstacleCompleteModel> _obstacles = new List<Models.ObstacleCompleteModel>();

    public static void AddObstacle(Models.ObstacleCompleteModel obstacleCompleteModel)
    {
        _obstacles.Add(obstacleCompleteModel);
        Console.WriteLine($"Obstacle {obstacleCompleteModel.GetHashCode()} added to database");
    }

    public static List<Models.ObstacleCompleteModel> GetDatabase()
    {
        return _obstacles;
    }
}