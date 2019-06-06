using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtualPhysicsLab.Web.Migrations
{
    public partial class UserExperimentRefactored2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiments_Users_Id",
                table: "Experiments");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Experiments",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Experiments_CreatedById",
                table: "Experiments",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Experiments_Users_CreatedById",
                table: "Experiments",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiments_Users_CreatedById",
                table: "Experiments");

            migrationBuilder.DropIndex(
                name: "IX_Experiments_CreatedById",
                table: "Experiments");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Experiments");

            migrationBuilder.AddForeignKey(
                name: "FK_Experiments_Users_Id",
                table: "Experiments",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
