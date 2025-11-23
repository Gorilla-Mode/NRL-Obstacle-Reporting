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

    /// <summary>
    /// Gets every obstacle from db where status matches input statues
    /// </summary>
    /// <param name="status"> array of statuses to return</param>
    /// <returns>A list obstacles where status matches</returns>
    Task<IList<ObstacleDto>> GetObstaclesByStatus(ObstacleCompleteModel.ObstacleStatus[] status);
    
    
    Task<IList<ObstacleDto>> GetObstaclesFiltered(ObstacleCompleteModel.ObstacleStatus[] status,
        ObstacleCompleteModel.ObstacleTypes[] type, ObstacleCompleteModel.Illumination[] illuminations,
        ObstacleCompleteModel.ObstacleMarking[] markings, DateOnly dateStart, DateOnly dateEnd);
    
}