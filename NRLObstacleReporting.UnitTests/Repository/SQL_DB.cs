using System;
using System.IO;

namespace NRLObstacleReporting.UnitTests.Repository
{
    public static class SQL_DB
    {
        /// <summary>
        /// Reads the contents of db.sql and returns it as a single string.
        /// </summary>
        /// <param name="overridePath">Optional absolute or relative path to db.sql.
        /// If omitted, tries to locate db.sql in the solution root or current directory.</param>
        public static string ReadDbSql(string? overridePath = null)
        {
            // 1) Explicit override path
            if (!string.IsNullOrWhiteSpace(overridePath))
            {
                var explicitFullPath = Path.GetFullPath(overridePath);
                if (!File.Exists(explicitFullPath))
                    throw new FileNotFoundException($"db.sql not found at path: {explicitFullPath}");
                return File.ReadAllText(explicitFullPath);
            }

            // 2) Prefer the test output-local copy (from CopyToOutputDirectory)
            var binLocal = Path.Combine(AppContext.BaseDirectory, "db.sql");

            var candidates = new []
            {
                binLocal,
                Path.Combine(Environment.CurrentDirectory, "db.sql"),
                Path.Combine(Environment.CurrentDirectory, @"..\db.sql"),
                Path.Combine(Environment.CurrentDirectory, @"..\..\db.sql"),
                Path.Combine(Environment.CurrentDirectory, @"..\..\..\db.sql"),
            };
            
            foreach (var candidate in candidates)
            {
                var full = Path.GetFullPath(candidate);
                if (File.Exists(full))
                {
                    return File.ReadAllText(full);
                }
            }
            throw new FileNotFoundException("db.sql could not be located. Provide an explicit path or ensure db.sql is copied to the test output.");
        }
    }
}