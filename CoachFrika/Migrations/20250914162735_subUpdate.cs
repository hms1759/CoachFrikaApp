using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoachFrika.Migrations
{
    public partial class subUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubjectCode",
                table: "Subjects");

            migrationBuilder.RenameColumn(
                name: "SubjectName",
                table: "Subjects",
                newName: "Subject");

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Subjects",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Subjects");

            migrationBuilder.RenameColumn(
                name: "Subject",
                table: "Subjects",
                newName: "SubjectName");

            migrationBuilder.AddColumn<string>(
                name: "SubjectCode",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
