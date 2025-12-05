using Dapper;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using NRLObstacleReporting.Repositories;
using NRLObstacleReporting.Repositories.IdentityStore;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NRLObstacleReporting.UnitTests.Repository.IdentityStore
{
    [Collection("SqliteInit")] // ensure Batteries.Init() ran
    [TestSubject(typeof(NrlRoleStore))]
    public class NrlRoleStoreTests : HelperQueries, IClassFixture<SqliteInitFixture>
    {
        private readonly SqliteInitFixture _fixture;
        public NrlRoleStoreTests(SqliteInitFixture fixture)
        {
            _fixture = fixture;
        }

        private static NrlRoleStore CreateRoleStore(SqliteConnection connection)
        {
            // Ensure the connection is open before use
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            // Use the SQLite test table name
            return new NrlRoleStore(connection);
        }

        /// <summary>
        /// Verifies that CreateAsync successfully inserts a new role into an in-memory SQLite database.
        /// The test sets up the AspNetRoles table, creates an IdentityRole, stores it through the role store,
        /// retrieves it from the database, and confirms that the stored values match and the operation succeeds.
        /// </summary>
        [Fact]
        public async Task CreateRoleAsync_AddsRoleToDatabase_WithInMemorySqlite()
        {
            // Arrange
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var sql = @"
                        CREATE TABLE AspNetRoles
                        (
                            Id TEXT NOT NULL PRIMARY KEY,
                            Name TEXT,
                            NormalizedName TEXT,
                            ConcurrencyStamp TEXT
                        );";
            await connection.ExecuteAsync(sql);

            var roleStore = CreateRoleStore(connection);

            var role = new IdentityRole("admin");
            
            // Use a non-cancelable token for this test invocation
            var result = await roleStore.CreateAsync(role, CancellationToken.None);
            IdentityRole insertedRole = await FakeGetRole(role.Id, connection);
            
            // Assert
            Assert.Equal(role.Id, insertedRole.Id);
            Assert.Equal(role.Name, insertedRole.Name);
            Assert.Equal(role.NormalizedName, insertedRole.NormalizedName);
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
        }
    }
}
