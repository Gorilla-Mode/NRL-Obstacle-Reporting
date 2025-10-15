using System.Data;
using MySqlConnector;
using Xunit;

namespace NRLObstacleReporting.StartupTests;

public class DatabaseTest 
{
    private readonly IConfiguration _config;
    
    public DatabaseTest(IConfiguration config)
    {
        _config = config;
    }
    
    public bool CheckInternalConnection()
    {
        string msg = "-- Internal Connection check:";
        try
        {
            Console.WriteLine("Database checking internal connection:");
            var internalconnection = new MySqlConnection(_config.GetConnectionString("InternalConnection"));
            
            internalconnection.Open();
            Console.WriteLine($"{msg} opened successfully!");
            
            Assert.True(internalconnection.State == ConnectionState.Open);
            Console.WriteLine($"{msg} state open!");
            
            Assert.True(internalconnection.DataSource == "db");
            Console.WriteLine($"{msg} datasource correct!");
            
            Assert.True(internalconnection.Database == "nrl");
            Console.WriteLine($"{msg} database correct!");
            
            Assert.True(internalconnection.Ping());
            Console.WriteLine($"{msg} pinged successfully!");
            
            internalconnection.Close();
            Console.WriteLine($"{msg} connection closed successfully");
            Console.WriteLine("Internal connection test succeeded!");
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Internal connection test failed: {e.Message}");
            return false;
        }
    }
}