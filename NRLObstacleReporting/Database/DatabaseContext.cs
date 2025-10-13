using Microsoft.EntityFrameworkCore;

namespace NRLObstacleReporting.Database;

public class DatabaseContext : DbContext
{
     public DbSet<Obstacle> Obstacle { get; set; } = null!;
     public DbSet<Pilot> Pilot { get; set; } = null!;
     public DbSet<Registrar> Registrar { get; set; } = null!;
     //public DbSet<TableClass>  TableClass { get; set; } = null!;
     public DatabaseContext()
     {
     }
     
     public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
     {
          
     }
     
     protected override void OnModelCreating(ModelBuilder modelBuilder)
     {
          modelBuilder.Entity<Obstacle>(entity =>
          {
               entity.HasKey(e => e.ObstacleId);
               entity.HasKey(e => e.IsDraft);
               entity.Property(e => e.ObstacleType);
               entity.Property(e => e.ObstacleName);
               entity.Property(e => e.ObstacleDescription);
               entity.Property(e => e.ObstacleIlluminated);
               entity.Property(e => e.GeometryGeoJson);
               entity.Property(e => e.ObstacleHeightMeter);
          });

          modelBuilder.Entity<Pilot>(entity =>
          {
               entity.HasKey(e => e.PilotId);
               entity.Property(e => e.Name);
               entity.Property(e => e.Organization);
          });
          
          modelBuilder.Entity<Registrar>(entity =>
          {
               entity.HasKey(e => e.RegistrarId);
               entity.Property(e => e.Name);
               entity.Property(e => e.Organization);
               entity.Property(e => e.Role);
          });
          
          base.OnModelCreating(modelBuilder);
          
     }
     
}

