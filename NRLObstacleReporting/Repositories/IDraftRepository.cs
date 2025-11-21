using NRLObstacleReporting.Database;

namespace NRLObstacleReporting.Repositories;

/// <summary>
/// Responsibility to edit a pilots drafts and submit them. 
/// </summary>
public interface IDraftRepository
{ 
    /// <summary>
    /// Edits the fields of an obstacle in the database with draft status
    /// </summary>
    /// <param name="id">ID of the obstacle to edit in the database</param>
    /// <param name="data">Data which will update the obstacle in the database</param>
    /// <returns></returns>
    Task EditDraft(ObstacleDto data);

    /// <summary>
    /// Submits an obstacle, by changing its status to pending (submitted) 
    /// </summary>
    /// <param name="data">The obstacle that should be submitted</param>
    /// <returns></returns>
    Task SubmitDraft(ObstacleDto data);
    
    /// <summary>
    /// Gets all drafts from database
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<ObstacleDto>> GetAllDrafts(string userId);
}