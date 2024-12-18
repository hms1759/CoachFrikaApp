using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoachFrika.Migrations
{
    public partial class newtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Coaches_CoachId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CoachId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "Focus",
                table: "Schedule",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeetingLink",
                table: "Schedule",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoachFrikaUserId1",
                table: "Coaches",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CoachId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocalGov",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SchoolName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Coaches_CoachFrikaUserId1",
                table: "Coaches",
                column: "CoachFrikaUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CoachId",
                table: "AspNetUsers",
                column: "CoachId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_CoachId",
                table: "AspNetUsers",
                column: "CoachId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Coaches_AspNetUsers_CoachFrikaUserId1",
                table: "Coaches",
                column: "CoachFrikaUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_CoachId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Coaches_AspNetUsers_CoachFrikaUserId1",
                table: "Coaches");

            migrationBuilder.DropIndex(
                name: "IX_Coaches_CoachFrikaUserId1",
                table: "Coaches");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CoachId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MeetingLink",
                table: "Schedule");

            migrationBuilder.DropColumn(
                name: "CoachFrikaUserId1",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "LocalGov",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SchoolName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Focus",
                table: "Schedule",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CoachId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CoachId",
                table: "AspNetUsers",
                column: "CoachId",
                unique: true,
                filter: "[CoachId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Coaches_CoachId",
                table: "AspNetUsers",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id");
        }
    }
}
