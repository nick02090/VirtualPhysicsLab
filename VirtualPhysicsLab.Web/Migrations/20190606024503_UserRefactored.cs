using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtualPhysicsLab.Web.Migrations
{
    public partial class UserRefactored : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiments_Users_CreatedById",
                table: "Experiments");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                nullable: true,
                oldClrType: typeof(string));

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Experiments_Users_CreatedById",
                table: "Experiments");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

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
    }
}
