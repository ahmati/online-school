using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineSchool.Domain.Migrations
{
    public partial class AddedColumn_WebexIntegration_RedirectUri : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ClientSecret",
                table: "WebexIntegration",
                type: "nvarchar(300)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "WebexIntegration",
                type: "nvarchar(300)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<string>(
                name: "AccessToken",
                table: "WebexIntegration",
                type: "nvarchar(300)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AddColumn<string>(
                name: "RedirectUri",
                table: "WebexIntegration",
                type: "nvarchar(300)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RedirectUri",
                table: "WebexIntegration");

            migrationBuilder.AlterColumn<string>(
                name: "ClientSecret",
                table: "WebexIntegration",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "WebexIntegration",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)");

            migrationBuilder.AlterColumn<string>(
                name: "AccessToken",
                table: "WebexIntegration",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)");
        }
    }
}
