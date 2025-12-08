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

        private async Task CreateAspNetRoles(SqliteConnection connection)
        {
            var sql = @"
                        CREATE TABLE AspNetRoles
                        (
                            Id TEXT NOT NULL PRIMARY KEY,
                            Name TEXT,
                            NormalizedName TEXT,
                            ConcurrencyStamp TEXT
                        );";
            
            await connection.ExecuteAsync(sql);
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

            await CreateAspNetRoles(connection);

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

        /// <summary>
        /// Validates that FindByIdAsync retrieves a role from an in-memory SQLite database.
        /// The test creates a role, stores it via the role store, and then attempts to
        /// find it by its ID. It asserts that the retrieved role matches the stored one,
        /// confirming the functionality of the FindByIdAsync method within the role store.
        /// </summary>
        [Fact]
        public async Task FindByIdAsync_FindsRoleFromDatabase_WithInMemorySqlite()
        {
            // Arrange
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            await CreateAspNetRoles(connection);

            var roleStore = CreateRoleStore(connection);

            var role = new IdentityRole("admin");
            await roleStore.CreateAsync(role, CancellationToken.None);
            
            // Act
            var result = await roleStore.FindByIdAsync(role.Id, CancellationToken.None);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(role.Id, result.Id);
            Assert.Equal(role.Name, result.Name);
            Assert.Equal(role.NormalizedName, result.NormalizedName);
            Assert.Equal(role.ConcurrencyStamp, result.ConcurrencyStamp);
        }

        /// <summary>
        /// Verifies that FindByNameAsync correctly retrieves a role from an in-memory SQLite database
        /// using the normalized role name. The test sets up the AspNetRoles table, inserts a role,
        /// and ensures that the retrieved role matches the stored details.
        /// </summary>
        [Fact]
        public async Task FindByNameAsync_FindsRoleFromDatabase_WithInMemorySqlite()
        {
            // Arrange
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            await CreateAspNetRoles(connection);

            var roleStore = CreateRoleStore(connection);

            var role = new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            await roleStore.CreateAsync(role, CancellationToken.None);
            
            // Act
            var result = await roleStore.FindByNameAsync(role.NormalizedName!, CancellationToken.None);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(role.Id, result.Id);
            Assert.Equal(role.Name, result.Name);
            Assert.Equal(role.NormalizedName, result.NormalizedName);
            Assert.Equal(role.ConcurrencyStamp, result.ConcurrencyStamp);
        }

        /// <summary>
        /// Verifies that GetNormalizedRoleNameAsync retrieves the normalized role name of a given role
        /// from the in-memory SQLite database. The test first sets up the AspNetRoles table, inserts
        /// a new role with a specific normalized name into the database, and confirms that the method
        /// correctly returns the expected normalized name matching the stored value.
        /// </summary>
        [Fact]
        public async Task GetNormalizedRoleNameAsync_FindsNormalizedRoleName_WithInMemorySqlite()
        {
            // Arrange
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            await CreateAspNetRoles(connection);

            var roleStore = CreateRoleStore(connection);

            var role = new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            await roleStore.CreateAsync(role, CancellationToken.None);
            
            // Act
            var result = await roleStore.GetNormalizedRoleNameAsync(role, CancellationToken.None);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(role.NormalizedName, result);
        }

        /// <summary>
        /// Verifies that GetRoleIdAsync retrieves the correct role ID from the in-memory SQLite database.
        /// The test sets up the AspNetRoles table, inserts a role using CreateAsync, retrieves the role ID via GetRoleIdAsync,
        /// and ensures the returned ID matches the inserted role's ID.
        /// </summary>
        [Fact]
        public async Task GetRoleIdAsync_FindsRoleId_WithInMemorySqlite()
        {
            // Arrange
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            await CreateAspNetRoles(connection);

            var roleStore = CreateRoleStore(connection);

            var role = new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            await roleStore.CreateAsync(role, CancellationToken.None);
            
            // Act
            var result = await roleStore.GetRoleIdAsync(role, CancellationToken.None);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(role.Id, result);
        }

        /// <summary>
        /// Verifies that GetRoleNameAsync retrieves the name of a role from an in-memory SQLite database.
        /// The test sets up the AspNetRoles table, inserts a role using the role store, and confirms that the
        /// retrieved role name matches the name initially inserted.
        /// </summary>
        [Fact]
        public async Task GetRoleNameAsync_FindsRoleName_WithInMemorySqlite()
        {
            // Arrange
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            await CreateAspNetRoles(connection);

            var roleStore = CreateRoleStore(connection);

            var role = new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            await roleStore.CreateAsync(role, CancellationToken.None);
            
            // Act
            var result = await roleStore.GetRoleNameAsync(role, CancellationToken.None);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(role.Name, result);
        }

        /// <summary>
        /// Verifies that SetRoleNameAsync successfully updates the role's Name property in an in-memory SQLite database.
        /// The test initializes the AspNetRoles table, creates an IdentityRole, updates its Name using SetRoleNameAsync,
        /// retrieves the role by its ID, and asserts that the Name property matches the updated value.
        /// </summary>
        [Fact]
        public async Task SetRoleNameAsync_UpdatesRoleName_WithInMemorySqlite()
        {
            // Arrange
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();
            
            const string expectedRoleName = "pilot";

            await CreateAspNetRoles(connection);

            var roleStore = CreateRoleStore(connection);

            var role = new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            
            await roleStore.CreateAsync(role, CancellationToken.None);
            
            // Act
            await roleStore.SetRoleNameAsync(role, expectedRoleName, CancellationToken.None);
            
            // Assert
            Assert.NotNull(role);
            Assert.Equal(expectedRoleName, role.Name );
        }

        /// <summary>
        /// Verifies that UpdateAsync successfully updates an existing role in an in-memory SQLite database.
        /// The test inserts a role into the database, modifies its properties, updates it using the role store,
        /// retrieves the updated role, and confirms that the changes were applied correctly.
        /// </summary>
        [Fact]
        public async Task UpdateAsync_UpdatesRole_WithInMemorySqlite()
        {
            // Arrange
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();
            
            const string expectedUpdatedRoleName = "pilot";

            await CreateAspNetRoles(connection);

            var roleStore = CreateRoleStore(connection);

            var role = new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            
            await roleStore.CreateAsync(role, CancellationToken.None);
            await roleStore.SetRoleNameAsync(role, expectedUpdatedRoleName, CancellationToken.None);
            
            // Act
            await roleStore.UpdateAsync(role, CancellationToken.None);
            var result = await roleStore.FindByIdAsync(role.Id, CancellationToken.None);
            
            // Assert
            Assert.NotNull(role);
            Assert.Equal(expectedUpdatedRoleName, result!.Name );
        }
        
        [Fact]
        public async Task DeleteAsync_DeletesRole_WithInMemorySqlite()
        {
            // Arrange
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            await CreateAspNetRoles(connection);

            var roleStore = CreateRoleStore(connection);

            var role = new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = "admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            
            await roleStore.CreateAsync(role, CancellationToken.None);
            
            // Act
            await roleStore.DeleteAsync(role, CancellationToken.None);
            var result = await roleStore.FindByIdAsync(role.Id, CancellationToken.None);
            
            // Assert
            Assert.Null(result);
        }
        
    }
}