﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VirtualPhysicsLab.Web.Data;

namespace VirtualPhysicsLab.Web.Migrations
{
    [DbContext(typeof(VPLContext))]
    [Migration("20190613133455_GravityAdded")]
    partial class GravityAdded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Description");

                    b.Property<string>("LogicalName");

                    b.Property<string>("Name");

                    b.Property<int>("Picture");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("Experiments");
                });

            modelBuilder.Entity("VirtualPhysicsLab.Data.Models.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("LogicalName");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("VirtualPhysicsLab.Data.Models.Mesh", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<Guid?>("ExperimentId");

                    b.Property<string>("LogicalName");

                    b.Property<string>("Name");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ExperimentId");

                    b.ToTable("Meshes");
                });

            modelBuilder.Entity("VirtualPhysicsLab.Data.Models.MeshSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Axis");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<float>("Friction");

                    b.Property<string>("LogicalName");

                    b.Property<float>("Mass");

                    b.Property<Guid>("MeshId");

                    b.Property<string>("Name");

                    b.Property<float>("Restitution");

                    b.HasKey("Id");

                    b.HasIndex("MeshId")
                        .IsUnique();

                    b.ToTable("MeshSettings");
                });

            modelBuilder.Entity("VirtualPhysicsLab.Data.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("LogicalName");

                    b.Property<string>("Name");

                    b.Property<int>("Occupation");

                    b.Property<string>("Password");

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

                    b.Property<string>("LogicalName");

                    b.Property<string>("Name");

                    b.HasKey("UserId", "GroupId");

                    b.HasAlternateKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("UserGroups");
                });

            modelBuilder.Entity("VirtualPhysicsLab.Web.Models.Color", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("B");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<float>("G");

                    b.Property<string>("LogicalName");

                    b.Property<Guid>("MeshSettingsId");

                    b.Property<string>("Name");

                    b.Property<float>("R");

                    b.HasKey("Id");

                    b.HasIndex("MeshSettingsId")
                        .IsUnique();

                    b.ToTable("Colors");
                });

            modelBuilder.Entity("VirtualPhysicsLab.Web.Models.ExperimentSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Axis");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<Guid>("ExperimentId");

                    b.Property<float>("Friction");

                    b.Property<string>("LogicalName");

                    b.Property<string>("Name");

                    b.Property<float>("Restitution");

                    b.Property<bool>("Walls");

                    b.HasKey("Id");

                    b.HasIndex("ExperimentId")
                        .IsUnique();

                    b.ToTable("ExperimentSettings");
                });

            modelBuilder.Entity("VirtualPhysicsLab.Web.Models.Gravity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Axis");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<Guid>("ExperimentSettingsId");

                    b.Property<string>("LogicalName");

                    b.Property<string>("Name");

                    b.Property<float>("X");

                    b.Property<float>("Y");

                    b.Property<float>("Z");

                    b.HasKey("Id");

                    b.HasIndex("ExperimentSettingsId")
                        .IsUnique();

                    b.ToTable("Gravities");
                });

            modelBuilder.Entity("VirtualPhysicsLab.Web.Models.Position", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("LogicalName");

                    b.Property<Guid>("MeshSettingsId");

                    b.Property<string>("Name");

                    b.Property<float>("X");

                    b.Property<float>("Y");

                    b.Property<float>("Z");

                    b.HasKey("Id");

                    b.HasIndex("MeshSettingsId")
                        .IsUnique();

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("VirtualPhysicsLab.Web.Models.Rotation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("LogicalName");

                    b.Property<Guid>("MeshSettingsId");

                    b.Property<string>("Name");

                    b.Property<float>("X");

                    b.Property<float>("Y");

                    b.Property<float>("Z");

                    b.HasKey("Id");

                    b.HasIndex("MeshSettingsId")
                        .IsUnique();

                    b.ToTable("Rotations");
                });

            modelBuilder.Entity("VirtualPhysicsLab.Web.Models.Size", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("LogicalName");

                    b.Property<Guid>("MeshSettingsId");

                    b.Property<string>("Name");

                    b.Property<float>("X");

                    b.Property<float>("Y");

                    b.Property<float>("Z");

                    b.HasKey("Id");

                    b.HasIndex("MeshSettingsId")
                        .IsUnique();

                    b.ToTable("Sizes");
                });

            modelBuilder.Entity("VirtualPhysicsLab.Web.Models.Velocity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("LogicalName");

                    b.Property<Guid>("MeshSettingsId");

                    b.Property<string>("Name");

                    b.Property<float>("X");

                    b.Property<float>("Y");

                    b.Property<float>("Z");

                    b.HasKey("Id");

                    b.HasIndex("MeshSettingsId")
                        .IsUnique();

                    b.ToTable("Velocities");
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

            modelBuilder.Entity("VirtualPhysicsLab.Web.Models.Color", b =>
                {
                    b.HasOne("VirtualPhysicsLab.Data.Models.MeshSettings", "MeshSettings")
                        .WithOne("Color")
                        .HasForeignKey("VirtualPhysicsLab.Web.Models.Color", "MeshSettingsId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VirtualPhysicsLab.Web.Models.ExperimentSettings", b =>
                {
                    b.HasOne("VirtualPhysicsLab.Data.Models.Experiment", "Experiment")
                        .WithOne("Settings")
                        .HasForeignKey("VirtualPhysicsLab.Web.Models.ExperimentSettings", "ExperimentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VirtualPhysicsLab.Web.Models.Gravity", b =>
                {
                    b.HasOne("VirtualPhysicsLab.Web.Models.ExperimentSettings", "ExperimentSettings")
                        .WithOne("Gravity")
                        .HasForeignKey("VirtualPhysicsLab.Web.Models.Gravity", "ExperimentSettingsId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VirtualPhysicsLab.Web.Models.Position", b =>
                {
                    b.HasOne("VirtualPhysicsLab.Data.Models.MeshSettings", "MeshSettings")
                        .WithOne("Position")
                        .HasForeignKey("VirtualPhysicsLab.Web.Models.Position", "MeshSettingsId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VirtualPhysicsLab.Web.Models.Rotation", b =>
                {
                    b.HasOne("VirtualPhysicsLab.Data.Models.MeshSettings", "MeshSettings")
                        .WithOne("Rotation")
                        .HasForeignKey("VirtualPhysicsLab.Web.Models.Rotation", "MeshSettingsId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VirtualPhysicsLab.Web.Models.Size", b =>
                {
                    b.HasOne("VirtualPhysicsLab.Data.Models.MeshSettings", "MeshSettings")
                        .WithOne("Size")
                        .HasForeignKey("VirtualPhysicsLab.Web.Models.Size", "MeshSettingsId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VirtualPhysicsLab.Web.Models.Velocity", b =>
                {
                    b.HasOne("VirtualPhysicsLab.Data.Models.MeshSettings", "MeshSettings")
                        .WithOne("Velocity")
                        .HasForeignKey("VirtualPhysicsLab.Web.Models.Velocity", "MeshSettingsId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}