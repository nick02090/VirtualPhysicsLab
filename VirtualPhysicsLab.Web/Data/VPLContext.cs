using Microsoft.EntityFrameworkCore;
using VirtualPhysicsLab.Data.Models;
using VirtualPhysicsLab.Web.Models;

namespace VirtualPhysicsLab.Web.Data
{
    public class VPLContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Experiment> Experiments { get; set; }
        public DbSet<ExperimentSettings> ExperimentSettings { get; set; }
        public DbSet<Mesh> Meshes { get; set; }
        public DbSet<MeshSettings> MeshSettings { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Rotation> Rotations { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Velocity> Velocities { get; set; }
        public DbSet<Gravity> Gravities { get; set; }
        public DbSet<SceneSize> SceneSizes { get; set; }

        public VPLContext(DbContextOptions<VPLContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasMany(u => u.Experiments).WithOne(e => e.CreatedBy).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Experiment>().HasMany(e => e.Meshes).WithOne(m => m.Experiment).OnDelete(DeleteBehavior.Cascade);

            #region MANY-MANY

            #region User-Group
            modelBuilder.Entity<UserGroup>()
                .HasKey(ug => new { ug.UserId, ug.GroupId });
            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.User)
                .WithMany(u => u.Groups)
                .HasForeignKey(ug => ug.UserId);
            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.Group)
                .WithMany(g => g.Users)
                .HasForeignKey(ug => ug.GroupId);
            #endregion

            #endregion

            #region ONE-ONE

            #region MeshSettings-Vector3
            modelBuilder.Entity<MeshSettings>()
                .HasOne(m => m.Color)
                .WithOne(c => c.MeshSettings)
                .HasForeignKey<Color>(c => c.MeshSettingsId);

            modelBuilder.Entity<MeshSettings>()
                .HasOne(m => m.Position)
                .WithOne(p => p.MeshSettings)
                .HasForeignKey<Position>(p => p.MeshSettingsId);

            modelBuilder.Entity<MeshSettings>()
                .HasOne(m => m.Rotation)
                .WithOne(r => r.MeshSettings)
                .HasForeignKey<Rotation>(r => r.MeshSettingsId);

            modelBuilder.Entity<MeshSettings>()
                 .HasOne(m => m.Size)
                 .WithOne(s => s.MeshSettings)
                 .HasForeignKey<Size>(s => s.MeshSettingsId);

            modelBuilder.Entity<MeshSettings>()
                .HasOne(m => m.Velocity)
                .WithOne(v => v.MeshSettings)
                .HasForeignKey<Velocity>(v => v.MeshSettingsId);
            #endregion

            #region ExperimentSettings-Vector3
            modelBuilder.Entity<ExperimentSettings>()
                .HasOne(e => e.Gravity)
                .WithOne(g => g.ExperimentSettings)
                .HasForeignKey<Gravity>(g => g.ExperimentSettingsId);

            modelBuilder.Entity<ExperimentSettings>()
                .HasOne(e => e.Size)
                .WithOne(s => s.ExperimentSettings)
                .HasForeignKey<SceneSize>(s => s.ExperimentSettingsId);
            #endregion

            #region Mesh-MeshSettings
            modelBuilder.Entity<Mesh>()
                .HasOne(m => m.Settings)
                .WithOne(s => s.Mesh)
                .HasForeignKey<MeshSettings>(s => s.MeshId);
            #endregion

            #region Experiment-ExperimentSettings
            modelBuilder.Entity<Experiment>()
                .HasOne(e => e.Settings)
                .WithOne(s => s.Experiment)
                .HasForeignKey<ExperimentSettings>(s => s.ExperimentId);
            #endregion

            #endregion
        }
    }
}
