using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineSchool.Domain.Migrations
{
    public partial class _DropColumn_BookedCourse_MeetingEmailReminder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeetingEmailReminder",
                table: "BookedCourse");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MeetingEmailReminder",
                table: "BookedCourse",
                type: "bit",
                nullable: false,
                defaultValueSql: "0");
        }
    }
}
