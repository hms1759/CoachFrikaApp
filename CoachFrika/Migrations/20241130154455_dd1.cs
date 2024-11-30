using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoachFrika.Migrations
{
    public partial class dd1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Subscriptions",
                table: "SchoolEnrollmentRequest",
                newName: "Programtype");

            migrationBuilder.AddColumn<string>(
                name: "ContactPersonEmail",
                table: "SchoolEnrollmentRequest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPersonName",
                table: "SchoolEnrollmentRequest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactPersonPhoneNumber",
                table: "SchoolEnrollmentRequest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Goals",
                table: "SchoolEnrollmentRequest",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactPersonEmail",
                table: "SchoolEnrollmentRequest");

            migrationBuilder.DropColumn(
                name: "ContactPersonName",
                table: "SchoolEnrollmentRequest");

            migrationBuilder.DropColumn(
                name: "ContactPersonPhoneNumber",
                table: "SchoolEnrollmentRequest");

            migrationBuilder.DropColumn(
                name: "Goals",
                table: "SchoolEnrollmentRequest");

            migrationBuilder.RenameColumn(
                name: "Programtype",
                table: "SchoolEnrollmentRequest",
                newName: "Subscriptions");
        }
    }
}
