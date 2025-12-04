using System.Data;
using Dapper;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.Repositories;

public sealed class ObstacleRepository : RepositoryBase, IObstacleRepository
{
    private readonly IDbConnection _connection;
    
    public ObstacleRepository()
    {
        _connection = CreateConnection();
    }
    
    public ObstacleRepository(IDbConnection mockconnection)
    {
        _connection = mockconnection;
    }
    
    /// <inheritdoc/>
    public async Task InsertStep1Async(ObstacleDto data)
    {
        var sql = @"INSERT INTO Obstacle (ObstacleID, Heightmeter, Type, GeometryGeoJson, CreationTime, UpdatedTime, UserId) 
                    VALUES (@ObstacleId, @HeightMeter, @Type, @GeometryGeoJson,  @CreationTime, @UpdatedTime, @UserId)"; 
        
        await  _connection.ExecuteAsync(sql, data);
    }

    /// <inheritdoc/>
    public async Task InsertStep2Async(ObstacleDto data)
    {
        var sql = @"UPDATE Obstacle 
                    SET GeometryGeoJson = @GeometryGeoJson, UpdatedTime = @UpdatedTime 
                    WHERE ObstacleID = @ObstacleId AND UserId = @UserId";
        
        await _connection.ExecuteAsync(sql, data);
    }

    /// <inheritdoc/>
    public async Task InsertStep3Async(ObstacleDto data)
    {
        var sql = @"UPDATE Obstacle 
                    SET Name = @Name, Description = @Description, Illuminated = @Illuminated, Status = @Status, 
                        Marking = @Marking, UpdatedTime = @UpdatedTime 
                    WHERE ObstacleID = @ObstacleId AND UserId = @UserId";
        
        await _connection.ExecuteAsync(sql, data);
    }

    /// <inheritdoc/>
    public async Task<ObstacleDto> GetObstacleByIdAsync(string? id)
    {
        var sql = @"SELECT * 
                    FROM Obstacle 
                    WHERE ObstacleId = @id";

        return await _connection.QuerySingleAsync<ObstacleDto>(sql, new { id = id });
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ObstacleDto>> GetAllSubmittedObstaclesAsync(string? userId)
    {
        var sql = @$"SELECT * FROM Obstacle
                     WHERE Status <> {(int)ObstacleCompleteModel.ObstacleStatus.Draft} AND UserId = @userId
                     ORDER BY CreationTime DESC";

        return await _connection.QueryAsync<ObstacleDto>(sql, new { userId = userId });
    }
}
