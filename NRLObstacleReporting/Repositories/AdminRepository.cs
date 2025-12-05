using System.Data;
using Dapper;
using NRLObstacleReporting.Database;

namespace NRLObstacleReporting.Repositories;

public sealed class AdminRepository : RepositoryBase, IAdminRepository
{
    private readonly IDbConnection _connection;

    public AdminRepository()
    {
        _connection = CreateConnection();
    }

    public AdminRepository(IDbConnection mockconnection)
    {
        _connection = mockconnection;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ViewUserRoleDto>> GetAllUsersAsync()
    {
        
        var sql = $@"SELECT UserId, RoleId, UserName, Email, PhoneNumber 
                    FROM view_UserRole";

        return await _connection.QueryAsync<ViewUserRoleDto>(sql);
    }
}