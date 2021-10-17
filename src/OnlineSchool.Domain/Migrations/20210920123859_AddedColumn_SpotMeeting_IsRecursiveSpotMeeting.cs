using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineSchool.Domain.Migrations
{
    public partial class AddedColumn_SpotMeeting_IsRecursiveSpotMeeting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRecursiveSpotMeeting",
                table: "SpotMeeting",
                type: "bit",
                nullable: false,
                defaultValueSql: "0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRecursiveSpotMeeting",
                table: "SpotMeeting");
        }
    }
}
