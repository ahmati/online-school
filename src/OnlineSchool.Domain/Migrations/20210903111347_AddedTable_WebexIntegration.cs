using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineSchool.Domain.Migrations
{
    public partial class AddedTable_WebexIntegration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebexIntegration",
                columns: table => new
                {
                    ClientId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ClientSecret = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    AccessToken = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ExpiresIn = table.Column<long>(type: "bigint", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebexIntegration");
        }
    }
}
