using System.Data;
using MySqlConnector;

namespace NRLObstacleReporting.Repositories;

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