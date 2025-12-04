using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Repositories;
using Xunit;

namespace NRLObstacleReporting.UnitTests.Repository
{
    [Collection("SqliteInit")] // ensure Batteries.Init() ran
    public class AdminRepositoryTests : IClassFixture<SqliteInitFixture>
    {
        private readonly SqliteInitFixture _fixture;

        public AdminRepositoryTests(SqliteInitFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsExpectedUsers_WithInMemorySqlite()
        {
            // Arrange: set up in-memory SQLite and schema to mimic view_UserRole
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            // Manually create the schema needed by AdminRepository.GetAllUsersAsync
            var createTableSql = @"
                CREATE TABLE view_UserRole (
                    UserId       TEXT    NOT NULL,
                    RoleId       TEXT    NOT NULL,
                    UserName     TEXT    NOT NULL,
                    Email        TEXT    NULL,
                    PhoneNumber  TEXT    NULL
                );";
            await connection.ExecuteAsync(createTableSql);

            var expectedUsers = new List<ViewUserRoleDto>
            {
                new ViewUserRoleDto { UserId = "1", RoleId = "Pilot",     UserName = "User1", Email = "user1@example.com", PhoneNumber = "12345678" },
                new ViewUserRoleDto { UserId = "2", RoleId = "Registrar", UserName = "User2", Email = "user2@example.com", PhoneNumber = "87654321" }
            };

            var insertSql = @"
                INSERT INTO view_UserRole (UserId, RoleId, UserName, Email, PhoneNumber)
                VALUES (@UserId, @RoleId, @UserName, @Email, @PhoneNumber);";
            foreach (var user in expectedUsers)
            {
                await connection.ExecuteAsync(insertSql, user);
            }

            var repo = new AdminRepository(connection);

            // Act
            var result = (await repo.GetAllUsersAsync()).ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUsers.Count, result.Count);
            for (int i = 0; i < expectedUsers.Count; i++)
            {
                Assert.Equal(expectedUsers[i].UserId, result[i].UserId);
                Assert.Equal(expectedUsers[i].RoleId, result[i].RoleId);
                Assert.Equal(expectedUsers[i].UserName, result[i].UserName);
                Assert.Equal(expectedUsers[i].Email, result[i].Email);
                Assert.Equal(expectedUsers[i].PhoneNumber, result[i].PhoneNumber);
            }
        }
    }
}
