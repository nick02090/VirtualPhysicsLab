using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtualPhysicsLab.Web.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    LogicalName = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    LogicalName = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    NickName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Experiments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    LogicalName = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Experiments_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    LogicalName = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => new { x.UserId, x.GroupId });
                    table.UniqueConstraint("AK_UserGroups_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroups_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Meshes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    LogicalName = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    ExperimentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meshes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meshes_Experiments_ExperimentId",
                        column: x => x.ExperimentId,
                        principalTable: "Experiments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MeshSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    LogicalName = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    HexColor = table.Column<string>(nullable: true),
                    MeshId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeshSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeshSettings_Meshes_MeshId",
                        column: x => x.MeshId,
                        principalTable: "Meshes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MeshSizes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    LogicalName = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    SettingsId = table.Column<Guid>(nullable: false),
                    Value = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeshSizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeshSizes_MeshSettings_SettingsId",
                        column: x => x.SettingsId,
                        principalTable: "MeshSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Experiments_CreatedById",
                table: "Experiments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Meshes_ExperimentId",
                table: "Meshes",
                column: "ExperimentId");

            migrationBuilder.CreateIndex(
                name: "IX_MeshSettings_MeshId",
                table: "MeshSettings",
                column: "MeshId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MeshSizes_SettingsId",
                table: "MeshSizes",
                column: "SettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_GroupId",
                table: "UserGroups",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeshSizes");

            migrationBuilder.DropTable(
                name: "UserGroups");

            migrationBuilder.DropTable(
                name: "MeshSettings");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Meshes");

            migrationBuilder.DropTable(
                name: "Experiments");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
