using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;
using Dapper;
using JetBrains.Annotations;
using MySqlConnector;
using NRLObstacleReporting.Repositories;
using Xunit;

namespace NRLObstacleReporting.StartupTests;

/// <summary>
/// Collection of tests for the database, testing internal reading and writing to the database
/// </summary>
public class InternalDatabaseReadWriteTest : RepositoryBase, IStartupDatabaseTest

{
    /*
     * IMPORTANT
     * When adding more tests to this class, make sure to use the "testprefix" for any new tests. Do this so the
     * "invokealltests" method can collect the method and invoke it. Make sure to use SuccessMessage as return
     * value if the test didn't fail, the "invokealltests" method uses it to count failed/succeeded tests.
     */
    
    
    /// <summary>
    /// Used to mark a test as a method to run
    /// </summary>
    private const string TestPrefix = "Check";
    
    /// <summary>
    /// Success return value of each test. Checked against to represent result as bool
    /// </summary>
    private const string SuccessMessage = "Success";

    //Instantiates class as singeton, to allow for interface implementation and non-static methods
    private static readonly Lazy<InternalDatabaseReadWriteTest> Instance = new Lazy<InternalDatabaseReadWriteTest>(()
        => new InternalDatabaseReadWriteTest());
    
    private InternalDatabaseReadWriteTest()
    {
    }
    
    /// <summary>
    /// Allows to pass configuration to the class, and run its testing methods.
    /// </summary>
    /// <returns>reference to singleton instance</returns>
    public static InternalDatabaseReadWriteTest GetInstance()
    {
        return Instance.Value;
    }
    
    /// <summary>
    /// Runs all internal database read/write tests in the class.
    /// </summary>
    public void InvokeAllTests()
    {
        //contains result of each test as bool, used to count fails/successes
        List<bool> testResults = new List<bool>();
        
        Console.WriteLine("|-------------------------------------------");
        Console.WriteLine("| Running all read/write tests:");
        
        //collects all private instanced methods with testprefix into array
        MethodInfo[] databaseReadWriteTests = GetType()
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(test => test.Name.StartsWith(TestPrefix))
            .ToArray();
        
        for (int i = 0; i < databaseReadWriteTests.Count(); i++)
        {
            Console.WriteLine("|-------------------------------------------");
            
            //Formatting. Finds uppercase letters and adds a space.
            string testName = Regex.Replace(databaseReadWriteTests[i].Name, @"([a-z])([A-Z])", "$1 $2");
            //Runs a given test
            //Formatting. Asserts in xunit spit out multiline strings, adds "|" at start of each new line.
            string testResult = databaseReadWriteTests[i].Invoke(this, new object[0])!.ToString()!.ReplaceLineEndings("\n| ");
            
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
            
            if (testResults.Count() == databaseReadWriteTests.Count())
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
    
    //TODO: Make these tests actually do something
    [UsedImplicitly]
    string CheckDatabaseWrite()
    {
        try
        {
            using var connection = CreateConnection();
            var sql = "INSERT INTO test (test_column) VALUES (1)"; 
            connection.Execute(sql);
            
            return SuccessMessage;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
    
    [UsedImplicitly]
    string CheckDatabaseRead()
    {
        try
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM test WHERE test_column = 1";
            var readResult = connection.QuerySingle<(int id, string name)>(sql, new { Id = (int?)null, Name = "test" });
            Assert.Equal(1, readResult.id);
            
            return SuccessMessage;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
    
    [UsedImplicitly]
    string CheckDatabaseTestTableDelete()
    {
        try
        {   
            using var connection = CreateConnection();
            var sql = "DELETE FROM test WHERE test_column = 1"; 
            connection.Execute(sql);
            
            return SuccessMessage;
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}