using System.Data;
using MySqlConnector;
using Xunit;

namespace NRLObstacleReporting.StartupTests;
/// <summary>
/// Collection of tests for the database
/// <param name="config">used to get connection string from appsettings</param>
/// </summary>
public class DatabaseTest(IConfiguration config)
{
    /// <summary>
    /// Checks if the program is able to connect to database in internal context
    /// </summary>
    /// <returns>True if all test succeded</returns>
    public bool CheckInternalConnection()
    {
        string msg = "-- Internal Connection check:";
        try
        {
            Console.WriteLine("Database checking internal connection:");
            var internalconnection = new MySqlConnection(config.GetConnectionString("InternalConnection"));
            
            //checks if connection opens
            internalconnection.Open();
            Console.WriteLine($"{msg} opened successfully!");
            
            //checks that connection remains open
            Assert.True(internalconnection.State == ConnectionState.Open);
            Console.WriteLine($"{msg} state open!");
            
            //checks that data source is correct
            Assert.True(internalconnection.DataSource == "db");
            Console.WriteLine($"{msg} datasource correct!");
            
            //checks that database is correct
            Assert.True(internalconnection.Database == "nrl");
            Console.WriteLine($"{msg} database correct!");
            
            //checks that database can be pinged
            Assert.True(internalconnection.Ping());
            Console.WriteLine($"{msg} pinged successfully!");
            
            //closes connection
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