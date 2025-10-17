using Microsoft.EntityFrameworkCore;

namespace NRLObstacleReporting.Database;

public class DatabaseContext : DbContext
{
     public DbSet<ObstacleCompleteModel> ObstacleCompleteModel { get; set; } = null!;
     //public DbSet<TableClass>  TableClass { get; set; } = null!;
     public DatabaseContext()
     {
     }
     
     public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
     {
          
     }
     
     protected override void OnModelCreating(ModelBuilder modelBuilder)
     {
          modelBuilder.Entity<ObstacleCompleteModel>(entity =>
          {
               entity.HasKey(e => e.ObstacleId);
               entity.HasKey(e => e.IsDraft);
               entity.Property(e => e.ObstacleName);
               entity.Property(e => e.ObstacleDescription);
               entity.Property(e => e.ObstacleIlluminated);
               entity.Property(e => e.GeometryGeoJson);
               entity.Property(e => e.ObstacleHeightMeter);

          });
          base.OnModelCreating(modelBuilder);
     }
     
}

