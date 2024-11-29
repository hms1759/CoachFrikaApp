using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoachFrika.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coaches_AspNetUsers_IdentityUserId1",
                table: "Coaches");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_AspNetUsers_CoachFrikaUsersId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_CoachFrikaUsersId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Coaches_IdentityUserId1",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "CoachFrikaUsersId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "FacebookUrl",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "IdentityUserId1",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "InstagramUrl",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "LinkedInUrl",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Coaches");

            migrationBuilder.RenameColumn(
                name: "TweeterUrl",
                table: "Coaches",
                newName: "ProfessionalTitle");

            migrationBuilder.RenameColumn(
                name: "IdentityUserId",
                table: "Coaches",
                newName: "CoachFrikaUserId");

            migrationBuilder.AddColumn<Guid>(
                name: "CoachId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfessionalTitle",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Stages",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StateOfOrigin",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Subscriptions",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TeacherId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CoachId",
                table: "AspNetUsers",
                column: "CoachId",
                unique: true,
                filter: "[CoachId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TeacherId",
                table: "AspNetUsers",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Coaches_CoachId",
                table: "AspNetUsers",
                column: "CoachId",
                principalTable: "Coaches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Teachers_TeacherId",
                table: "AspNetUsers",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Coaches_CoachId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Teachers_TeacherId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CoachId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TeacherId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CoachId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfessionalTitle",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Stages",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StateOfOrigin",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Subscriptions",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ProfessionalTitle",
                table: "Coaches",
                newName: "TweeterUrl");

            migrationBuilder.RenameColumn(
                name: "CoachFrikaUserId",
                table: "Coaches",
                newName: "IdentityUserId");

            migrationBuilder.AddColumn<string>(
                name: "CoachFrikaUsersId",
                table: "Subjects",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Coaches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacebookUrl",
                table: "Coaches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Coaches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId1",
                table: "Coaches",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstagramUrl",
                table: "Coaches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedInUrl",
                table: "Coaches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Coaches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_CoachFrikaUsersId",
                table: "Subjects",
                column: "CoachFrikaUsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Coaches_IdentityUserId1",
                table: "Coaches",
                column: "IdentityUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Coaches_AspNetUsers_IdentityUserId1",
                table: "Coaches",
                column: "IdentityUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_AspNetUsers_CoachFrikaUsersId",
                table: "Subjects",
                column: "CoachFrikaUsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
