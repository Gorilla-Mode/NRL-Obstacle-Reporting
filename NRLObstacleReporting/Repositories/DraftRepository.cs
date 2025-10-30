using Dapper;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.Repositories;

public class DraftRepository : RepositoryBase, IDraftRepository
{
    
    //TODO: Implements these things
    public async Task EditDraft(ObstacleDto data)
    {
        using var connection = CreateConnection();
        var sql = @"UPDATE Obstacle 
                    SET Heightmeter = @HeightMeter, GeometryGeoJson = @GeometryGeoJson, Name = @Name, 
                        Description = @Description, Illuminated = @Illuminated, Type = @Type, Status = @Status,
                        Marking = @Marking
                    WHERE ObstacleID = @ObstacleId";
        
        await connection.ExecuteAsync(sql, data);
    }

    public async Task SubmitDraft(ObstacleDto data)
    {
        using var connection = CreateConnection();
        const int statusId = (int)ObstacleCompleteModel.ObstacleStatus.Pending;
        var sql = @$"UPDATE Obstacle
                     SET Status = {statusId} 
                     WHERE ObstacleID = @ObstacleID";
        
        await connection.ExecuteAsync(sql, data);
    }

    public async Task<IEnumerable<ObstacleDto>> GetAllDrafts()
    {
        using var connection = CreateConnection();
        const int statusId = (int)ObstacleCompleteModel.ObstacleStatus.Draft;
        var sql = @$"SELECT * 
                     FROM Obstacle 
                     WHERE Status = {statusId}";
        
        return await connection.QueryAsync<ObstacleDto>(sql);
    }
}