using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineSchool.Domain.Migrations
{
    public partial class AddedTable_BookedSpotMeeting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookedSpotMeeting",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    SpotMeetingId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    AuthDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookedSpotMeeting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookedSpotMeeting_SpotMeetings",
                        column: x => x.SpotMeetingId,
                        principalTable: "SpotMeeting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookedSpotMeeting_Students",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "SpotMeetingId_Index",
                table: "BookedSpotMeeting",
                column: "SpotMeetingId");

            migrationBuilder.CreateIndex(
                name: "studentId_Index",
                table: "BookedSpotMeeting",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookedSpotMeeting");
        }
    }
}
