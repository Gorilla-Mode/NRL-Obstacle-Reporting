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
    /// <param name="id">ID of the obstacle in the database to submit</param>
    /// <returns></returns>
    Task SubmitDraft(int id);

    //TODO: once users are implemented, this should get a users draft. Not all
    /// <summary>
    /// Gets all drafts from database
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<ObstacleDto>> GetAllDrafts();
}