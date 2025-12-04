using System.Data;
using System.Threading.Tasks;
using Dapper;
using NRLObstacleReporting.Database;

namespace NRLObstacleReporting.UnitTests.Repository;

public class HelperQueries
{
    /// <summary>
    /// Retrieves an obstacle record from the specified database table by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the obstacle to retrieve.</param>
    /// <param name="tableName">The name of the table where the obstacle data is stored.</param>
    /// <param name="connection">The database connection used to query the obstacle data.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="ObstacleDto"/> record corresponding to the specified ID.</returns>
    protected static async Task<ObstacleDto> FakeGetObstacleById(string id, string tableName, IDbConnection connection)
    {
        var sql = $@"SELECT * 
                    FROM {tableName} 
                    WHERE ObstacleID = {id}";

        return await connection.QuerySingleAsync<ObstacleDto>(sql);
    }
    
    protected static async Task InsertObstacleHelper(ObstacleDto obstacle, IDbConnection connection, string tableName)
    {
        var sql = 
            $@"INSERT INTO {tableName} 
                          (ObstacleID, Heightmeter, GeometryGeoJson, Name, Description, Illuminated,
                           Type, Status, Marking, UserId)
                    VALUES 
                        ('{obstacle.ObstacleId}', {obstacle.HeightMeter}, '{obstacle.GeometryGeoJson}', '{obstacle.Name}',
                         '{obstacle.Description}', {obstacle.Illuminated}, {obstacle.Type}, {obstacle.Status},
                         {obstacle.Marking}, '{obstacle.UserId}');";
            
        await connection.ExecuteAsync(sql);
    }

    protected static async Task CreateObstacleTable(IDbConnection connection, string tableName)
    {
        var createTableSql = $@"
            CREATE TABLE {tableName}
            (
                ObstacleID     TEXT    NOT NULL PRIMARY KEY,
                UserId         TEXT    NOT NULL,
                HeightMeter    INTEGER NOT NULL,
                GeometryGeoJson TEXT   NULL,
                Type           INTEGER NOT NULL,
                Status         INTEGER NOT NULL DEFAULT 0,
                Marking        INTEGER NOT NULL DEFAULT 0,
                Name           TEXT    NULL,
                Description    TEXT    NULL,
                Illuminated    INTEGER NOT NULL DEFAULT 0,
                CreationTime   TEXT    NULL,
                UpdatedTime    TEXT    NULL
            );";
        await connection.ExecuteAsync(createTableSql);
    }
}