using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineSchool.Domain.Migrations
{
    public partial class AddedTable_WebexGuestIssuer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebexGuestIssuer",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    Secret = table.Column<string>(type: "nvarchar(300)", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebexGuestIssuer");
        }
    }
}
