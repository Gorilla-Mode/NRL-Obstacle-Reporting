using NRLObstacleReporting.Database;

namespace NRLObstacleReporting.Repositories;

public interface IRegistrarRepository
{
    /// <summary>
    /// Gets all obstacles in database where status isn't draft
    /// </summary>
    /// <returns> IEnumerable of all submitted obstacles</returns>
    Task<IEnumerable<ObstacleDto>> GetAllSubmittedObstacles(); 
}