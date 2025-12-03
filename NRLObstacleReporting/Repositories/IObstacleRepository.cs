using NRLObstacleReporting.Database;

namespace NRLObstacleReporting.Repositories;

public interface IObstacleRepository
{
    /// <summary>
    /// Writes fields part of step 1 to database
    /// </summary>
    /// <param name="data">extracted data is injected into sql</param>
    /// <returns></returns>
    /// 
    Task InsertStep1Async(ObstacleDto data);
    
    /// <summary>
    /// Updates an entry with data from step 2
    /// </summary>
    /// <param name="data">extracted data is injected into sql. Data must include an id</param>
    /// <returns></returns>
    Task InsertStep2Async(ObstacleDto data);
    
    /// <summary>
    /// Updates an entry with data from step 3
    /// </summary>
    /// <param name="data">extracted data is injected into sql. Data must include an id</param>
    /// <returns></returns>
    Task InsertStep3Async(ObstacleDto data);

    /// <summary>
    /// Gets an obstacle by id from the database, and maps it to the data transfer object
    /// </summary>
    /// <param name="id">ID of the obstacle to get from the database</param>
    /// <returns>Task:ObstacleDto </returns>
    Task<ObstacleDto> GetObstacleByIdAsync(string? id);
    Task<IEnumerable<ObstacleDto>> GetAllSubmittedObstaclesAsync(string? userId); 
}