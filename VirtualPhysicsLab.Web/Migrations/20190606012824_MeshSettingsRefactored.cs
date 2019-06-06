using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtualPhysicsLab.Web.Migrations
{
    public partial class MeshSettingsRefactored : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeshSizes");

            migrationBuilder.AddColumn<float>(
                name: "Friction",
                table: "MeshSettings",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Mass",
                table: "MeshSettings",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Restitution",
                table: "MeshSettings",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Friction",
                table: "MeshSettings");

            migrationBuilder.DropColumn(
                name: "Mass",
                table: "MeshSettings");

            migrationBuilder.DropColumn(
                name: "Restitution",
                table: "MeshSettings");

            migrationBuilder.CreateTable(
                name: "MeshSizes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    LogicalName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
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
                name: "IX_MeshSizes_SettingsId",
                table: "MeshSizes",
                column: "SettingsId");
        }
    }
}
