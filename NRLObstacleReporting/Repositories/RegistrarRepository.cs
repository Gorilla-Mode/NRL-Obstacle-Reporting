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

    /// <inheritdoc/>
    public async Task<IList<ObstacleDto>> GetObstaclesByStatus(ObstacleCompleteModel.ObstacleStatus[] status)
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

    public async Task<IList<ObstacleDto>> GetObstaclesFiltered(ObstacleCompleteModel.ObstacleStatus[] status, ObstacleCompleteModel.ObstacleTypes[] type, ObstacleCompleteModel.Illumination[] illuminations,
        ObstacleCompleteModel.ObstacleMarking[] markings)
    {
        using var connection = CreateConnection();
        
        string sql = $@"SELECT * 
                        FROM Obstacle 
                        WHERE 1=1
                        AND Status <> {(int)ObstacleCompleteModel.ObstacleStatus.Draft}"; //base sql returns whole table
        
        if (status.Length != 0)
        {
            string statusList = string.Join(", ", status.Select(s => (int)s));

            sql += $" AND Status IN ({statusList})";
        }

        if (type.Length != 0)
        {
            string typeList = string.Join(", ", type.Select(t => (int)t));
            
            sql += $" AND Type IN ({typeList})";
        }
        
        if (illuminations.Length != 0)
        {
            string illuminationList = string.Join(", ", illuminations.Select(m => (int)m));
            
            sql += $" AND Illuminated IN ({illuminationList})";
        }
        
        if (markings.Length != 0)
        {
            string markingList = string.Join(", ", markings.Select(i => (int)i));
            
            sql += $" AND Marking IN ({markingList})";
        }
        
        var queryResult = await connection.QueryAsync<ObstacleDto>(sql);
        return queryResult.ToList();
    }
}
