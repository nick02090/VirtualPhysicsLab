using Microsoft.EntityFrameworkCore;
using VirtualPhysicsLab.Data.Models;

namespace VirtualPhysicsLab.Web.Data
{
    public class VPLContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Experiment> Experiments { get; set; }
        public DbSet<Mesh> Meshes { get; set; }
        public DbSet<MeshSettings> MeshSettings { get; set; }
        public DbSet<MeshSize> MeshSizes { get; set; }

        public VPLContext(DbContextOptions<VPLContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

            #region Mesh-MeshSettings
            modelBuilder.Entity<Mesh>()
                .HasOne(m => m.Settings)
                .WithOne(s => s.Mesh)
                .HasForeignKey<MeshSettings>(s => s.MeshId);
            #endregion

            #endregion
        }
    }
}
