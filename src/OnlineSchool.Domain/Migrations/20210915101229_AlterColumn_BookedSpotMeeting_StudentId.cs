using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineSchool.Domain.Migrations
{
    public partial class AlterColumn_BookedSpotMeeting_StudentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookedSpotMeeting_Students",
                table: "BookedSpotMeeting");

            migrationBuilder.DropIndex(
                name: "studentId_Index",
                table: "BookedSpotMeeting");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "BookedSpotMeeting");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BookedSpotMeeting",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "userId_Index",
                table: "BookedSpotMeeting",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookedSpotMeeting_Users",
                table: "BookedSpotMeeting",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookedSpotMeeting_Users",
                table: "BookedSpotMeeting");

            migrationBuilder.DropIndex(
                name: "userId_Index",
                table: "BookedSpotMeeting");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BookedSpotMeeting");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "BookedSpotMeeting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "studentId_Index",
                table: "BookedSpotMeeting",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookedSpotMeeting_Students",
                table: "BookedSpotMeeting",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
