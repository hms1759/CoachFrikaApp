using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoachFrika.Migrations
{
    public partial class subjs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentScoreSheet_Subjects_SubjectId",
                table: "StudentScoreSheet");

            migrationBuilder.DropIndex(
                name: "IX_StudentScoreSheet_SubjectId",
                table: "StudentScoreSheet");

            migrationBuilder.AlterColumn<string>(
                name: "SubjectId",
                table: "StudentScoreSheet",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "SubjectId",
                table: "StudentScoreSheet",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentScoreSheet_SubjectId",
                table: "StudentScoreSheet",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentScoreSheet_Subjects_SubjectId",
                table: "StudentScoreSheet",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
