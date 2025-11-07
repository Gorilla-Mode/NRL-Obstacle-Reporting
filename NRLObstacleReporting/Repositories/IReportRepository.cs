
using NRLObstacleReporting.Models;
using NRLObstacleReporting.Database;

namespace NRLObstacleReporting.Repositories;

public interface IRegistrarRepository
{
    /// <summary>
    /// Gets all submitted obstacles that are not Draft or Deleted.
    /// </summary>
    /// <returns>List of submitted obstacles</returns>
    Task<IEnumerable<ObstacleDto>> GetAllReportsAsync();
    
public interface IRegistrarRepository
{
    /// <summary>
    /// Gets all submitted obstacles that are not Draft or Deleted.
    /// </summary>
    /// <returns>List of submitted obstacles</returns>
    Task<IEnumerable<ObstacleDto>> GetAllReportsAsync();

    /// <summary>
    /// Gets all submitted obstacles filtered by status.
    /// </summary>
    /// <param name="status">Status to filter by (e.g. Pending, Approved, Rejected)</param>
    /// <returns>List of obstacles with given status</returns>
    Task<IEnumerable<ObstacleDto>> GetReportsByStatusAsync(ObstacleCompleteModel.ObstacleStatus status);
}


    /// <summary>
    /// Gets all submitted obstacles filtered by status.
    /// </summary>
    /// <param name="status">Status to filter by (e.g. Pending, Approved, Rejected)</param>
    /// <returns>List of obstacles with given status</returns>
    Task<IEnumerable<ObstacleDto>> GetReportsByStatusAsync(ObstacleCompleteModel.ObstacleStatus status);
}
