using Dapper;
using Microsoft.Data.Sqlite;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;
using NRLObstacleReporting.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

        [Fact]
        public async Task GetObstacleByIdAsync_ReturnsExpectedObstacle_WithInMemorySqlite()
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
                ObstacleId = "4",
                UserId = "1",
                HeightMeter = 25,
                GeometryGeoJson = "POINT(70 80)",
                Type = 4,
                Status = 0,
                Marking = 1,
                Name = "Test Obstacle D",
                Description = "Fourth test obstacle",
                Illuminated = 0,
                CreationTime = null,
                UpdatedTime = null
            };

            await repo.InsertStep1Async(obstacle);
            // Act
            var fetched = await repo.GetObstacleByIdAsync("4");
            // Assert
            Assert.NotNull(fetched);
            Assert.Equal(obstacle.ObstacleId, fetched.ObstacleId);
            Assert.Equal("4", fetched.ObstacleId);
            Assert.Equal(obstacle.HeightMeter, fetched.HeightMeter);
        }

        [Fact]
        public async Task GetAllSubmittedObstaclesAsync_ReturnsExpectedObstacle_WithInMemorySqlite()
        {
            const string tableName = "Obstacle";
            
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();
            var createTableSql = @"
        CREATE TABLE Obstacle
        (
            ObstacleID      TEXT    NOT NULL PRIMARY KEY,
            UserId          TEXT    NOT NULL,
            HeightMeter     INTEGER NOT NULL,
            GeometryGeoJson TEXT    NULL,
            Type            INTEGER NOT NULL,
            Status          INTEGER NOT NULL DEFAULT 0,
            Marking         INTEGER NOT NULL DEFAULT 0,
            Name            TEXT    NULL,
            Description     TEXT    NULL,
            Illuminated     INTEGER NOT NULL DEFAULT 0,
            CreationTime    TEXT    NULL,
            UpdatedTime     TEXT    NULL
        );";
            await connection.ExecuteAsync(createTableSql);

            var repo = new ObstacleRepository(connection);

            // Insert obstacles for user "u1" and "u2" with mixed statuses
            var now = DateTime.UtcNow;
            var obstacles = new[]
            {
            new ObstacleDto { ObstacleId = "a", UserId = "u1", HeightMeter = 10, GeometryGeoJson = null, Type = 1, Status = 0, Marking = 0, Name = "Draft A", Description = null, Illuminated = 0, CreationTime = now.AddMinutes(-10), UpdatedTime = null }, // Draft
            new ObstacleDto { ObstacleId = "b", UserId = "u1", HeightMeter = 11, GeometryGeoJson = null, Type = 1, Status = 1, Marking = 0, Name = "Submitted B", Description = null, Illuminated = 0, CreationTime = now.AddMinutes(-5), UpdatedTime = null },  // Non-draft
            new ObstacleDto { ObstacleId = "c", UserId = "u1", HeightMeter = 12, GeometryGeoJson = null, Type = 1, Status = 2, Marking = 0, Name = "Submitted C", Description = null, Illuminated = 0, CreationTime = now.AddMinutes(-1), UpdatedTime = null },  // Non-draft
            new ObstacleDto { ObstacleId = "d", UserId = "u2", HeightMeter = 13, GeometryGeoJson = null, Type = 1, Status = 2, Marking = 0, Name = "Other user D", Description = null, Illuminated = 0, CreationTime = now.AddMinutes(-2), UpdatedTime = null },         // Different user
            new ObstacleDto { ObstacleId = "e", UserId = "u3", HeightMeter = 14, GeometryGeoJson = null, Type = 1, Status = 2, Marking = 0, Name = "Other user D", Description = null, Illuminated = 0, CreationTime = now.AddMinutes(-4), UpdatedTime = null }         // Different user

        };
            foreach (var obstacle in obstacles)
            {
                await InsertObstacleHelper(obstacle, connection, tableName);
            }
            
            var result = (await repo.GetAllSubmittedObstaclesAsync("u1")).ToList();


            Assert.True(result.All(r => r.UserId == "u1"), "All returned obstacles must belong to user 'u1'");
            Assert.Equal(2, result.Count); // only b and c
            Assert.All(result, r => Assert.Equal("u1", r.UserId));
            Assert.DoesNotContain(result, r => r.Status == 0);

        }
    }
    }
