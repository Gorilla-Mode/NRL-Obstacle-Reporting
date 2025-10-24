using System.Data;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.Repositories;

public interface IObstacleRepository
{
    /// <summary>
    /// Writes fields part of step 1 to database
    /// </summary>
    /// <param name="data">extracted data is injected into sql</param>
    /// <returns></returns>
    /// 
    Task InsertStep1(ObstacleDto data);
    
    /// <summary>
    /// Updates an entry with data from step 2
    /// </summary>
    /// <param name="data">extracted data is injected into sql. Data must include an id</param>
    /// <returns></returns>
    Task InsertStep2(ObstacleDto data);
    
    /// <summary>
    /// Updates an entry with data from step 3
    /// </summary>
    /// <param name="data">extracted data is injected into sql. Data must include an id</param>
    /// <returns></returns>
    Task InsertStep3(ObstacleDto data);

    /// <summary>
    /// Gets an obstacle by id
    /// </summary>
    /// <param name="data">extracted data is injected into sql. Data must include an id</param>
    /// <returns></returns>
    Task<ObstacleCompleteModel> GetObstacle(ObstacleDto data);
    Task<IEnumerable<ObstacleDto>> GetAllObstacleData(); 
}