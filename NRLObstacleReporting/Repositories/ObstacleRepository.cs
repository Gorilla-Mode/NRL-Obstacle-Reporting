using Dapper;
using MySqlConnector;
using System.Data;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.Repositories
{
    public class ObstacleRepository : RepositoryBase, IObstacleRepository
    {
        public async Task InsertStep1(ObstacleDto data)
        {
            using var connection = CreateConnection();
            var sql = @"INSERT INTO Obstacle (ObstacleID, Heightmeter, Type, GeometryGeoJson, CreationTime, UpdatedTime, UserId) 
                        VALUES (@ObstacleId, @HeightMeter, @Type, @GeometryGeoJson,  @CreationTime, @UpdatedTime, @UserId)"; 
            await connection.ExecuteAsync(sql, data);
        }

        public async Task InsertStep2(ObstacleDto data)
        {
            using var connection = CreateConnection();
            var sql = @"UPDATE Obstacle 
                        SET GeometryGeoJson = @GeometryGeoJson, UpdatedTime = @UpdatedTime 
                        WHERE ObstacleID = @ObstacleId";
            await connection.ExecuteAsync(sql, data);
        }

        public async Task InsertStep3(ObstacleDto data)
        {
            using var connection = CreateConnection();
            var sql = @"UPDATE Obstacle 
                        SET Name = @Name, Description = @Description, Illuminated = @Illuminated, Status = @Status, 
                            Marking = @Marking, UpdatedTime = @UpdatedTime 
                        WHERE ObstacleID = @ObstacleId";
            await connection.ExecuteAsync(sql, data);

        }

        public async Task<ObstacleDto> GetObstacleById(string? id)
        {
            using var connection = CreateConnection();
            connection.Open();
            var sql = "SELECT * FROM Obstacle WHERE ObstacleID = @id";

            return await connection.QuerySingleAsync<ObstacleDto>(sql, new { Id = id });
        }

        public async Task<IEnumerable<ObstacleDto>> GetAllSubmittedObstacles()
        {
            //Needs to be updated to only get form ceatain users when IdentityCore is implemented
            using var connection = CreateConnection();
            var sql = @$"SELECT * 
                         FROM Obstacle 
                         WHERE Status <> {(int)ObstacleCompleteModel.ObstacleStatus.Draft}";
            await connection.ExecuteAsync(sql);
            
            return connection.Query<ObstacleDto>(sql);
        }
        
    }
}