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
            var sql = @"INSERT INTO Obstacle (ObstacleID, Heightmeter, Type) 
                        VALUES (@ObstacleId, @HeightMeter, @Type)"; 
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
                        SET Name = @Name, Description = @Description, Illuminated = @Illuminated 
                        WHERE ObstacleID = @ObstacleId";
            await connection.ExecuteAsync(sql, data);

        }

        public async Task<ObstacleDto> GetObstacleById(int id)
        {
            using var connection = CreateConnection();
            connection.Open();
            var sql = "SELECT * FROM Obstacle WHERE ObstacleID = @id";
            var parameters = new { Id = id };

            return await connection.QuerySingleAsync<ObstacleDto>(sql, parameters);
        }

        public Task<IEnumerable<ObstacleDto>> GetAllObstacleData()
        {
            throw new NotImplementedException(); //Not implemented yet
        }
    }
}