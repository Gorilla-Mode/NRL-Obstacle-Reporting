using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.Repositories;

public interface IRegistrarRepository
{
    /// <summary>
    /// Gets all obstacles in database where status isn't draft
    /// </summary>
    /// <returns> IEnumerable of all submitted obstacles</returns>
    Task<IEnumerable<ObstacleDto>> GetAllSubmittedObstacles();

    /// <summary>
    /// Gets a single obstacle from the database given an id
    /// </summary>
    /// <param name="id">Id of the obstacle to retireve from db</param>
    /// <returns>obstacle dto of the obstacle row in db</returns>
    Task<ViewObstacleUserDto> GetSubmittedObstacleById(string id);

    /// <summary>
    /// Updates the status of an obstacle to match provided data
    /// </summary>
    /// <param name="data"></param>
    Task UpdateObstacleStatus(ObstacleDto data);

    Task<IList<ObstacleDto>> GetObstaclesByStatus(params ObstacleCompleteModel.ObstacleStatus[] status);
}