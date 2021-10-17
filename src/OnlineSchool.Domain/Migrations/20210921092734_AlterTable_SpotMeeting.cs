using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineSchool.Domain.Migrations
{
    public partial class AlterTable_SpotMeeting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MeetingId",
                table: "SpotMeeting",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MeetingPassword",
                table: "SpotMeeting",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MeetingSipAddress",
                table: "SpotMeeting",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MeetingTitle",
                table: "SpotMeeting",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeetingId",
                table: "SpotMeeting");

            migrationBuilder.DropColumn(
                name: "MeetingPassword",
                table: "SpotMeeting");

            migrationBuilder.DropColumn(
                name: "MeetingSipAddress",
                table: "SpotMeeting");

            migrationBuilder.DropColumn(
                name: "MeetingTitle",
                table: "SpotMeeting");
        }
    }
}
