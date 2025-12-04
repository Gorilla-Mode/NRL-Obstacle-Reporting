using System.Data;
using Dapper;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.Repositories;

public sealed class DraftRepository : RepositoryBase, IDraftRepository
{
    private readonly IDbConnection _connection;

    public DraftRepository()
    {
        _connection = CreateConnection();
    }

    public DraftRepository(IDbConnection mockconnection)
    {
        _connection = mockconnection;
    }

    /// <inheritdoc/>
    public async Task EditDraftAsync(ObstacleDto data)
    {
        var sql = @"UPDATE Obstacle 
                    SET Heightmeter = @HeightMeter, GeometryGeoJson = @GeometryGeoJson, Name = @Name, 
                        Description = @Description, Illuminated = @Illuminated, Type = @Type, Status = @Status,
                        Marking = @Marking, UpdatedTime = @UpdatedTime
                    WHERE ObstacleID = @ObstacleId AND UserId = @UserId";

        await _connection.ExecuteAsync(sql, data);
    }

    /// <inheritdoc/>
    public async Task SubmitDraftAsync(ObstacleDto data)
    {
        const int statusId = (int)ObstacleCompleteModel.ObstacleStatus.Pending;

        var sql = @$"UPDATE Obstacle
                     SET Status = {statusId} 
                     WHERE ObstacleID = @ObstacleID AND UserId = @UserId";

        await _connection.ExecuteAsync(sql, data);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ObstacleDto>> GetAllDraftsAsync(string userId)
    {
        const int statusId = (int)ObstacleCompleteModel.ObstacleStatus.Draft;

        var sql = @$"SELECT * 
                     FROM Obstacle 
                     WHERE Status = {statusId} AND UserId = @userId
                     ORDER BY CreationTime DESC";

        return await _connection.QueryAsync<ObstacleDto>(sql, new { UserId = userId });
    }

    /// <inheritdoc/>
    public async Task<ObstacleDto> GetDraftByIdAsync(string obstacleId, string userId)
    {
        var sql = @"SELECT *
                    FROM Obstacle
                    WHERE ObstacleID = @obstacleId AND UserId = @userId";

        return await _connection.QuerySingleAsync<ObstacleDto>(sql, new { UserId = userId, ObstacleId = obstacleId });
    }
}