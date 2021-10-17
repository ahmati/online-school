using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineSchool.Domain.Migrations
{
    public partial class AddedTables_SpotMeetingMaterial_SpotMeetingTeacher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpotMeetingMaterial",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpotMeetingId = table.Column<int>(nullable: false),
                    MaterialId = table.Column<int>(nullable: false),
                    AuthDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpotMeetingMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpotMeetingMaterial_Material",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SpotMeetingMaterial_SpotMeeting",
                        column: x => x.SpotMeetingId,
                        principalTable: "SpotMeeting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SpotMeetingTeacher",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpotMeetingId = table.Column<int>(nullable: false),
                    TeacherId = table.Column<int>(nullable: false),
                    AuthDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpotMeetingTeacher", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpotMeetingTeacher_SpotMeeting",
                        column: x => x.SpotMeetingId,
                        principalTable: "SpotMeeting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SpotMeetingTeacher_Teacher",
                        column: x => x.TeacherId,
                        principalTable: "Teacher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpotMeetingMaterial_MaterialId",
                table: "SpotMeetingMaterial",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "lessonId_materialId_Index",
                table: "SpotMeetingMaterial",
                columns: new[] { "SpotMeetingId", "MaterialId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpotMeetingTeacher_TeacherId",
                table: "SpotMeetingTeacher",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "lessonId_teacherId_Index",
                table: "SpotMeetingTeacher",
                columns: new[] { "SpotMeetingId", "TeacherId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpotMeetingMaterial");

            migrationBuilder.DropTable(
                name: "SpotMeetingTeacher");
        }
    }
}
