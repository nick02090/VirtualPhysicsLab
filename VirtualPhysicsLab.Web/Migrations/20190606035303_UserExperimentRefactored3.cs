using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtualPhysicsLab.Web.Migrations
{
    public partial class UserExperimentRefactored3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiments_Users_CreatedById",
                table: "Experiments");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedById",
                table: "Experiments",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Experiments_Users_CreatedById",
                table: "Experiments",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiments_Users_CreatedById",
                table: "Experiments");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedById",
                table: "Experiments",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Experiments_Users_CreatedById",
                table: "Experiments",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
