using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtualPhysicsLab.Web.Migrations
{
    public partial class CascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiments_Users_CreatedById",
                table: "Experiments");

            migrationBuilder.DropForeignKey(
                name: "FK_Meshes_Experiments_ExperimentId",
                table: "Meshes");

            migrationBuilder.AddForeignKey(
                name: "FK_Experiments_Users_CreatedById",
                table: "Experiments",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Meshes_Experiments_ExperimentId",
                table: "Meshes",
                column: "ExperimentId",
                principalTable: "Experiments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiments_Users_CreatedById",
                table: "Experiments");

            migrationBuilder.DropForeignKey(
                name: "FK_Meshes_Experiments_ExperimentId",
                table: "Meshes");

            migrationBuilder.AddForeignKey(
                name: "FK_Experiments_Users_CreatedById",
                table: "Experiments",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Meshes_Experiments_ExperimentId",
                table: "Meshes",
                column: "ExperimentId",
                principalTable: "Experiments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
