using System.Data;
using MySqlConnector;

namespace NRLObstacleReporting.Repositories;
/// <summary>
/// Superclass for any repositories to inherit base methods from
/// </summary>
public abstract class RepositoryBase
{
    private string ConnectionString { get; }

    protected RepositoryBase()
    {
        ConnectionString = Environment.GetEnvironmentVariable("INTERNALCONNECTION")!;
    }

    /// <summary>
    /// Establishes a connection to database
    /// </summary>
    /// <returns>MariaDB connection</returns>
    protected IDbConnection CreateConnection()
    {
        return new MySqlConnection(ConnectionString);
    }
}