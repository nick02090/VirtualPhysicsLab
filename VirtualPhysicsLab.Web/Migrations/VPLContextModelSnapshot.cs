﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VirtualPhysicsLab.Web.Data;

namespace VirtualPhysicsLab.Web.Migrations
{
    [DbContext(typeof(VPLContext))]
    partial class VPLContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VirtualPhysicsLab.Data.Models.Experiment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CreatedById");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("LogicalName")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("Experiments");
                });

            modelBuilder.Entity("VirtualPhysicsLab.Data.Models.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("LogicalName")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("VirtualPhysicsLab.Data.Models.Mesh", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<Guid>("ExperimentId");

                    b.Property<string>("LogicalName")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ExperimentId");

                    b.ToTable("Meshes");
                });

            modelBuilder.Entity("VirtualPhysicsLab.Data.Models.MeshSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("HexColor");

                    b.Property<string>("LogicalName")
                        .IsRequired();

                    b.Property<Guid>("MeshId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("MeshId")
                        .IsUnique();

                    b.ToTable("MeshSettings");
                });

            modelBuilder.Entity("VirtualPhysicsLab.Data.Models.MeshSize", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("LogicalName")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<Guid>("SettingsId");

                    b.Property<decimal>("Value");

                    b.HasKey("Id");

                    b.HasIndex("SettingsId");

                    b.ToTable("MeshSizes");
                });

            modelBuilder.Entity("VirtualPhysicsLab.Data.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("LogicalName")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("NickName")
                        .IsRequired();

                    b.Property<int>("Occupation");

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("Token");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("VirtualPhysicsLab.Data.Models.UserGroup", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("GroupId");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<Guid>("Id");

                    b.Property<string>("LogicalName")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("UserId", "GroupId");

                    b.HasAlternateKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("UserGroups");
                });

            modelBuilder.Entity("VirtualPhysicsLab.Data.Models.Experiment", b =>
                {
                    b.HasOne("VirtualPhysicsLab.Data.Models.User", "CreatedBy")
                        .WithMany("Experiments")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VirtualPhysicsLab.Data.Models.Mesh", b =>
                {
                    b.HasOne("VirtualPhysicsLab.Data.Models.Experiment", "Experiment")
                        .WithMany("Meshes")
                        .HasForeignKey("ExperimentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VirtualPhysicsLab.Data.Models.MeshSettings", b =>
                {
                    b.HasOne("VirtualPhysicsLab.Data.Models.Mesh", "Mesh")
                        .WithOne("Settings")
                        .HasForeignKey("VirtualPhysicsLab.Data.Models.MeshSettings", "MeshId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VirtualPhysicsLab.Data.Models.MeshSize", b =>
                {
                    b.HasOne("VirtualPhysicsLab.Data.Models.MeshSettings", "Settings")
                        .WithMany("Sizes")
                        .HasForeignKey("SettingsId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VirtualPhysicsLab.Data.Models.UserGroup", b =>
                {
                    b.HasOne("VirtualPhysicsLab.Data.Models.Group", "Group")
                        .WithMany("Users")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("VirtualPhysicsLab.Data.Models.User", "User")
                        .WithMany("Groups")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
