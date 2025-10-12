using Microsoft.EntityFrameworkCore;

namespace NRLObstacleReporting.Database;

public class DatabaseContext : DbContext
{
     public DbSet<ObstacleCompleteModel> ObstacleCompleteModel { get; set; }

     public DatabaseContext()
     {
     }
     
     public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
     {
          
     }
     
     
}

