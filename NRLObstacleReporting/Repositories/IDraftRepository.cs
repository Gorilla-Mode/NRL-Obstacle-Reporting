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
    /// <param name="data">Data which will update the obstacle in the database. must contain valid id and user id</param>
    /// <returns></returns>
    Task EditDraft(ObstacleDto data);

    /// <summary>
    /// Submits an obstacle, by changing its status to pending (submitted) 
    /// </summary>
    /// <param name="data">The obstacle that should be submitted. must contain valid id and user id</param>
    /// <returns></returns>
    Task SubmitDraft(ObstacleDto data);
    
    /// <summary>
    /// Sql returns all drafts submitted by a user
    /// </summary>
    /// <param name="userId">ID of the user</param>
    /// <returns>IEnumerable containing all drafts a given user has</returns>
    Task<IEnumerable<ObstacleDto>> GetAllDrafts(string userId);

    /// <summary>
    /// Gets data of a draft from db
    /// </summary>
    /// <param name="obstacleId">ID of the obstacle to retrieve</param>
    /// <param name="userId">ID of the user </param>
    /// <returns></returns>
    Task<ObstacleDto> GetDraftById(string obstacleId, string userId);
}