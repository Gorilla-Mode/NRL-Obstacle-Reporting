using System;
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
    [Collection("SqliteInit")]
    public class ObstacleRepositoryTests : IClassFixture<SqliteInitFixture>
    {
        // Only the SQLite init fixture is injected.
        public ObstacleRepositoryTests(SqliteInitFixture _) { }

        private static IObstacleRepository CreateRepo(SqliteConnection connection)
        {
            // Ensure the connection is open before use
            if (connection.State != System.Data.ConnectionState.Open)
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

            var fetched = await repo.GetObstacleByIdAsync("1");


            // Assert
            Assert.NotNull(fetched);
            Assert.Equal(obstacle.HeightMeter, fetched.HeightMeter);
            Assert.Equal(obstacle.ObstacleId, fetched.ObstacleId);
        }
    } 
    }