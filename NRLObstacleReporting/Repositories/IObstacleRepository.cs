using System.Data;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.Repositories;

public interface IObstacleRepository
{
    /// <summary>
    /// Writes fields part of step 1 to database
    /// </summary>
    Task InsertStep1(ObstacleDto data);

    /// <summary>
    /// Updates an entry with data from step 2
    /// </summary>
    Task InsertStep2(ObstacleDto data);

    /// <summary>
    /// Updates an entry with data from step 3
    /// </summary>
    Task InsertStep3(ObstacleDto data);

    /// <summary>
    /// Gets an obstacle by id from the database
    /// </summary>
    Task<ObstacleDto> GetObstacleById(int id);

    /// <summary>
    /// Gets all submitted obstacles (excluding drafts)
    /// </summary>
    Task<IEnumerable<ObstacleDto>> GetAllSubmittedObstacles();

    /// <summary>
    /// Gets all obstacles filtered by status
    /// </summary>
    Task<IEnumerable<ObstacleDto>> GetReportsByStatusAsync(ObstacleCompleteModel.ObstacleStatus status);

    /// <summary>
    /// Updates only the status of an obstacle
    /// </summary>
    Task UpdateObstacleStatus(int obstacleId, ObstacleCompleteModel.ObstacleStatus status);
}
