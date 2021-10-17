using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineSchool.Domain.Migrations
{
    public partial class AddedTable_SpotMeeting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpotMeeting",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    AuthDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpotMeeting", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "title_Index",
                table: "SpotMeeting",
                column: "Title");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpotMeeting");
        }
    }
}
