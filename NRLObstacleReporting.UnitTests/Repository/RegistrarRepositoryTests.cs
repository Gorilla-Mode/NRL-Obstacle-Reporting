using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using JetBrains.Annotations;
using Microsoft.Data.Sqlite;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;
using NRLObstacleReporting.Repositories;
using Xunit;

namespace NRLObstacleReporting.UnitTests.Repository;

[Collection("SqliteInit")] // ensure Batteries.Init() ran
[TestSubject(typeof(RegistrarRepository))]
public class RegistrarRepositoryTests : HelperQueries, IClassFixture<SqliteInitFixture>
{
    public RegistrarRepositoryTests(SqliteInitFixture _)
    {
    }

    private static IRegistrarRepository CreateRegistrarRepository(SqliteConnection connection)
    {
        // Ensure the connection is open before use
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        return new RegistrarRepository(connection);
    }

    protected async Task CreateObstacleUserView(IDbConnection connection, string tableName)
    {
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
                UpdatedTime    TEXT    NULL,
                UserName       TEXT    NULL,
                PhoneNumber    TEXT    NULL,
                Email          TEXT    NULL
                 
            );";
        await connection.ExecuteAsync(createTableSql);
    }

    protected async Task InsertObstacleUserHelper(ViewObstacleUserDto obstacle, IDbConnection connection,
        string tableName)
    {
        var sql =
            $@"INSERT INTO {tableName} 
                          (ObstacleID, Heightmeter, GeometryGeoJson, Name, Description, Illuminated,
                           Type, Status, Marking, UserId, UserName, PhoneNumber, Email)
                    VALUES 
                        ('{obstacle.ObstacleId}', {obstacle.HeightMeter}, '{obstacle.GeometryGeoJson}', '{obstacle.Name}',
                         '{obstacle.Description}', {obstacle.Illuminated}, {obstacle.Type}, {obstacle.Status},
                         {obstacle.Marking}, '{obstacle.UserId}', '{obstacle.UserName}', '{obstacle.PhoneNumber}', '{obstacle.Email}');";

        await connection.ExecuteAsync(sql);
    }

    /// <summary>
    /// Test method to verify that calling GetAllSubmittedObstaclesAsync
    /// returns all obstacle reports with a 'Pending' status.
    /// Ensures that no obstacles with a 'Draft' status are included in the result and
    /// validates the correctness of the returned obstacle data.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation of retrieving and asserting
    /// all submitted obstacles with status 'Pending' from the repository.
    /// </returns>
    [Fact]
    public async Task GetAllSubmittedObstaclesAsync_ShouldReturnAllReports()
    {
        // Arrange
        const string tableName = "Obstacle";
        const string userId = "1234";

        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await CreateObstacleTable(connection, tableName);
        var registrarRepo = CreateRegistrarRepository(connection);

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
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Pending,
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
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Pending,
                CreationTime = null,
                UpdatedTime = null
            },
            new ObstacleDto //not expected, a draft
            {
                ObstacleId = "3",
                UserId = userId,
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
        var result = await registrarRepo.GetAllSubmittedObstaclesAsync();
        var resultList = result.ToList();

        //assert
        Assert.NotNull(result);
        Assert.All(resultList,
            obs => Assert.Equal((int)ObstacleCompleteModel.ObstacleStatus.Pending,
                obs.Status)); //checks all submitted are pending
        Assert.All(resultList,
            obs => Assert.NotEqual((int)ObstacleCompleteModel.ObstacleStatus.Draft,
                obs.Status)); //checks there are no drafts
        Assert.Equal(obstacles[0], resultList[0]);
        Assert.Equal(obstacles[1], resultList[1]);
    }

    /// <summary>
    /// Test method to verify that calling UpdateObstacleStatusAsync correctly updates the status
    /// of a specified obstacle in the repository.
    /// Ensures that the obstacle's updated status matches the expected value and verifies that
    /// the changes are persisted correctly in the database.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation of updating the status of an obstacle and
    /// asserting the correctness of the update against the expected result.
    /// </returns>
    [Fact]
    public async Task UpdateObstacleStatusAsync_ShouldUpdateObstacleStatus()
    {
        // Arrange
        const string tableName = "Obstacle";
        const string userId = "1234";
        const int expectedStatus = (int)ObstacleCompleteModel.ObstacleStatus.Approved;

        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await CreateObstacleTable(connection, tableName);
        var registrarRepo = CreateRegistrarRepository(connection);

        var oldObstacle = new ObstacleDto
        {
            ObstacleId = "1",
            UserId = userId,
            HeightMeter = 20,
            GeometryGeoJson = "blablablablab",
            Name = "Test Obstacle",
            Description = "This is a test obstacle.",
            Status = (int)ObstacleCompleteModel.ObstacleStatus.Pending,
            CreationTime = null,
            UpdatedTime = null
        };

        var updatedObstacle = new ObstacleDto //expected
        {
            ObstacleId = "1",
            UserId = userId,
            HeightMeter = 20,
            GeometryGeoJson = "blablablablab",
            Name = "Test Obstacle",
            Description = "This is a test obstacle.",
            Status = expectedStatus,
            CreationTime = null,
            UpdatedTime = null
        };

        await InsertObstacleHelper(oldObstacle, connection, tableName);

        //act
        await registrarRepo.UpdateObstacleStatusAsync(updatedObstacle);
        var updateObstacleStatus = await FakeGetObstacleById(updatedObstacle.ObstacleId, tableName, connection);

        //assert
        Assert.NotNull(updatedObstacle);
        Assert.Equal(updateObstacleStatus, updatedObstacle);
        Assert.True(updatedObstacle.Status == expectedStatus);
    }

    [Fact]
    public async Task GetObstacleByIdAsync_ShouldReturnObstacleWithSameStatus()
    {
        // Arrange
        const string tableName = "Obstacle";
        const string userId = "1234";
        ObstacleCompleteModel.ObstacleStatus[] filters = new[]
        {
            ObstacleCompleteModel.ObstacleStatus.Approved,
            ObstacleCompleteModel.ObstacleStatus.Pending,
        };

        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await CreateObstacleTable(connection, tableName);
        var registrarRepo = CreateRegistrarRepository(connection);

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
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Pending,
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
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Approved,
                CreationTime = null,
                UpdatedTime = null
            },
            new ObstacleDto //not expected
            {
                ObstacleId = "3",
                UserId = userId,
                HeightMeter = 20,
                GeometryGeoJson = "blablablablab",
                Name = "Test Obstacle",
                Description = "This is a test obstacle.",
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Draft,
                CreationTime = null,
                UpdatedTime = null
            },
            new ObstacleDto //not expected
            {
                ObstacleId = "4",
                UserId = userId,
                HeightMeter = 20,
                GeometryGeoJson = "blablab",
                Name = "Tesle",
                Description = "This is le test go go ga ga",
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Draft,
                CreationTime = null,
                UpdatedTime = null
            },
        };

        foreach (var obstacle in obstacles)
        {
            await InsertObstacleHelper(obstacle, connection, tableName);
        }

        //act
        var result = await registrarRepo.GetObstaclesByStatusAsync(filters);

        //assert
        Assert.NotNull(result);
        Assert.Equal((int)ObstacleCompleteModel.ObstacleStatus.Pending, result[0].Status);
        Assert.Equal((int)ObstacleCompleteModel.ObstacleStatus.Approved, result[1].Status);
        Assert.All(result,
            obs => Assert.NotEqual((int)ObstacleCompleteModel.ObstacleStatus.Draft,
                obs.Status)); // no drafts in collection
    }

    /// <summary>
    /// Test method to ensure that calling GetSubmittedObstacleByIdAsync
    /// returns the correct obstacle data corresponding to the provided obstacle ID.
    /// Verifies that the retrieved obstacle matches the expected data and filters
    /// out obstacles with non-matching IDs.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation of retrieving an obstacle by its ID
    /// and asserting the correctness of the returned obstacle details.
    /// </returns>
    [Fact]
    public async Task GetSubmittedObstacleByIdAsync_ShouldReturnObstacle()
    {
        // Arrange
        const string tableName = "view_ObstacleUser";
        const string userId = "1234";
        const string expectedObstacleId = "1";
        const string expectedInvalidObstacleId = "2";

        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await CreateObstacleUserView(connection, tableName);
        var registrarRepo = CreateRegistrarRepository(connection);

        var obstacles = new List<ViewObstacleUserDto>
        {
            new ViewObstacleUserDto //expected
            {
                ObstacleId = expectedObstacleId,
                UserId = userId,
                HeightMeter = 20,
                GeometryGeoJson = "blablablablab",
                Name = "Test Obstacle",
                Description = "This is a test obstacle.",
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Pending,
                CreationTime = null,
                UpdatedTime = null,
                UserName = "Pilotman",
                PhoneNumber = "1232113",
                Email = "Pilot@pilot.com"
            },
            new ViewObstacleUserDto // Not expected
            {
                ObstacleId = "2",
                UserId = userId,
                HeightMeter = 20,
                GeometryGeoJson = "blablablablab",
                Name = "Test Obstacle",
                Description = "This is a test obstacle.",
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Draft,
                CreationTime = null,
                UpdatedTime = null,
                UserName = "Pilot@pilot.com",
                PhoneNumber = "1232113",
                Email = "Pilot@pilot.com"
            }
        };

        foreach (var obstacle in obstacles)
        {
            await InsertObstacleUserHelper(obstacle, connection, tableName);
        }

        //act
        var result = await registrarRepo.GetSubmittedObstacleByIdAsync(expectedObstacleId);
        Task<ViewObstacleUserDto> InvalidResult() => registrarRepo.GetSubmittedObstacleByIdAsync(expectedInvalidObstacleId);

        //assert
        Assert.NotNull(result);
        Assert.Equal(result, obstacles[0]);
        Assert.Equal(result.ObstacleId, expectedObstacleId);
        await Assert.ThrowsAsync<InvalidOperationException>(InvalidResult);
    }

    /// <summary>
    /// Test method to validate that calling GetObstaclesFilteredAsync with specific filters
    /// retrieves only the obstacles that match the provided criteria. This ensures that the filtering
    /// functionality correctly includes and excludes obstacles based on the defined parameters,
    /// maintaining accuracy in the returned results.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation of retrieving and asserting
    /// the correct filtered obstacle data from the repository based on the specified filters.
    /// </returns>
    [Fact]
    public async Task GetObstaclesFilteredAsync_GivenFilters_ShouldReturnMatchingObstacles()
    {
        // Arrange
        const string tableName = "Obstacle";
        const string userId = "1234";
        ObstacleCompleteModel.ObstacleStatus[] statusFilters = new[]
        {
            ObstacleCompleteModel.ObstacleStatus.Approved,
            ObstacleCompleteModel.ObstacleStatus.Pending,
        };
        ObstacleCompleteModel.ObstacleTypes[] typeFilters = new[]
        {
            ObstacleCompleteModel.ObstacleTypes.Bridge,
            ObstacleCompleteModel.ObstacleTypes.Construction,
        };

        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await CreateObstacleTable(connection, tableName);
        var registrarRepo = CreateRegistrarRepository(connection);

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
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Pending,
                Type = (int)ObstacleCompleteModel.ObstacleTypes.Construction,
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
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Approved,
                Type = (int)ObstacleCompleteModel.ObstacleTypes.Bridge,
                CreationTime = null,
                UpdatedTime = null
            },
            new ObstacleDto //not expected 
            {
                ObstacleId = "3",
                UserId = userId,
                HeightMeter = 20,
                GeometryGeoJson = "blablablablab",
                Name = "Test Obstacle",
                Description = "This is a test obstacle.",
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Rejected,
                Type = (int)ObstacleCompleteModel.ObstacleTypes.PoleOrTower,
                CreationTime = null,
                UpdatedTime = null
            },
            new ObstacleDto //not expected
            {
                ObstacleId = "4",
                UserId = userId,
                HeightMeter = 20,
                GeometryGeoJson = "blablab",
                Name = "Tesle",
                Description = "This is le test go go ga ga",
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Rejected,
                Type = (int)ObstacleCompleteModel.ObstacleTypes.Other,
                CreationTime = null,
                UpdatedTime = null
            },
            new ObstacleDto //not expected
            {
                ObstacleId = "5",
                UserId = userId,
                HeightMeter = 20,
                GeometryGeoJson = "blablab",
                Name = "Tesle",
                Description = "This is le test go go ga ga",
                Type = (int)ObstacleCompleteModel.ObstacleTypes.PoleOrTower,
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Deleted,
                CreationTime = null,
                UpdatedTime = null
            }
        };

        foreach (var obstacle in obstacles)
        {
            await InsertObstacleHelper(obstacle, connection, tableName);
        }
        
        
        //act
        var result = await registrarRepo.GetObstaclesFilteredAsync(
            status: statusFilters, 
            type: typeFilters, 
            illuminations: new ObstacleCompleteModel.Illumination[] { },
            markings: new ObstacleCompleteModel.ObstacleMarking[] { }, default, default);

        //assert
        Assert.NotNull(result);
        Assert.Equal(result[0].Status, (int)ObstacleCompleteModel.ObstacleStatus.Pending);
        Assert.Equal(result[0].Type, (int)ObstacleCompleteModel.ObstacleTypes.Construction);
        Assert.Equal(result[1].Status, (int)ObstacleCompleteModel.ObstacleStatus.Approved);
        Assert.Equal(result[1].Type, (int)ObstacleCompleteModel.ObstacleTypes.Bridge);
        Assert.True(result.Count() == 2);
    }

    /// <summary>
    /// Test method to verify that calling GetObstaclesFilteredAsync with no filters applied
    /// returns all obstacles in the database. Ensures that obstacles with varying statuses
    /// and types are correctly included in the result while validating the integrity of the returned data.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation of retrieving and asserting all obstacles
    /// from the repository, irrespective of their status or type, when no filters are applied.
    /// </returns>
    [Fact]
    public async Task GetObstaclesFilteredAsync_GivenNoFilters_ShouldReturnAllObstacles()
    {
        // Arrange
        const string tableName = "Obstacle";
        const string userId = "1234";

        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await CreateObstacleTable(connection, tableName);
        var registrarRepo = CreateRegistrarRepository(connection);

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
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Pending,
                Type = (int)ObstacleCompleteModel.ObstacleTypes.AirSpan,
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
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Approved,
                Type = (int)ObstacleCompleteModel.ObstacleTypes.Bridge,
                CreationTime = null,
                UpdatedTime = null
            },
            new ObstacleDto //expected
            {
                ObstacleId = "3",
                UserId = userId,
                HeightMeter = 20,
                GeometryGeoJson = "blablablablab",
                Name = "Test Obstacle",
                Description = "This is a test obstacle.",
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Rejected,
                Type = (int)ObstacleCompleteModel.ObstacleTypes.PoleOrTower,
                CreationTime = null,
                UpdatedTime = null
            },
            new ObstacleDto //not expected
            {
                ObstacleId = "4",
                UserId = userId,
                HeightMeter = 20,
                GeometryGeoJson = "blablab",
                Name = "Tesle",
                Description = "This is le test go go ga ga",
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Rejected,
                CreationTime = null,
                UpdatedTime = null
            },
        };

        foreach (var obstacle in obstacles)
        {
            await InsertObstacleHelper(obstacle, connection, tableName);
        }

        //act
        var result = await registrarRepo.GetObstaclesFilteredAsync(
            status: new ObstacleCompleteModel.ObstacleStatus[] { },
            type: new ObstacleCompleteModel.ObstacleTypes[] { },
            illuminations: new ObstacleCompleteModel.Illumination[] { },
            markings: new ObstacleCompleteModel.ObstacleMarking[] { }, default, default);

        //assert
        Assert.NotNull(result);
        Assert.Equal(result, obstacles);
    }

    /// <summary>
    /// Test method to ensure that filtering obstacles by a specified marking using the
    /// GetObstaclesFilteredAsync method returns the correct subset of obstacles.
    /// Verifies that only obstacles with the specified marking are included in the result
    /// and validates the accuracy of the retrieved data.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation that tests filtering obstacles
    /// by marking using GetObstaclesFilteredAsync and asserts the correctness of the filtered result.
    /// </returns>
    [Fact]
    public async Task GetObstaclesFilteredAsync_FilterByMarkReturnsCorrectly()
    {
        // Arrange
        const string tableName = "Obstacle";
        const string userId = "1234";
        ObstacleCompleteModel.ObstacleMarking[] markings = new[]
        {
            ObstacleCompleteModel.ObstacleMarking.Marked
        };
        

        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await CreateObstacleTable(connection, tableName);
        var registrarRepo = CreateRegistrarRepository(connection);

        var obstacles = new List<ObstacleDto>
        {

            new ObstacleDto
            {
                ObstacleId = "1",
                UserId = userId,
                HeightMeter = 20,
                GeometryGeoJson = "blablablablab",
                Name = "Test Obstacle",
                Description = "This is a test obstacle.",
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Pending,
                Type = (int)ObstacleCompleteModel.ObstacleTypes.Construction,
                Marking = (int)ObstacleCompleteModel.ObstacleMarking.Marked,
                Illuminated = (int)ObstacleCompleteModel.Illumination.Illuminated,
                CreationTime = null,
                UpdatedTime = null
            },
            new ObstacleDto
            {
                ObstacleId = "2",
                UserId = userId,
                HeightMeter = 20,
                GeometryGeoJson = "blablablablab",
                Name = "Test Obstacle",
                Description = "This is a test obstacle.",
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Approved,
                Type = (int)ObstacleCompleteModel.ObstacleTypes.Other,
                Marking = (int)ObstacleCompleteModel.ObstacleMarking.Unknown,
                Illuminated = (int)ObstacleCompleteModel.Illumination.NotIlluminated,
                CreationTime = null,
                UpdatedTime = null
            }

        };
        
        
        foreach (var obstacle in obstacles)
        {
            await InsertObstacleHelper(obstacle, connection, tableName);
        }
        
        
        //act
        var result = await registrarRepo.GetObstaclesFilteredAsync(
            status: new ObstacleCompleteModel.ObstacleStatus[] { }, 
            type: new ObstacleCompleteModel.ObstacleTypes[] { }, 
            illuminations: new ObstacleCompleteModel.Illumination[] { },
            markings: markings, 
            default,
            default);

        //assert
        Assert.NotNull(result);
        Assert.Equal(result[0], obstacles[0]);
        Assert.True(result.Count() == 1);
    }

    /// <summary>
    /// Test method to verify that calling GetObstaclesFilteredAsync with a filter
    /// for specific illumination levels returns only the obstacles matching the illumination criteria.
    /// Ensures that obstacles with 'Illuminated' status are retrieved, while others are excluded,
    /// validating the correct behavior of the filtering logic.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation of filtering and asserting obstacles based
    /// on illumination levels from the repository.
    /// </returns>
    [Fact]
    public async Task GetObstaclesFilteredAsync_FilterByIlluminationReturnsCorrectly()
    {
        // Arrange
        const string tableName = "Obstacle";
        const string userId = "1234";
        ObstacleCompleteModel.Illumination[] illuminations = new[]
        {
            ObstacleCompleteModel.Illumination.Illuminated
        };
        

        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await CreateObstacleTable(connection, tableName);
        var registrarRepo = CreateRegistrarRepository(connection);

        var obstacles = new List<ObstacleDto>
        {

            new ObstacleDto
            {
                ObstacleId = "1",
                UserId = userId,
                HeightMeter = 20,
                GeometryGeoJson = "blablablablab",
                Name = "Test Obstacle",
                Description = "This is a test obstacle.",
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Pending,
                Type = (int)ObstacleCompleteModel.ObstacleTypes.Construction,
                Marking = (int)ObstacleCompleteModel.ObstacleMarking.Marked,
                Illuminated = (int)ObstacleCompleteModel.Illumination.Illuminated,
                CreationTime = null,
                UpdatedTime = null
            },
            new ObstacleDto
            {
                ObstacleId = "2",
                UserId = userId,
                HeightMeter = 20,
                GeometryGeoJson = "blablablablab",
                Name = "Test Obstacle",
                Description = "This is a test obstacle.",
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Approved,
                Type = (int)ObstacleCompleteModel.ObstacleTypes.Other,
                Marking = (int)ObstacleCompleteModel.ObstacleMarking.Unknown,
                Illuminated = (int)ObstacleCompleteModel.Illumination.NotIlluminated,
                CreationTime = null,
                UpdatedTime = null
            }

        };
        
        
        foreach (var obstacle in obstacles)
        {
            await InsertObstacleHelper(obstacle, connection, tableName);
        }
        
        //act
        var result = await registrarRepo.GetObstaclesFilteredAsync(
            status: new ObstacleCompleteModel.ObstacleStatus[] { }, 
            type: new ObstacleCompleteModel.ObstacleTypes[] { }, 
            illuminations: illuminations,
            markings: new ObstacleCompleteModel.ObstacleMarking[]{}, 
            default,
            default);

        //assert
        Assert.NotNull(result);
        Assert.Equal(result[0], obstacles[0]);
        Assert.True(result.Count() == 1);
    }

    /// <summary>
    /// Test method to verify that calling GetObstaclesFilteredAsync with a specific obstacle type filter
    /// returns only obstacles of the specified type. Ensures that obstacles of other types
    /// are excluded from the result.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation of retrieving and asserting filtered
    /// obstacles based on the provided type criteria.
    /// </returns>
    [Fact]
    public async Task GetObstaclesFilteredAsync_FilterByTypeReturnsCorrectly()
    {
        // Arrange
        const string tableName = "Obstacle";
        const string userId = "1234";
        ObstacleCompleteModel.ObstacleTypes[] types = new[]
        {
            ObstacleCompleteModel.ObstacleTypes.Construction
        };
        

        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await CreateObstacleTable(connection, tableName);
        var registrarRepo = CreateRegistrarRepository(connection);

        var obstacles = new List<ObstacleDto>
        {

            new ObstacleDto
            {
                ObstacleId = "1",
                UserId = userId,
                HeightMeter = 20,
                GeometryGeoJson = "blablablablab",
                Name = "Test Obstacle",
                Description = "This is a test obstacle.",
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Pending,
                Type = (int)ObstacleCompleteModel.ObstacleTypes.Construction,
                Marking = (int)ObstacleCompleteModel.ObstacleMarking.Marked,
                Illuminated = (int)ObstacleCompleteModel.Illumination.Illuminated,
                CreationTime = null,
                UpdatedTime = null
            },
            new ObstacleDto
            {
                ObstacleId = "2",
                UserId = userId,
                HeightMeter = 20,
                GeometryGeoJson = "blablablablab",
                Name = "Test Obstacle",
                Description = "This is a test obstacle.",
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Approved,
                Type = (int)ObstacleCompleteModel.ObstacleTypes.Other,
                Marking = (int)ObstacleCompleteModel.ObstacleMarking.Unknown,
                Illuminated = (int)ObstacleCompleteModel.Illumination.NotIlluminated,
                CreationTime = null,
                UpdatedTime = null
            }

        };
        
        
        foreach (var obstacle in obstacles)
        {
            await InsertObstacleHelper(obstacle, connection, tableName);
        }
        
        
        //act
        var result = await registrarRepo.GetObstaclesFilteredAsync(
            status: new ObstacleCompleteModel.ObstacleStatus[] { }, 
            type: types, 
            illuminations: new ObstacleCompleteModel.Illumination[] { },
            markings: new ObstacleCompleteModel.ObstacleMarking[] { }, 
            default,
            default);

        //assert
        Assert.NotNull(result);
        Assert.Equal(result[0], obstacles[0]);
        Assert.True(result.Count() == 1);
    }

    /// <summary>
    /// Test method to verify that calling GetObstaclesFilteredAsync with a specific obstacle status
    /// filter returns only the obstacles that match the given status.
    /// Ensures that obstacles with statuses not included in the filter are excluded from the result.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation of retrieving and asserting
    /// the filtered obstacle data based on the provided status criteria.
    /// </returns>
    [Fact]
    public async Task GetObstaclesFilteredAsync_FilterByStatusReturnsCorrectly()
    {
        // Arrange
        const string tableName = "Obstacle";
        const string userId = "1234";
        ObstacleCompleteModel.ObstacleStatus[] Statuses = new[]
        {
            ObstacleCompleteModel.ObstacleStatus.Pending,
        };
        
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        await CreateObstacleTable(connection, tableName);
        var registrarRepo = CreateRegistrarRepository(connection);

        var obstacles = new List<ObstacleDto>
        {

            new ObstacleDto
            {
                ObstacleId = "1",
                UserId = userId,
                HeightMeter = 20,
                GeometryGeoJson = "blablablablab",
                Name = "Test Obstacle",
                Description = "This is a test obstacle.",
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Pending,
                Type = (int)ObstacleCompleteModel.ObstacleTypes.Construction,
                Marking = (int)ObstacleCompleteModel.ObstacleMarking.Marked,
                Illuminated = (int)ObstacleCompleteModel.Illumination.Illuminated,
                CreationTime = null,
                UpdatedTime = null
            },
            new ObstacleDto
            {
                ObstacleId = "2",
                UserId = userId,
                HeightMeter = 20,
                GeometryGeoJson = "blablablablab",
                Name = "Test Obstacle",
                Description = "This is a test obstacle.",
                Status = (int)ObstacleCompleteModel.ObstacleStatus.Approved,
                Type = (int)ObstacleCompleteModel.ObstacleTypes.Other,
                Marking = (int)ObstacleCompleteModel.ObstacleMarking.Unknown,
                Illuminated = (int)ObstacleCompleteModel.Illumination.NotIlluminated,
                CreationTime = null,
                UpdatedTime = null
            }
        };
        
        foreach (var obstacle in obstacles)
        {
            await InsertObstacleHelper(obstacle, connection, tableName);
        }
        
        
        //act
        var result = await registrarRepo.GetObstaclesFilteredAsync(
            status: Statuses, 
            type: new ObstacleCompleteModel.ObstacleTypes[] { }, 
            illuminations: new ObstacleCompleteModel.Illumination[] { },
            markings: new ObstacleCompleteModel.ObstacleMarking[]{}, 
            default,
            default);

        //assert
        Assert.NotNull(result);
        Assert.Equal(result[0], obstacles[0]);
        Assert.True(result.Count() == 1);
    }
}