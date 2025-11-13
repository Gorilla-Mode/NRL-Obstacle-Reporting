using System.Runtime.InteropServices;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.db;
public static class Localdatabase
{
    private static List<ObstacleCompleteModel> _obstacles = new List<Models.ObstacleCompleteModel>();
    
    /// <summary>
    /// Method Adds a new object to the database
    /// </summary>
    /// <param name="obstacleCompleteModel"></param>
    public static void AddObstacle(ObstacleCompleteModel obstacleCompleteModel)
    {
        _obstacles.Add(obstacleCompleteModel);
        Console.WriteLine($"Obstacle {obstacleCompleteModel.GetHashCode()} added to database");
    }
    
    /// <summary>
    ///  Method Edits the coordinates of an obstacle in the database
    /// </summary>
    /// <param name="obstacleId">ID of obstacle to edit</param>
    /// <param name="geometryGeoJson">New GeoJSON</param>
   /*  public static void EditObstacleCoordinates(int obstacleId, string? geometryGeoJson)
    {
        for (var i = 0; i < _obstacles.Count; i++)
        {
            if (_obstacles[i].ObstacleId == obstacleId)
            {
                _obstacles[i].GeometryGeoJson = geometryGeoJson;
            }
        }
    }
    */
   
    /// <summary>
    /// Method Replaces obstacle with the same id as the obstacle inputted in the database
    /// </summary>
    /// <param name="editedObstacle">Replaces object in database</param>
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
    
    /// <summary>
    /// Method Returs object of a completeobstacle given a valid ID
    /// </summary>
    /// <param name="obstacleId"> ID to get from database</param>
    /// <returns></returns>
    /// <exception cref="Exception"> Obstacle not found</exception>
   /* public static ObstacleCompleteModel GetObstacleCompleteModel(int obstacleId)
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
*/
    /// <summary>
    /// Method Delets object at specified index
    /// </summary>
    /// <param name="index">object to delete</param>
    public static void RemoveObstacleAtIndex(int index)
    {
        _obstacles.RemoveAt(index);
    }

    /// <summary>
    /// Method returns the entire database
    /// </summary>
    /// <returns></returns>
    public static List<ObstacleCompleteModel> GetDatabase()
    {
        return _obstacles;
    }
}