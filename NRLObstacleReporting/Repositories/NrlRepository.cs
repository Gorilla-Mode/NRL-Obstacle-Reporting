using Dapper;
using MySqlConnector;
using System.Data;
using NRLObstacleReporting.Database;

namespace NRLObstacleReporting.Repositories
{
    public class RepositoryBase
    {
        protected string ConnectionString { get; }

        public RepositoryBase()
        {
            ConnectionString = Environment.GetEnvironmentVariable("INTERNALCONNECTION")!;
        }

        protected IDbConnection CreateConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
    }

    public class ObstacleRepository : RepositoryBase, INrlRepository
    {
        public async Task InsertObstacleData(ObstacleDto data)
        {
            using var connection = CreateConnection();
            var sql = "INSERT INTO Obstacle (ObstacleID, Heightmeter) VALUES (@ObstacleId, @ObstacleHeightMeter)"; 
            await connection.ExecuteAsync(sql, data);
        }

        public Task<IEnumerable<ObstacleDto>> GetAllObstacleData()
        {
            throw new NotImplementedException(); //Not implemented yet
        }
    }
}