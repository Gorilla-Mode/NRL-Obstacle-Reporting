using Dapper;
using Microsoft.AspNetCore.Identity;
using MySqlConnector;
using System.Data;

namespace NRLObstacleReporting.Repositories.IdentityStore;

// Dapper-based implementation of IUserStore without EF Core
public class NrlUserStore : IUserStore<IdentityUser>
{
    private readonly string _connectionString;
    private const string UsersTable = "AspNetUsers"; // default Identity users table

    public NrlUserStore()
    {
        _connectionString = Environment.GetEnvironmentVariable("INTERNALCONNECTION")!;
    }

    private IDbConnection CreateConnection() => new MySqlConnection(_connectionString);

    public void Dispose()
    {
        // Nothing to dispose; connections are created per operation.
    }

    public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(user.Id);
    }

    public Task<string?> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(user.UserName);
    }

    public Task SetUserNameAsync(IdentityUser user, string? userName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        cancellationToken.ThrowIfCancellationRequested();
        user.UserName = userName;
        return Task.CompletedTask;
    }

    public Task<string?> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(user.NormalizedUserName);
    }

    public Task SetNormalizedUserNameAsync(IdentityUser user, string? normalizedName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        cancellationToken.ThrowIfCancellationRequested();
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    public async Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);

        user.ConcurrencyStamp ??= Guid.NewGuid().ToString();
        user.SecurityStamp ??= Guid.NewGuid().ToString();

        const string sql = @$"INSERT INTO {UsersTable}
            (Id, UserName, NormalizedUserName, PasswordHash, SecurityStamp, ConcurrencyStamp, Email, NormalizedEmail, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
            VALUES (@Id, @UserName, @NormalizedUserName, @PasswordHash, @SecurityStamp, @ConcurrencyStamp, @Email, @NormalizedEmail, @EmailConfirmed, @PhoneNumber, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEnabled, @AccessFailedCount)";

        using var conn = CreateConnection();
        var command = new CommandDefinition(sql, user, cancellationToken: cancellationToken);
        await conn.ExecuteAsync(command);

        return IdentityResult.Success;
    }

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
        var command = new CommandDefinition(sql, user, cancellationToken: cancellationToken);
        await conn.ExecuteAsync(command);

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(user);

        const string sql = @$"DELETE FROM {UsersTable} WHERE Id = @Id";

        using var conn = CreateConnection();
        var command = new CommandDefinition(sql, new { user.Id }, cancellationToken: cancellationToken);
        await conn.ExecuteAsync(command);

        return IdentityResult.Success;
    }

    public async Task<IdentityUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        const string sql = @$"SELECT Id, UserName, NormalizedUserName, PasswordHash, SecurityStamp, ConcurrencyStamp, Email, NormalizedEmail, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount
                              FROM {UsersTable}
                              WHERE Id = @Id LIMIT 1";

        using var conn = CreateConnection();
        var command = new CommandDefinition(sql, new { Id = userId }, cancellationToken: cancellationToken);
        return await conn.QueryFirstOrDefaultAsync<IdentityUser>(command);
    }

    public async Task<IdentityUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        const string sql = @$"SELECT Id, UserName, NormalizedUserName, PasswordHash, SecurityStamp, ConcurrencyStamp, Email, NormalizedEmail, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount
                              FROM {UsersTable}
                              WHERE NormalizedUserName = @NormalizedUserName LIMIT 1";

        using var conn = CreateConnection();
        var command = new CommandDefinition(sql, new { NormalizedUserName = normalizedUserName }, cancellationToken: cancellationToken);
        return await conn.QueryFirstOrDefaultAsync<IdentityUser>(command);
    }
}