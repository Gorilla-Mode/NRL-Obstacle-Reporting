using System.Collections;
using System.Collections.Immutable;
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
    public async Task<ViewObstacleUserDto> GetSubmittedObstacleById(string id)
    {
        using var connection = CreateConnection();
        var sql = $@"SELECT * 
                    FROM view_ObstacleUser
                    WHERE ObstacleID = @id AND Status <> {(int)ObstacleCompleteModel.ObstacleStatus.Draft}";

        return await connection.QuerySingleAsync<ViewObstacleUserDto>(sql, new { Id = id });
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

    public async Task<IList<ObstacleDto>> GetObstaclesByStatus(params ObstacleCompleteModel.ObstacleStatus[] status)
    {
        using var connection = CreateConnection();
        var queryResult = new List<ObstacleDto>();
        
        foreach (var query in status)
        {
            var sql = $@"SELECT *
                    FROM Obstacle
                    WHERE Status = {(int)query}";
            var filteredResult = await connection.QueryAsync<ObstacleDto>(sql);

            foreach (var obstacle in filteredResult)
            {
                if (queryResult != null)
                {
                    queryResult.Add(obstacle);
                }
            }
        }
        
        return queryResult ?? throw new InvalidOperationException();
    }
}
