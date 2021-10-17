using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineSchool.Domain.Migrations
{
    public partial class _AddColumn_SpotMeeting_ReminderEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ReminderEmail",
                table: "SpotMeeting",
                type: "bit",
                nullable: false,
                defaultValueSql: "0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReminderEmail",
                table: "SpotMeeting");
        }
    }
}
