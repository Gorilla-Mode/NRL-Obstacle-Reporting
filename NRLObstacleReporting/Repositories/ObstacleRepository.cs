using Dapper;
using MySqlConnector;
using NRLObstacleReporting.Database;
using NRLObstacleReporting.Models;

namespace NRLObstacleReporting.Repositories
{
    public class ObstacleRepository : RepositoryBase, IObstacleRepository
    {
        public async Task InsertStep1(ObstacleDto data)
        {
            using var connection = CreateConnection();
            var sql = @"INSERT INTO Obstacle (ObstacleID, Heightmeter, Type, GeometryGeoJson) 
                        VALUES (@ObstacleId, @HeightMeter, @Type, @GeometryGeoJson)";
            await connection.ExecuteAsync(sql, data);
        }

        public async Task InsertStep2(ObstacleDto data)
        {
            using var connection = CreateConnection();
            var sql = @"UPDATE Obstacle 
                        SET GeometryGeoJson = @GeometryGeoJson 
                        WHERE ObstacleID = @ObstacleId";
            await connection.ExecuteAsync(sql, data);
        }

        public async Task InsertStep3(ObstacleDto data)
        {
            using var connection = CreateConnection();
            var sql = @"UPDATE Obstacle 
                        SET Name = @Name, Description = @Description, Illuminated = @Illuminated, 
                            Status = @Status, Marking = @Marking 
                        WHERE ObstacleID = @ObstacleId";
            await connection.ExecuteAsync(sql, data);
        }

        public async Task<ObstacleDto> GetObstacleById(int id)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Obstacle WHERE ObstacleID = @id";
            return await connection.QuerySingleAsync<ObstacleDto>(sql, new { Id = id });
        }

        public async Task<IEnumerable<ObstacleDto>> GetAllSubmittedObstacles()
        {
            using var connection = CreateConnection();
            var sql = @$"SELECT * FROM Obstacle 
                         WHERE Status <> {(int)ObstacleCompleteModel.ObstacleStatus.Draft}";
            return await connection.QueryAsync<ObstacleDto>(sql);
        }

        public async Task<IEnumerable<ObstacleDto>> GetReportsByStatusAsync(ObstacleCompleteModel.ObstacleStatus status)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Obstacle WHERE Status = @Status";
            return await connection.QueryAsync<ObstacleDto>(sql, new { Status = (int)status });
        }

        public async Task UpdateObstacleStatus(int obstacleId, ObstacleCompleteModel.ObstacleStatus status)
        {
            using var connection = CreateConnection();
            var sql = "UPDATE Obstacle SET Status = @Status WHERE ObstacleID = @ObstacleId";
            await connection.ExecuteAsync(sql, new { Status = (int)status, ObstacleId = obstacleId });
        }
    }
}
