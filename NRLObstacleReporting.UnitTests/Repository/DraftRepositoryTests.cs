using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using JetBrains.Annotations;
using Microsoft.Data.Sqlite;
using NRLObstacleReporting.Controllers;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Repositories;
using Xunit;

namespace NRLObstacleReporting.UnitTests.Repository
{
    [Collection("SqliteInit")] // ensure Batteries.Init() ran
    [TestSubject(typeof(DraftRepository))]
    public class DraftRepositoryTests : HelperQueries, IClassFixture<SqliteInitFixture>
    {
        public DraftRepositoryTests(SqliteInitFixture _)
        {
        }

        private static IDraftRepository CreateRepo(SqliteConnection connection)
        {
            // Ensure the connection is open before use
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return new DraftRepository(connection);
        }

        /// <summary>
        /// Verifies that the EditDraftAsync method correctly updates an existing draft entry in the database with the provided data.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation. Confirms the draft data is updated correctly in the database by validating it against expected values.
        /// </returns>
        [Fact]
        public async Task EditDraftAsync_ShouldUpdatesDraft()
        {
            // Arrange
            const string tableName = "Obstacle";
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();
            
            await CreateObstacleTable(connection, tableName);

            var draftRepository = CreateRepo(connection); 
            var updatedObstacle = new ObstacleDto
            {
                ObstacleId = "1",
                UserId = "1234", 
                HeightMeter = 20,
                GeometryGeoJson = "blablablablab",
                Name = "Test Obstacle",
                Description = "This is a test obstacle.",
                CreationTime = null,
                UpdatedTime = null
            };

            var initialData = new ObstacleDto
            {
                ObstacleId = "1",
                UserId = "1234",
                HeightMeter = 10,
                GeometryGeoJson = "blablablablab",
                Name = "Old Obstacle",
                Description = "Old description",
                CreationTime = null,
                UpdatedTime = null
            };
            
            await InsertObstacleHelper(initialData, connection, tableName); // Use helper to mock old data

            // Act
            await draftRepository.EditDraftAsync(updatedObstacle);
            var result = await FakeGetObstacleById(updatedObstacle.ObstacleId, tableName, connection);
            
            // Assert
            Assert.NotEqual(result, initialData);
            Assert.NotEqual(result.Description, initialData.Description);
            Assert.NotNull(result);
            Assert.Equal(updatedObstacle.ObstacleId, result.ObstacleId);
            Assert.Equal(updatedObstacle, result);
        }
    }
}