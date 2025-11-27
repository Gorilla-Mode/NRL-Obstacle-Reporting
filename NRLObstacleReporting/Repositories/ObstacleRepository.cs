using Dapper;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.Repositories;

public class ObstacleRepository : RepositoryBase, IObstacleRepository
{
    /// <inheritdoc/>
    public async Task InsertStep1Async(ObstacleDto data)
    {
        using var connection = CreateConnection();
        var sql = @"INSERT INTO Obstacle (ObstacleID, Heightmeter, Type, GeometryGeoJson, CreationTime, UpdatedTime, UserId) 
                    VALUES (@ObstacleId, @HeightMeter, @Type, @GeometryGeoJson,  @CreationTime, @UpdatedTime, @UserId)"; 
        await connection.ExecuteAsync(sql, data);
    }

    /// <inheritdoc/>
    public async Task InsertStep2Async(ObstacleDto data)
    {
        using var connection = CreateConnection();
        var sql = @"UPDATE Obstacle 
                    SET GeometryGeoJson = @GeometryGeoJson, UpdatedTime = @UpdatedTime 
                    WHERE ObstacleID = @ObstacleId AND UserId = @UserId";
        await connection.ExecuteAsync(sql, data);
    }

    /// <inheritdoc/>
    public async Task InsertStep3Async(ObstacleDto data)
    {
        using var connection = CreateConnection();
        var sql = @"UPDATE Obstacle 
                    SET Name = @Name, Description = @Description, Illuminated = @Illuminated, Status = @Status, 
                        Marking = @Marking, UpdatedTime = @UpdatedTime 
                    WHERE ObstacleID = @ObstacleId AND UserId = @UserId";
        await connection.ExecuteAsync(sql, data);

    }

    /// <inheritdoc/>
    public async Task<ObstacleDto> GetObstacleByIdAsync(string? id)
    {
        using var connection = CreateConnection();
        
        var sql = @"SELECT * 
                    FROM Obstacle 
                    WHERE ObstacleID = @id";

        return await connection.QuerySingleAsync<ObstacleDto>(sql, new { Id = id });
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ObstacleDto>> GetAllSubmittedObstaclesAsync(string? userId)
    {
        using var connection = CreateConnection();
        
        var sql = @$"SELECT * FROM Obstacle WHERE Status <> {(int)ObstacleCompleteModel.ObstacleStatus.Draft} AND UserId = @userId
                     ORDER BY CreationTime DESC";

        return await connection.QueryAsync<ObstacleDto>(sql, new { UserId = userId });
    }
}
