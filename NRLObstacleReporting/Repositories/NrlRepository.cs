using Dapper;
using MySqlConnector;
using System.Data;
using NRLObstacleReporting.Database;

namespace NRLObstacleReporting.Repositories
{
    public class RepositoryBase
    {
        protected string ConnectionString { get; }

        public RepositoryBase(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected IDbConnection CreateConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
    }

    public class ObtacleRepository : RepositoryBase, INrlRepository
    {
        public ObtacleRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task InsertObstacleData(ObstacleCompleteModel data)
        {
            using var connection = CreateConnection();
            var sql = "INSERT INTO ..."; //Not implemented yet
            await connection.ExecuteAsync(sql, data);
        }

        public Task<IEnumerable<ObstacleCompleteModel>> GetAllObstacleData()
        {
            throw new NotImplementedException(); //Not implemented yet
        }
    }
}