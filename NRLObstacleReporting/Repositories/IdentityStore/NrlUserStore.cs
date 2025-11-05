using Dapper;
using Microsoft.AspNetCore.Identity;
using MySqlConnector;
using System.Data;

namespace NRLObstacleReporting.Repositories.IdentityStore;

/// <summary>
/// Dapper implementation of IUserStore, IUserPasswordStore, IUserEmailStore and IUserRoleStore.
/// Identity core depends on this implementation.
/// </summary>
public class NrlUserStore : IUserPasswordStore<IdentityUser>, IUserEmailStore<IdentityUser>, IUserRoleStore<IdentityUser>
{
    private readonly string _connectionString;
    private const string UsersTable = "AspNetUsers";
    private const string RolesTable = "AspNetRoles";
    private const string UserRolesTable = "AspNetUserRoles";
    
    /// <summary>
    /// Constructor, retrieves connection string from .env file
    /// </summary>
    public NrlUserStore()
    {
        _connectionString = Environment.GetEnvironmentVariable("INTERNALCONNECTION")!;
    }

    /// <summary>
    /// Establishes a new connection to the database
    /// </summary>
    /// <returns>new SQL connection to mariadb container</returns>
    private IDbConnection CreateConnection() => new MySqlConnection(_connectionString);

    /// <inheritdoc/>
    public void Dispose()
    {
        // nothing to dispose (connections are per operation)
    }

    // -------------------- IUserStore Implementation ---------------------------------------
    
    /// <inheritdoc/>
    public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        cancellationToken.ThrowIfCancellationRequested();
        
        return Task.FromResult(user.Id);
    }

    /// <inheritdoc/>
    public Task<string?> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        cancellationToken.ThrowIfCancellationRequested();
        
        return Task.FromResult(user.UserName);
    }

    /// <inheritdoc/>
    public Task SetUserNameAsync(IdentityUser user, string? userName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        cancellationToken.ThrowIfCancellationRequested();
        
        user.UserName = userName;
        
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task<string?> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        cancellationToken.ThrowIfCancellationRequested();
        
        return Task.FromResult(user.NormalizedUserName);
    }

    /// <inheritdoc/>
    public Task SetNormalizedUserNameAsync(IdentityUser user, string? normalizedName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        cancellationToken.ThrowIfCancellationRequested();
        
        user.NormalizedUserName = normalizedName;
        
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);

        user.ConcurrencyStamp ??= Guid.NewGuid().ToString();
        user.SecurityStamp ??= Guid.NewGuid().ToString();

        const string sql = @$"INSERT INTO {UsersTable}
                                    (Id, UserName, NormalizedUserName, PasswordHash, SecurityStamp, ConcurrencyStamp,
                                     Email, NormalizedEmail, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed,
                                     TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
                              VALUES (@Id, @UserName, @NormalizedUserName, @PasswordHash, @SecurityStamp, 
                                      @ConcurrencyStamp, @Email, @NormalizedEmail, @EmailConfirmed, @PhoneNumber, 
                                      @PhoneNumberConfirmed, @TwoFactorEnabled,@LockoutEnabled, @AccessFailedCount)";

        using var conn = CreateConnection();
        await conn.ExecuteAsync(new CommandDefinition(sql, user, cancellationToken: cancellationToken));

        return IdentityResult.Success;
    }

    /// <inheritdoc/>
    public async Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);

        user.ConcurrencyStamp ??= Guid.NewGuid().ToString();

        const string sql = @$"UPDATE {UsersTable}
            SET UserName = @UserName,
                NormalizedUserName = @NormalizedUserName,
                PasswordHash = @PasswordHash,
                SecurityStamp = @SecurityStamp,
                ConcurrencyStamp = @ConcurrencyStamp,
                Email = @Email,
                NormalizedEmail = @NormalizedEmail,
                EmailConfirmed = @EmailConfirmed,
                PhoneNumber = @PhoneNumber,
                PhoneNumberConfirmed = @PhoneNumberConfirmed,
                TwoFactorEnabled = @TwoFactorEnabled,
                LockoutEnabled = @LockoutEnabled,
                AccessFailedCount = @AccessFailedCount
            WHERE Id = @Id";

        using var conn = CreateConnection();
        await conn.ExecuteAsync(new CommandDefinition(sql, user, cancellationToken: cancellationToken));

        return IdentityResult.Success;
    }

    /// <inheritdoc/>
    public async Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);

        const string sql = @$"DELETE FROM {UsersTable}
                              WHERE Id = @Id";

        using var conn = CreateConnection();
        await conn.ExecuteAsync(new CommandDefinition(sql, new { user.Id }, cancellationToken: cancellationToken));

        return IdentityResult.Success;
    }

    /// <inheritdoc/>
    public async Task<IdentityUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        const string sql = @$"SELECT Id, UserName, NormalizedUserName, PasswordHash, SecurityStamp, ConcurrencyStamp,
                                     Email, NormalizedEmail, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed,
                                     TwoFactorEnabled, LockoutEnabled, AccessFailedCount
                              FROM {UsersTable}
                              WHERE Id = @Id LIMIT 1";

        using var conn = CreateConnection();
        
        return await conn.QueryFirstOrDefaultAsync<IdentityUser>
            (
                new CommandDefinition(sql, new { Id = userId }, cancellationToken: cancellationToken)
            );
    }

    /// <inheritdoc/>
    public async Task<IdentityUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        const string sql = @$"SELECT Id, UserName, NormalizedUserName, PasswordHash, SecurityStamp, ConcurrencyStamp,
                                     Email, NormalizedEmail, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed,
                                     TwoFactorEnabled, LockoutEnabled, AccessFailedCount
                              FROM {UsersTable}
                              WHERE NormalizedUserName = @NormalizedUserName LIMIT 1";

        using var conn = CreateConnection();
        
        return await conn.QueryFirstOrDefaultAsync<IdentityUser>
            (
                new CommandDefinition(sql, new { NormalizedUserName = normalizedUserName }, cancellationToken: cancellationToken)
            );
    }

    // --------------------- IUserPasswordStore Implementation ---------------------------------
    
    /// <inheritdoc/>
    public Task SetPasswordHashAsync(IdentityUser user, string? passwordHash, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        cancellationToken.ThrowIfCancellationRequested();
        
        user.PasswordHash = passwordHash;
        
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task<string?> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        cancellationToken.ThrowIfCancellationRequested();
        
        return Task.FromResult(user.PasswordHash);
    }

    /// <inheritdoc/>
    public Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        cancellationToken.ThrowIfCancellationRequested();
        
        return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
    }

    // ---------------- IUserEmailStore implementation ----------------------------------
    
    /// <inheritdoc/>
    public Task SetEmailAsync(IdentityUser user, string? email, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        cancellationToken.ThrowIfCancellationRequested();
        user.Email = email;
        
        return Task.CompletedTask;
    }
    
    /// <inheritdoc/>
    public Task<string?> GetEmailAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        cancellationToken.ThrowIfCancellationRequested();
        
        return Task.FromResult(user.Email);
    }

    /// <inheritdoc/>
    public Task<bool> GetEmailConfirmedAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        cancellationToken.ThrowIfCancellationRequested();
        
        return Task.FromResult(user.EmailConfirmed);
    }

    /// <inheritdoc/>
    public Task SetEmailConfirmedAsync(IdentityUser user, bool confirmed, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        cancellationToken.ThrowIfCancellationRequested();
        
        user.EmailConfirmed = confirmed;
        
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task<IdentityUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        const string sql = @$"SELECT Id, UserName, NormalizedUserName, PasswordHash, SecurityStamp, ConcurrencyStamp,
                                     Email, NormalizedEmail, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed,
                                     TwoFactorEnabled, LockoutEnabled, AccessFailedCount
                              FROM {UsersTable}
                              WHERE NormalizedEmail = @NormalizedEmail LIMIT 1";

        using var conn = CreateConnection();
        
        return await conn.QueryFirstOrDefaultAsync<IdentityUser>
            (
                new CommandDefinition(sql, new { NormalizedEmail = normalizedEmail }, cancellationToken: cancellationToken)
            );
    }

    /// <inheritdoc/>
    public Task<string?> GetNormalizedEmailAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        cancellationToken.ThrowIfCancellationRequested();
        
        return Task.FromResult(user.NormalizedEmail);
    }

    /// <inheritdoc/>
    public Task SetNormalizedEmailAsync(IdentityUser user, string? normalizedEmail, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        cancellationToken.ThrowIfCancellationRequested();
        
        user.NormalizedEmail = normalizedEmail;
        
        return Task.CompletedTask;
    }

    // ---------------- IUserRoleStore implementation ----------------------------------

    /// <inheritdoc/>
    public async Task AddToRoleAsync(IdentityUser user, string roleName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(roleName);
        cancellationToken.ThrowIfCancellationRequested();

        string normalized = roleName.ToUpperInvariant();

        const string getRoleSql = @$"SELECT Id FROM {RolesTable} WHERE NormalizedName = @NormalizedName LIMIT 1";
        using var conn = CreateConnection();
        var roleId = await conn.QueryFirstOrDefaultAsync<string>(new CommandDefinition(getRoleSql, new { NormalizedName = normalized }, cancellationToken: cancellationToken));
        if (string.IsNullOrEmpty(roleId))
        {
            // role doesn't exist; silently return (alternatively create role or throw)
            return;
        }

        const string insertSql = @$"INSERT IGNORE INTO {UserRolesTable} (UserId, RoleId) VALUES (@UserId, @RoleId)";
        await conn.ExecuteAsync(new CommandDefinition(insertSql, new { UserId = user.Id, RoleId = roleId }, cancellationToken: cancellationToken));
    }

    /// <inheritdoc/>
    public async Task RemoveFromRoleAsync(IdentityUser user, string roleName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(roleName);
        cancellationToken.ThrowIfCancellationRequested();

        string normalized = roleName.ToUpperInvariant();

        const string getRoleSql = @$"SELECT Id FROM {RolesTable} WHERE NormalizedName = @NormalizedName LIMIT 1";
        using var conn = CreateConnection();
        var roleId = await conn.QueryFirstOrDefaultAsync<string>(new CommandDefinition(getRoleSql, new { NormalizedName = normalized }, cancellationToken: cancellationToken));
        if (string.IsNullOrEmpty(roleId))
        {
            return;
        }

        const string deleteSql = @$"DELETE FROM {UserRolesTable} WHERE UserId = @UserId AND RoleId = @RoleId";
        await conn.ExecuteAsync(new CommandDefinition(deleteSql, new { UserId = user.Id, RoleId = roleId }, cancellationToken: cancellationToken));
    }

    /// <inheritdoc/>
    public async Task<IList<string>> GetRolesAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        cancellationToken.ThrowIfCancellationRequested();

        const string sql = @$"SELECT r.Name
                              FROM {RolesTable} r
                              INNER JOIN {UserRolesTable} ur ON ur.RoleId = r.Id
                              WHERE ur.UserId = @UserId";

        using var conn = CreateConnection();
        var roles = await conn.QueryAsync<string>(new CommandDefinition(sql, new { UserId = user.Id }, cancellationToken: cancellationToken));
        return roles.AsList();
    }

    /// <inheritdoc/>
    public async Task<bool> IsInRoleAsync(IdentityUser user, string roleName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(roleName);
        cancellationToken.ThrowIfCancellationRequested();

        string normalized = roleName.ToUpperInvariant();

        const string sql = @$"SELECT COUNT(1)
                              FROM {RolesTable} r
                              INNER JOIN {UserRolesTable} ur ON ur.RoleId = r.Id
                              WHERE ur.UserId = @UserId AND r.NormalizedName = @NormalizedName";

        using var conn = CreateConnection();
        var count = await conn.ExecuteScalarAsync<int>(new CommandDefinition(sql, new { UserId = user.Id, NormalizedName = normalized }, cancellationToken: cancellationToken));
        return count > 0;
    }

    /// <inheritdoc/>
    public async Task<IList<IdentityUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(roleName);
        cancellationToken.ThrowIfCancellationRequested();

        string normalized = roleName.ToUpperInvariant();

        const string sql = @$"SELECT u.Id, u.UserName, u.NormalizedUserName, u.PasswordHash, u.SecurityStamp, u.ConcurrencyStamp,
                                     u.Email, u.NormalizedEmail, u.EmailConfirmed, u.PhoneNumber, u.PhoneNumberConfirmed,
                                     u.TwoFactorEnabled, u.LockoutEnabled, u.AccessFailedCount
                              FROM {UsersTable} u
                              INNER JOIN {UserRolesTable} ur ON ur.UserId = u.Id
                              INNER JOIN {RolesTable} r ON ur.RoleId = r.Id
                              WHERE r.NormalizedName = @NormalizedName";

        using var conn = CreateConnection();
        var users = await conn.QueryAsync<IdentityUser>(new CommandDefinition(sql, new { NormalizedName = normalized }, cancellationToken: cancellationToken));
        return users.AsList();
    }
}