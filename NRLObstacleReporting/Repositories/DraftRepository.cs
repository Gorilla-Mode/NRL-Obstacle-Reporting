using Dapper;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.Repositories;

public sealed class DraftRepository : RepositoryBase, IDraftRepository
{
    /// <inheritdoc/>
    public async Task EditDraftAsync(ObstacleDto data)
    {
        using var connection = CreateConnection();
        
        var sql = @"UPDATE Obstacle 
                    SET Heightmeter = @HeightMeter, GeometryGeoJson = @GeometryGeoJson, Name = @Name, 
                        Description = @Description, Illuminated = @Illuminated, Type = @Type, Status = @Status,
                        Marking = @Marking, UpdatedTime = @UpdatedTime
                    WHERE ObstacleID = @ObstacleId AND UserId = @UserId";
        
        await connection.ExecuteAsync(sql, data);
    }

    /// <inheritdoc/>
    public async Task SubmitDraftAsync(ObstacleDto data)
    {
        using var connection = CreateConnection();
        
        const int statusId = (int)ObstacleCompleteModel.ObstacleStatus.Pending;
        
        var sql = @$"UPDATE Obstacle
                     SET Status = {statusId} 
                     WHERE ObstacleID = @ObstacleID AND UserId = @UserId";
        
        await connection.ExecuteAsync(sql, data);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ObstacleDto>> GetAllDraftsAsync(string userId)
    {
        using var connection = CreateConnection();
        const int statusId = (int)ObstacleCompleteModel.ObstacleStatus.Draft;
        
        var sql = @$"SELECT * 
                     FROM Obstacle 
                     WHERE Status = {statusId} AND UserId = @userId
                     ORDER BY CreationTime DESC";
        
        return await connection.QueryAsync<ObstacleDto>(sql, new { UserId = userId });
    }

    /// <inheritdoc/>
    public async Task<ObstacleDto> GetDraftByIdAsync(string obstacleId, string userId)
    {
        using var connection = CreateConnection();
        var sql = @"SELECT *
                    FROM Obstacle
                    WHERE ObstacleID = @obstacleId AND UserId = @userId";

        return await connection.QuerySingleAsync<ObstacleDto>(sql, new { UserId = userId, ObstacleId = obstacleId });
    }
}