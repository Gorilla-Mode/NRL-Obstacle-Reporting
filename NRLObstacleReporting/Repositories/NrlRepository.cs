using Dapper;
using MySqlConnector;
using System.Data;
using NRLObstacleReporting.Database;

namespace NRLObstacleReporting.Repositories
{
    public class ObstacleRepository : RepositoryBase, IObstacleRepository
    {
        public async Task Insert(ObstacleDto data)
        {
            using var connection = CreateConnection();
            var sql = "INSERT INTO Obstacle (ObstacleID, Heightmeter, Type) VALUES (@ObstacleId, @ObstacleHeightMeter, @ObstacleType)"; 
            await connection.ExecuteAsync(sql, data);
        }

        public Task<IEnumerable<ObstacleDto>> GetAllObstacleData()
        {
            throw new NotImplementedException(); //Not implemented yet
        }
    }
}