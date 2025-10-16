using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;
using MySqlConnector;
using Xunit;

namespace NRLObstacleReporting.StartupTests;
/// <summary>
/// Collection of tests for the database
/// </summary>
public sealed class InternalDatabaseConnectionTest : IStartupTest
{
    /*
     * IMPORTANT
     * When adding more tests to this class, make sure to use the "testprefix" for any new tests. Do this so the
     * "invokealltests" method can collect the method and invoke it. Make sure to use SuccessMessage as return
     * value if the test didn't fail, the "invokealltests" method uses it to count failed/succeeded tests.
     */
    private static IConfiguration? _config;
    private readonly MySqlConnection _internalConnection = new MySqlConnection(_config!.GetConnectionString("InternalConnection"));
    
    /// <summary>
    /// Used to mark a test as a method to run
    /// </summary>
    private const string TestPrefix = "Check";
    
    /// <summary>
    /// Success return value of each test. Checked against to represent result as bool
    /// </summary>
    private const string SuccessMessage = "Success";

    //Instantiates class as singeton, to allow for interface implementation and non-static methods
    private static readonly Lazy<InternalDatabaseConnectionTest> Instance = new Lazy<InternalDatabaseConnectionTest>(()
        => new InternalDatabaseConnectionTest());
    
    private InternalDatabaseConnectionTest()
    {
    }
    
    /// <summary>
    /// Allows to pass configuration to the class, and run its testing methods.
    /// </summary>
    /// <param name="configuration">Configuration which the class gets the connection string to run tests with</param>
    /// <returns>reference to singleton instance</returns>
    public static InternalDatabaseConnectionTest GetInstance(IConfiguration configuration)
    {
        _config = configuration;
        return Instance.Value;
    }
    
    /// <summary>
    /// Runs all internal database connection tests in the class.
    /// </summary>
    public void InvokeAllTests()
    {
        //contains result of each test as bool, used to count fails/successes
        List<bool> testResults = new List<bool>();
        
        Console.WriteLine("|-------------------------------------------");
        Console.WriteLine("| Running all internal connection tests:");
        
        //collects all private instanced methods with testprefix into array
        MethodInfo[] databaseConnectionTests = GetType()
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(test => test.Name.StartsWith(TestPrefix))
            .ToArray();
        
        for (int i = 0; i < databaseConnectionTests.Count(); i++)
        {
            Console.WriteLine("|-------------------------------------------");
            
            //Formatting. Finds uppercase letters and adds a space.
            string testName = Regex.Replace(databaseConnectionTests[i].Name, @"([a-z])([A-Z])", "$1 $2");
            //Runs a given test
            //Formatting. Asserts in xunit spit out multiline strings, adds "|" at start of each new line.
            string testResult = databaseConnectionTests[i].Invoke(this, new object[0])!.ToString()!.ReplaceLineEndings("\n| ");
            
            //console result output
            Console.WriteLine($"| {i + 1}. {testName}: {testResult}");
            
            switch (testResult)
            {
                case SuccessMessage:
                    testResults.Add(true);
                    break;
                default:
                    testResults.Add(false);
                    break;
            }
            
            if (testResults.Count() == databaseConnectionTests.Count())
            {
                int failedTests = testResults.Count(fail => fail == false);
                int successfulTests = testResults.Count(success => success);
                
                Console.WriteLine("|-------------------------------------------");
                Console.WriteLine($"| Tests succeeded: {successfulTests}");
                Console.WriteLine($"| Tests failed: {failedTests}");
                Console.WriteLine("|-------------------------------------------");
            }
        }
    }
    
    //test methods
    [UsedImplicitly]
    private string CheckConnectionOpens()
    {
        try
        {
            _internalConnection.Open();
            return SuccessMessage;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
    
    [UsedImplicitly]
    private string CheckConnectionState()
    {
        try
        {
            Assert.True(_internalConnection.State == ConnectionState.Open);
            return SuccessMessage;
        }
        catch (Exception e)
        {
            return $"{e.Message}";
        }
    }
    
    [UsedImplicitly]
    private string CheckConnectionDatasource()
    {
        try
        {
            Assert.True(_internalConnection.DataSource == "db");
            return SuccessMessage;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
    
    [UsedImplicitly]
    private string CheckConnectionDatabase()
    {
        try
        {
            Assert.True(_internalConnection.Database == "nrl");
            return SuccessMessage;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
    
    [UsedImplicitly]
    private string CheckConnectionPing()
    {
        try
        {
            Assert.True(_internalConnection.Ping());
            return SuccessMessage;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
    
    [UsedImplicitly]
    private string CheckConnectionClose()
    {
        try
        {
            _internalConnection.Close();
            return SuccessMessage;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}