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
                    SET Type = @Type, Heightmeter = @HeightMeter, Name = @Name, Description = @Description,
                        Illuminated = @Illuminated
                    WHERE ObstacleID = @ObstacleId";
        
        await connection.ExecuteAsync(sql, data);
    }

    public async Task SubmitDraft(int id)
    {
        using var connection = CreateConnection();
        const int statusId = (int)ObstacleCompleteModel.ObstacleStatus.Pending;
        var sql = @$"UPDATE Obstacle
                     SET Status = {statusId} 
                     WHERE ObstacleID = @id";
        
        await connection.ExecuteAsync(sql);
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