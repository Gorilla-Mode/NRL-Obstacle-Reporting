using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.Repositories;

public interface IRegistrarRepository
{
    /// <summary>
    /// Gets all obstacles in database where status isn't draft
    /// </summary>
    /// <returns> IEnumerable of all submitted obstacles</returns>
    Task<IEnumerable<ObstacleDto>> GetAllSubmittedObstaclesAsync();

    /// <summary>
    /// Gets a single obstacle from the database given an id
    /// </summary>
    /// <param name="id">Id of the obstacle to retireve from db</param>
    /// <returns>obstacle dto of the obstacle row in db</returns>
    Task<ViewObstacleUserDto> GetSubmittedObstacleByIdAsync(string id);

    /// <summary>
    /// Updates the status of an obstacle to match provided data
    /// </summary>
    /// <param name="data"></param>
    Task UpdateObstacleStatusAsync(ObstacleDto data);

    /// <summary>
    /// Gets every obstacle from db where status matches input statues
    /// </summary>
    /// <param name="status"> array of statuses to return</param>
    /// <returns>A list obstacles where status matches</returns>
    Task<IList<ObstacleDto>> GetObstaclesByStatusAsync(ObstacleCompleteModel.ObstacleStatus[] status);
    
    /// <summary>
    /// Gets obstacles all obstacles that have all the inputted filters. Empty arrays can be submitted to omit a filter
    /// </summary>
    /// <param name="status">Statuses of the obstacles to select</param>
    /// <param name="type">Types of the obstacles to select</param>
    /// <param name="illuminations">Illumination states of the obstacles to select</param>
    /// <param name="markings">Marking states of the obstacles to select</param>
    /// <param name="dateStart">Starting date to filter by</param>
    /// <param name="dateEnd">Ending date to filter by</param>
    /// <returns>An IList of obstacles that match all the selected filters</returns>
    Task<IList<ObstacleDto>> GetObstaclesFilteredAsync(ObstacleCompleteModel.ObstacleStatus[] status,
        ObstacleCompleteModel.ObstacleTypes[] type, ObstacleCompleteModel.Illumination[] illuminations,
        ObstacleCompleteModel.ObstacleMarking[] markings, DateOnly dateStart, DateOnly dateEnd);
    
}