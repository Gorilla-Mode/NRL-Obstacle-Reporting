using Microsoft.EntityFrameworkCore;

namespace NRLObstacleReporting.Database;

public class DatabaseContext : DbContext
{
     public DbSet<ObstacleDto> ObstacleData { get; set; } = null!;
     //public DbSet<TableClass>  TableClass { get; set; } = null!;
     public DatabaseContext()
     {
     }
     
     public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
     {
          
     }
     }


