namespace NRLObstacleReporting.StartupTests;
/// <summary>
/// To be implemented by startup database test. Used to run tests from subclasses polymorphically.
/// </summary>
public interface IStartupDatabaseTest
{
    //method should run all test methods in a class
    public void InvokeAllTests();
}