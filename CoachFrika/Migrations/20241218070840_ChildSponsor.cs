using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoachFrika.Migrations
{
    public partial class ChildSponsor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChildSponsor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SponsorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SponsorEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SponsorPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumbersOfChildren = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildSponsor", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChildSponsor");
        }
    }
}
