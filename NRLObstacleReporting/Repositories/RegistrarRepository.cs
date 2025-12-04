using System.Data;
using Dapper;
using System.Globalization;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.Repositories;

public sealed class RegistrarRepository : RepositoryBase, IRegistrarRepository
{
    private readonly IDbConnection _connection;

    public RegistrarRepository()
    {
        _connection = CreateConnection();
    }

    public RegistrarRepository(IDbConnection mockconnection)
    {
        _connection = mockconnection;
    }


    /// <inheritdoc/>
    public async Task<IEnumerable<ObstacleDto>> GetAllSubmittedObstaclesAsync()
    {
        var sql = @$"SELECT * 
                     FROM Obstacle 
                     WHERE Status <> {(int)ObstacleCompleteModel.ObstacleStatus.Draft}";

        return await _connection.QueryAsync<ObstacleDto>(sql);
    }

    /// <inheritdoc/>
    public async Task<ViewObstacleUserDto> GetSubmittedObstacleByIdAsync(string id)
    {
        var sql = $@"SELECT * 
                    FROM view_ObstacleUser
                    WHERE ObstacleID = @id AND Status <> {(int)ObstacleCompleteModel.ObstacleStatus.Draft}";

        return await _connection.QuerySingleAsync<ViewObstacleUserDto>(sql, new { Id = id });
    }

    /// <inheritdoc/>
    public async Task UpdateObstacleStatusAsync(ObstacleDto data)
    {
        var sql = @"UPDATE Obstacle 
                    SET Status = @Status
                    WHERE ObstacleID = @ObstacleId";
        await _connection.ExecuteAsync(sql, data);
    }

    /// <inheritdoc/>
    public async Task<IList<ObstacleDto>> GetObstaclesByStatusAsync(ObstacleCompleteModel.ObstacleStatus[] status)
    {
        string statusList = string.Join(", ", status.Select(s => (int)s));

        var sql = $@"SELECT *
                FROM Obstacle
                WHERE Status IN ({statusList})";

        var queryResult = await _connection.QueryAsync<ObstacleDto>(sql);

        return queryResult.ToList();
    }

    /// <inheritdoc/>
    public async Task<IList<ObstacleDto>> GetObstaclesFilteredAsync(ObstacleCompleteModel.ObstacleStatus[] status,
        ObstacleCompleteModel.ObstacleTypes[] type, ObstacleCompleteModel.Illumination[] illuminations,
        ObstacleCompleteModel.ObstacleMarking[] markings, DateOnly dateStart, DateOnly dateEnd)
    {
        //TODO: Parameterize the variables
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

        if (dateStart.ToString() != String.Empty && dateEnd.ToString() != String.Empty)
        {
            string formattedDateStart = dateStart.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string formattedDateEnd = dateEnd.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            sql += $" AND CreationTime BETWEEN '{formattedDateStart}' AND '{formattedDateEnd}'";
        }

        var queryResult = await _connection.QueryAsync<ObstacleDto>(sql);

        return queryResult.ToList();
    }
}