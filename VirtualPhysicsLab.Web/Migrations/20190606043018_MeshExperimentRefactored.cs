using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtualPhysicsLab.Web.Migrations
{
    public partial class MeshExperimentRefactored : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meshes_Experiments_ExperimentId",
                table: "Meshes");

            migrationBuilder.AlterColumn<Guid>(
                name: "ExperimentId",
                table: "Meshes",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Meshes_Experiments_ExperimentId",
                table: "Meshes",
                column: "ExperimentId",
                principalTable: "Experiments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meshes_Experiments_ExperimentId",
                table: "Meshes");

            migrationBuilder.AlterColumn<Guid>(
                name: "ExperimentId",
                table: "Meshes",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Meshes_Experiments_ExperimentId",
                table: "Meshes",
                column: "ExperimentId",
                principalTable: "Experiments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
