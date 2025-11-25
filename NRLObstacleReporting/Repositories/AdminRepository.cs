using Dapper;
using NRLObstacleReporting.Database;

namespace NRLObstacleReporting.Repositories;

public class AdminRepository : RepositoryBase, IAdminRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<ViewUserRoleDto>> GetAllUsers()
    {
        using var connection = CreateConnection();
        
        var sql = $@"SELECT UserId, RoleId, UserName, Email, PhoneNumber 
                    FROM view_UserRole";

        return await connection.QueryAsync<ViewUserRoleDto>(sql);
    }
}