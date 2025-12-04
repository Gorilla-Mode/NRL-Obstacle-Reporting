using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Repositories;
using Xunit;

namespace NRLObstacleReporting.UnitTests.Repository
{
    [Collection("SqliteInit")]
    public class ObstacleRepositoryTests : HelperQueries, IClassFixture<SqliteInitFixture>
    {
        // Only the SQLite init fixture is injected.
        public ObstacleRepositoryTests(SqliteInitFixture _) { }

        private static IObstacleRepository CreateRepo(SqliteConnection connection)
        {
            // Ensure the connection is open before use
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return new ObstacleRepository(connection);
        }

        [Fact]
        public async Task InsertStep1Async_InsertsExpectedObstacle_WithInMemorySqlite()
        {
            // Arrange
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var createTableSql = @"
            CREATE TABLE Obstacle
            (
                ObstacleID     TEXT    NOT NULL PRIMARY KEY,
                UserId         TEXT    NOT NULL,
                HeightMeter    INTEGER NOT NULL,
                GeometryGeoJson TEXT   NULL,
                Type           INTEGER NOT NULL,
                Status         INTEGER NOT NULL DEFAULT 0,
                Marking        INTEGER NOT NULL DEFAULT 0,
                Name           TEXT    NULL,
                Description    TEXT    NULL,
                Illuminated    INTEGER NOT NULL DEFAULT 0,
                CreationTime   TEXT    NULL,
                UpdatedTime    TEXT    NULL
            );";
            await connection.ExecuteAsync(createTableSql);

            var repo = CreateRepo(connection);

            var obstacle = new ObstacleDto
            {
                ObstacleId = "1",
                UserId = "1",
                HeightMeter = 11,
                GeometryGeoJson = "POINT(10 20)",
                Type = 1,
                Status = 0,
                Marking = 1,
                Name = "Test Obstacle A",
                Description = "First test obstacle",
                Illuminated = 0,
                // Use valid DateTime values (UTC)
                CreationTime = null,
                UpdatedTime = null
            };

            // Act
            await repo.InsertStep1Async(obstacle);

            var fetched = await HelperQueries.FakeGetObstacleById("1", "Obstacle", connection);

            // Assert
            Assert.NotNull(fetched);
            Assert.Equal(obstacle.HeightMeter, fetched.HeightMeter);
            Assert.Equal(obstacle.ObstacleId, fetched.ObstacleId);
        }

        [Fact]
        public async Task InsertStep2Async_UpdatesExpectedObstacle_WithInMemorySqlite()
        {
            // Arrange
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();
            var createTableSql = @"
            CREATE TABLE Obstacle
            (
                ObstacleID     TEXT    NOT NULL PRIMARY KEY,
                UserId         TEXT    NOT NULL,
                HeightMeter    INTEGER NOT NULL,
                GeometryGeoJson TEXT   NULL,
                Type           INTEGER NOT NULL,
                Status         INTEGER NOT NULL DEFAULT 0,
                Marking        INTEGER NOT NULL DEFAULT 0,
                Name           TEXT    NULL,
                Description    TEXT    NULL,
                Illuminated    INTEGER NOT NULL DEFAULT 0,
                CreationTime   TEXT    NULL,
                UpdatedTime    TEXT    NULL
            );";
            await connection.ExecuteAsync(createTableSql);
            var repo = CreateRepo(connection);
            var obstacle = new ObstacleDto
            {
                ObstacleId = "2",
                UserId = "1",
                HeightMeter = 15,
                GeometryGeoJson = null,
                Type = 2,
                Status = 0,
                Marking = 1,
                Name = null,
                Description = null,
                Illuminated = 0,
                CreationTime = null,
                UpdatedTime = null
            };
            // Act
            await repo.InsertStep1Async(obstacle);
            // Update GeometryGeoJson
            var updatedObstacle = obstacle with
            {
                GeometryGeoJson = "POINT(30 40)",
                UpdatedTime = DateTime.UtcNow
            };
            await repo.InsertStep2Async(updatedObstacle);
            var fetched = await HelperQueries.FakeGetObstacleById("2", "Obstacle", connection);

            // Assert
            Assert.NotNull(fetched);
            Assert.Equal(updatedObstacle.GeometryGeoJson, fetched.GeometryGeoJson);
        }
        
        [Fact]
        public async Task InsertStep3Async_UpdatesExpectedObstacle_WithInMemorySqlite()
        {
            // Arrange
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();
            var createTableSql = @"
            CREATE TABLE Obstacle
            (
                ObstacleID     TEXT    NOT NULL PRIMARY KEY,
                UserId         TEXT    NOT NULL,
                HeightMeter    INTEGER NOT NULL,
                GeometryGeoJson TEXT   NULL,
                Type           INTEGER NOT NULL,
                Status         INTEGER NOT NULL DEFAULT 0,
                Marking        INTEGER NOT NULL DEFAULT 0,
                Name           TEXT    NULL,
                Description    TEXT    NULL,
                Illuminated    INTEGER NOT NULL DEFAULT 0,
                CreationTime   TEXT    NULL,
                UpdatedTime    TEXT    NULL
            );";
            await connection.ExecuteAsync(createTableSql);
            var repo = CreateRepo(connection);
            var obstacle = new ObstacleDto
            {
                ObstacleId = "3",
                UserId = "1",
                HeightMeter = 20,
                GeometryGeoJson = "POINT(50 60)",
                Type = 3,
                Status = 0,
                Marking = 1,
                Name = null,
                Description = null,
                Illuminated = 0,
                CreationTime = null,
                UpdatedTime = null
            };
            // Act
            await repo.InsertStep1Async(obstacle);
            await repo.InsertStep2Async(obstacle);
            // Update final details
            var updatedObstacle = obstacle with
            {
                Name = "Test Obstacle C",
                Description = "Third test obstacle",
                Illuminated = 1,
                Status = 2,
                Marking = 2,
                UpdatedTime = DateTime.UtcNow
            };
            await repo.InsertStep3Async(updatedObstacle);
            var fetched = await HelperQueries.FakeGetObstacleById("3", "Obstacle", connection);
            // Assert
            Assert.NotNull(fetched);
            Assert.Equal(updatedObstacle.Name, fetched.Name);
            Assert.Equal(updatedObstacle.Description, fetched.Description);
            Assert.Equal(updatedObstacle.Illuminated, fetched.Illuminated);
            Assert.Equal(updatedObstacle.Status, fetched.Status);
            Assert.Equal(updatedObstacle.Marking, fetched.Marking);
        }
    }
}