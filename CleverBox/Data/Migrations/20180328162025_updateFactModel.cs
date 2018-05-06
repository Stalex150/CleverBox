using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CleverBox.Data.Migrations
{
    public partial class updateFactModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facts_AspNetUsers_UserId1",
                table: "Facts");

            migrationBuilder.DropIndex(
                name: "IX_Facts_UserId1",
                table: "Facts");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Facts");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Facts",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Facts_UserId",
                table: "Facts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facts_AspNetUsers_UserId",
                table: "Facts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facts_AspNetUsers_UserId",
                table: "Facts");

            migrationBuilder.DropIndex(
                name: "IX_Facts_UserId",
                table: "Facts");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Facts",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Facts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Facts_UserId1",
                table: "Facts",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Facts_AspNetUsers_UserId1",
                table: "Facts",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
