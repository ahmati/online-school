using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineSchool.Domain.Migrations
{
    public partial class AddedColumns_Session : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MeetingId",
                table: "Session",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MeetingPassword",
                table: "Session",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MeetingSipAddress",
                table: "Session",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MeetingTitle",
                table: "Session",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeetingId",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "MeetingPassword",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "MeetingSipAddress",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "MeetingTitle",
                table: "Session");
        }
    }
}
