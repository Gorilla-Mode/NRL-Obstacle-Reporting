using Microsoft.EntityFrameworkCore;

namespace NRLObstacleReporting.Database;

public class DatabaseContext : DbContext
{
     public DbSet<ObstacleDatamodel> ObstacleDatamodels { get; set; } = null!;
     public DbSet<PilotDatamodel> PilotDatamodels { get; set; } = null!;
     public DbSet<RegistrarDatamodel> RegistrarDatamodels { get; set; } = null!;
     public DbSet<ObstacleTypeDatamodel> ObstacleTypeDatamodels { get; set; } = null!;
     public DbSet<ReportDatamodel> ReportDatamodels { get; set; } = null!;
     public DbSet<StatusDatamodel> StatusDatamodels { get; set; } = null!;>
     //public DbSet<TableClass>  TableClass { get; set; } = null!;
     public DatabaseContext()
     {
     }
     
     public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
     {
          
     }
     
     protected override void OnModelCreating(ModelBuilder modelBuilder)
     {
          modelBuilder.Entity<ObstacleDatamodel>(entity =>
          {
               entity.HasKey(e => e.ObstacleId);
               entity.Property(e => e.IsDraft);
               entity.Property(e => e.ObstacleName);
               entity.Property(e => e.ObstacleDescription);
               entity.Property(e => e.ObstacleIlluminated);
               entity.Property(e => e.GeometryGeoJson);
               entity.Property(e => e.ObstacleHeightMeter);
               
               // FK til ObstacleType
               entity.HasOne(o => o.ObstacleType)                // navigasjonsproperty
                    .WithMany()
                    .HasForeignKey(o => o.ObstacleTypeId);      // FK-feltet (INT)
               entity.HasIndex(o => o.ObstacleTypeId);
          });

          modelBuilder.Entity<PilotDatamodel>(entity =>
          {
               entity.HasKey(e => e.PilotId);
               entity.Property(e => e.Name);
               entity.Property(e => e.Organization);
          });
          
          modelBuilder.Entity<RegistrarDatamodel>(entity =>
          {
               entity.HasKey(e => e.RegistrarId);
               entity.Property(e => e.Name);
               entity.Property(e => e.Organization);
               entity.Property(e => e.Role);
          });
          
          modelBuilder.Entity<ObstacleTypeDatamodel>(entity =>
          {
               entity.HasKey(e => e.ObstacleTypeId);
               entity.Property(e => e.ObstacleName);
          });
          
          modelBuilder.Entity<ReportDatamodel>(entity =>
          {
               entity.HasKey(e => e.ReportId);
               entity.Property(e => e.Dato).HasColumnType("date");
               entity.Property(e => e.Status);
               
               entity.HasOne(e => e.Pilot)
                    .WithMany()              
                    .HasForeignKey(e => e.PilotId)
                    .OnDelete(DeleteBehavior.Restrict); 
               entity.HasIndex(e => e.PilotId);
               
               entity.HasOne(e => e.Status)
                    .WithMany(s => s.Reports)
                    .HasForeignKey(e => e.StatusId)
                    .OnDelete(DeleteBehavior.Restrict);
               entity.HasIndex(e => e.StatusId);
          });

          modelBuilder.Entity<StatusDatamodel>(entity =>
          {
               entity.HasKey(e => e.StatusId);
               entity.Property(e => e.StatusName)
          });
          
          base.OnModelCreating(modelBuilder);
     }
}

