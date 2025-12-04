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
    [Collection("SqliteInit")] // ensure Batteries.Init() ran
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

        private async Task InsertObstacleHelper(ObstacleDto obstacle, IDbConnection connection, string tableName)
        {
            var sql = $@"INSERT INTO {tableName} 
                                      (ObstacleID, Heightmeter, GeometryGeoJson, Name, Description, Illuminated,
                                       Type, Status, Marking, UserId)
                                VALUES 
                                    ('{obstacle.ObstacleId}', {obstacle.HeightMeter}, '{obstacle.GeometryGeoJson}', '{obstacle.Name}',
                                     '{obstacle.Description}', {obstacle.Illuminated}, {obstacle.Type}, {obstacle.Status},
                                     {obstacle.Marking}, '{obstacle.UserId}');";
            
            await connection.ExecuteAsync(sql);
        }

        [Fact]
        public async Task EditDraftAsync_Should_UpdateObstacle_When_ValidDataIsProvided()
        {
            // Arrange
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();
            const string tableName = "Obstacle";
            
            var createTableSql = $@"
            CREATE TABLE {tableName}
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

            var draftRepository = CreateRepo(connection); // Replace with actual repository initialization
            var updatedObstacle = new ObstacleDto
            {
                ObstacleId = "1",
                UserId = "1234", // Replace with a valid test UserId
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
            
            // In-memory setup (e.g., use SQLite or mocks)
            await InsertObstacleHelper(initialData, connection, tableName); // Mock inserting initial data

            // Act
            await draftRepository.EditDraftAsync(updatedObstacle);
            var result = await HelperQueries.FakeGetObstacleById(updatedObstacle.ObstacleId, tableName, connection);
            // Assert
            
            Assert.NotEqual(result, initialData);
            Assert.NotNull(result);
            Assert.Equal(updatedObstacle.ObstacleId, result.ObstacleId);
            Assert.Equal(updatedObstacle, result);
        }
    }
}