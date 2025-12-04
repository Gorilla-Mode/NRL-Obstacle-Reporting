using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Data.Sqlite;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;
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
        /// Validates that the EditDraftAsync method in the DraftRepository updates the draft information in the database.
        /// Verifies the updated fields of the draft, ensuring the data changes persist as expected.
        /// </summary>
        /// <returns>
        /// A completed task after verifying the draft has been successfully updated in the database.
        /// The updated fields in the database should match the provided data.
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

        /// <summary>
        /// Validates that the SubmitDraftAsync method in the DraftRepository successfully updates the status of a draft obstacle.
        /// Ensures that the status of a draft obstacle changes to 'Pending' upon submission and persists these changes in the database.
        /// </summary>
        /// <returns>
        /// A completed task after verifying that the draft obstacle's status has been updated to 'Pending' in the database.
        /// Verifies that the updated obstacle data in the database matches the expected submitted state.
        /// </returns>
        [Fact]
        public async Task SubmitDraftAsync_ShouldChangeStatus()
        {
            // Arrange
            const string tableName = "Obstacle";
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();
            
            await CreateObstacleTable(connection, tableName);

            var draftRepository = CreateRepo(connection); 
            var draftObstacle = new ObstacleDto
            {
                ObstacleId = "1",
                UserId = "1234", 
                HeightMeter = 20,
                GeometryGeoJson = "blablablablab",
                Name = "Test Obstacle",
                Description = "This is a test obstacle.",
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Draft,
                CreationTime = null,
                UpdatedTime = null
            };

            var expecedSubmittedObstacle = new ObstacleDto
            {
                ObstacleId = "1",
                UserId = "1234", 
                HeightMeter = 20,
                GeometryGeoJson = "blablablablab",
                Name = "Test Obstacle",
                Description = "This is a test obstacle.",
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Pending,
                CreationTime = null,
                UpdatedTime = null
            };
            
            await InsertObstacleHelper(draftObstacle, connection, tableName); // Use helper to mock old data

            // Act
            await draftRepository.SubmitDraftAsync(draftObstacle);
            var result = await FakeGetObstacleById(draftObstacle.ObstacleId, tableName, connection);
            
            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(result, draftObstacle);
            Assert.Equal(expecedSubmittedObstacle, result);
        }

        /// <summary>
        /// Validates that the GetAllDraftsAsync method in the DraftRepository retrieves all draft obstacles
        /// associated with the specified user from the database. Ensures the retrieved drafts are filtered
        /// correctly based on user ID and status.
        /// </summary>
        /// <returns>
        /// A completed task after verifying that only draft obstacles specific to the user are retrieved.
        /// Ensures that no obstacles with a different status or associated with a different user are included.
        /// </returns>
        [Fact]
        public async Task GetAllDraftsAsync_ShouldGetAllDrafts()
        {
            // Arrange
            const string tableName = "Obstacle";
            const string userId = "1234";
            
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();
            await CreateObstacleTable(connection, tableName);
            var draftRepository = CreateRepo(connection);
            
            var obstacles = new List<ObstacleDto>
            {
                new ObstacleDto //expected
                {
                    ObstacleId = "1",
                    UserId = userId,
                    HeightMeter = 20,
                    GeometryGeoJson = "blablablablab",
                    Name = "Test Obstacle",
                    Description = "This is a test obstacle.",
                    Status = (int)ObstacleCompleteModel.ObstacleStatus.Draft,
                    CreationTime = null,
                    UpdatedTime = null
                },
                new ObstacleDto //expected
                {
                    ObstacleId = "2",
                    UserId = userId,
                    HeightMeter = 20,
                    GeometryGeoJson = "blablablablab",
                    Name = "Test Obstacle",
                    Description = "This is a test obstacle.",
                    Status = (int)ObstacleCompleteModel.ObstacleStatus.Draft,
                    CreationTime = null,
                    UpdatedTime = null
                },
                new ObstacleDto //not expected, not a draft
                {
                    ObstacleId = "3",
                    UserId = userId,
                    HeightMeter = 20,
                    GeometryGeoJson = "blablablablab",
                    Name = "Test Obstacle",
                    Description = "This is a test obstacle.",
                    Status = (int)ObstacleCompleteModel.ObstacleStatus.Approved,
                    CreationTime = null,
                    UpdatedTime = null
                },
                new ObstacleDto //not expected, diff user
                {
                    ObstacleId = "4",
                    UserId = "34",
                    HeightMeter = 20,
                    GeometryGeoJson = "blablablablab",
                    Name = "Test Obstacle",
                    Description = "This is a test obstacle.",
                    Status = (int)ObstacleCompleteModel.ObstacleStatus.Draft,
                    CreationTime = null,
                    UpdatedTime = null
                }
                
            };
            
            foreach (var obstacle in obstacles)
            {
                await InsertObstacleHelper(obstacle, connection, tableName);
            }
            
            //act
            var result = await draftRepository.GetAllDraftsAsync(userId);
            var resultList = result.ToList();
            
            //assert
            Assert.NotNull(result);
            Assert.Equal(2, resultList.Count);
            Assert.Equal(obstacles[0], resultList[0]);
            Assert.Equal(obstacles[1], resultList[1]);
            Assert.True(resultList[1].Status == (int)ObstacleCompleteModel.ObstacleStatus.Draft);
            Assert.True(resultList[0].UserId == userId);
        }

        /// <summary>
        /// Validates that the GetDraftByIdAsync method in the DraftRepository retrieves the correct draft by its unique ID and associated user ID.
        /// Ensures that the retrieved draft matches the expected properties, filtering out items that are not drafts or do not belong to the specified user.
        /// </summary>
        /// <returns>
        /// A completed task after verifying the retrieved draft matches the expected properties, including the correct ID, user ID, and draft status.
        /// </returns>
        [Fact]
        public async Task GetDraftByIdAsync_ShouldADraftById()
        {
            // Arrange
            const string tableName = "Obstacle";
            const string userId = "1234";
            
            await using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();
            await CreateObstacleTable(connection, tableName);
            var draftRepository = CreateRepo(connection);
            
            var obstacles = new List<ObstacleDto>
            {
                new ObstacleDto //expected
                {
                    ObstacleId = "1",
                    UserId = userId,
                    HeightMeter = 20,
                    GeometryGeoJson = "blablablablab",
                    Name = "Test Obstacle",
                    Description = "This is a test obstacle.",
                    Status = (int)ObstacleCompleteModel.ObstacleStatus.Draft,
                    CreationTime = null,
                    UpdatedTime = null
                },
                new ObstacleDto // expected
                {
                    ObstacleId = "2",
                    UserId = userId,
                    HeightMeter = 20,
                    GeometryGeoJson = "blablablablab",
                    Name = "Test Obstacle",
                    Description = "This is a test obstacle.",
                    Status = (int)ObstacleCompleteModel.ObstacleStatus.Draft,
                    CreationTime = null,
                    UpdatedTime = null
                },
                new ObstacleDto //not expected, not a draft
                {
                    ObstacleId = "3",
                    UserId = userId,
                    HeightMeter = 20,
                    GeometryGeoJson = "blablablablab",
                    Name = "Test Obstacle",
                    Description = "This is a test obstacle.",
                    Status = (int)ObstacleCompleteModel.ObstacleStatus.Approved,
                    CreationTime = null,
                    UpdatedTime = null
                },
                new ObstacleDto //not expected, diff user
                {
                    ObstacleId = "4",
                    UserId = "34",
                    HeightMeter = 20,
                    GeometryGeoJson = "blablablablab",
                    Name = "Test Obstacle",
                    Description = "This is a test obstacle.",
                    Status = (int)ObstacleCompleteModel.ObstacleStatus.Draft,
                    CreationTime = null,
                    UpdatedTime = null
                }
                
            };
            
            foreach (var obstacle in obstacles)
            {
                await InsertObstacleHelper(obstacle, connection, tableName);
            }
            
            //act
            var result = await draftRepository.GetDraftByIdAsync("1", userId);
            
            //assert
            Assert.NotNull(result);
            Assert.Equal(obstacles[0], result);
            Assert.True(result.Status == (int)ObstacleCompleteModel.ObstacleStatus.Draft);
            Assert.True(result.UserId == userId);
        }
    }
}