using NRLObstacleReporting.Database;

namespace NRLObstacleReporting.Repositories;

public interface IAdminRepository
{
    /// <summary>
    /// Gets all users and their role from db
    /// </summary>
    /// <returns>IEnumerable containing all users and their assigned roles</returns>
    Task<IEnumerable<ViewUserRoleDto>> GetAllUsersAsync();
}