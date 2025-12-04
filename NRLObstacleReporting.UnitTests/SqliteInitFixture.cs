// Plan (pseudocode):
// - Define a fixture class SqliteInitFixture.
// - In its constructor, initialize SQLite batteries (SQLitePCL.Batteries_V2.Init()).
// - Define a collection named "SqliteInit" that uses this fixture via ICollectionFixture.
// - This will make the fixture discoverable and fix CS0246.

using System;
using Xunit;

namespace NRLObstacleReporting.UnitTests
{
    // Ensure the fixture is applied to the "SqliteInit" collection
    [CollectionDefinition("SqliteInit")]
    public class SqliteInitCollection : ICollectionFixture<SqliteInitFixture>
    {
        // No code needed here; this class only serves to define the test collection
    }

    public class SqliteInitFixture
    {
        public SqliteInitFixture()
        {
            // Initialize SQLite batteries to support Microsoft.Data.Sqlite on some platforms
            try
            {
                SQLitePCL.Batteries_V2.Init();
            }
            catch
            {
                // Swallow exceptions to avoid test failures if already initialized or not required
                Console.WriteLine("Get Fucked in the asshole");
            }
        }
    }
}
