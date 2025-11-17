using Dapper;
using MySqlConnector;
using System.Data;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.Repositories;

public class RegistrarRepository : RepositoryBase, IRegistrarRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<ObstacleDto>> GetAllSubmittedObstacles()
    {
        
        using var connection = CreateConnection();
        var sql = @$"SELECT * 
                     FROM Obstacle 
                     WHERE Status <> {(int)ObstacleCompleteModel.ObstacleStatus.Draft}";
        
        return await connection.QueryAsync<ObstacleDto>(sql);
    }

    /// <inheritdoc/>
    public async Task<ObstacleDto> GetSubmittedObstacleById(string id)
    {
        using var connection = CreateConnection();
        var sql = $@"SELECT * 
                    FROM Obstacle
                    WHERE ObstacleID = @id AND Status <> {(int)ObstacleCompleteModel.ObstacleStatus.Draft}";

        return await connection.QuerySingleAsync<ObstacleDto>(sql, new { Id = id });
    }

    /// <inheritdoc/>
    public async Task UpdateObstacleStatus(ObstacleDto data)
    {
        using var connection = CreateConnection();
        var sql = @"UPDATE Obstacle 
                    SET Status = @Status
                    WHERE ObstacleID = @ObstacleId";
        await connection.ExecuteAsync(sql, data);
    }
}
