using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineSchool.Domain.Migrations
{
    public partial class AlterTable_SpotMeetingTeacher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHost",
                table: "SpotMeetingTeacher",
                type: "bit",
                nullable: false,
                defaultValueSql: "0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHost",
                table: "SpotMeetingTeacher");
        }
    }
}
