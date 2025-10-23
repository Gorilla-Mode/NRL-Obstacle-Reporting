using Dapper;
using MySqlConnector;
using System.Data;
using NRLObstacleReporting.Database;

namespace NRLObstacleReporting.Repositories
{
    public class ObstacleRepository : RepositoryBase, IObstacleRepository
    {
        public async Task InsertStep1(ObstacleDto data)
        {
            using var connection = CreateConnection();
            var sql = @"INSERT INTO Obstacle (ObstacleID, Heightmeter, Type) 
                        VALUES (@ObstacleId, @ObstacleHeightMeter, @ObstacleType)"; 
            await connection.ExecuteAsync(sql, data);
        }

        public async Task InsertStep2(ObstacleDto data)
        {
            using var connection = CreateConnection();
            var sql = @"UPDATE Obstacle 
                        SET GeometryGeoJson = @GeometryGeoJson 
                        WHERE ObstacleID = @ObstacleID";
            await connection.ExecuteAsync(sql, data);
        }

        public async Task InsertStep3(ObstacleDto data)
        {
            using var connection = CreateConnection();
            var sql = @"UPDATE Obstacle 
                        SET Name = @ObstacleName, Description = @ObstacleDescription, Illuminated = @ObstacleIlluminated 
                        WHERE ObstacleID = @ObstacleID";
            await connection.ExecuteAsync(sql, data);

        }

        public Task<IEnumerable<ObstacleDto>> GetAllObstacleData()
        {
            throw new NotImplementedException(); //Not implemented yet
        }
    }
}