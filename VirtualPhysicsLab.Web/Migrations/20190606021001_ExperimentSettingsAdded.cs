using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtualPhysicsLab.Web.Migrations
{
    public partial class ExperimentSettingsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Axis",
                table: "MeshSettings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Experiments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Experiments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExperimentSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LogicalName = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Friction = table.Column<float>(nullable: false),
                    Restitution = table.Column<float>(nullable: false),
                    Walls = table.Column<bool>(nullable: false),
                    Axis = table.Column<bool>(nullable: false),
                    ExperimentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperimentSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExperimentSettings_Experiments_ExperimentId",
                        column: x => x.ExperimentId,
                        principalTable: "Experiments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentSettings_ExperimentId",
                table: "ExperimentSettings",
                column: "ExperimentId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExperimentSettings");

            migrationBuilder.DropColumn(
                name: "Axis",
                table: "MeshSettings");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Experiments");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Experiments");
        }
    }
}
