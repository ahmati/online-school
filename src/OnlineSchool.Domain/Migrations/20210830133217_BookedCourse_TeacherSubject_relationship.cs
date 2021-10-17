using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineSchool.Domain.Migrations
{
    public partial class BookedCourse_TeacherSubject_relationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "BookedCourse",
                newName: "TeacherSubjectId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "BookedCourse",
                type: "decimal(10, 2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AuthDate",
                table: "BookedCourse",
                type: "datetime",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "teacherSubjectId_Index",
                table: "BookedCourse",
                column: "TeacherSubjectId");

            migrationBuilder.CreateIndex(
                name: "studentId_Index",
                table: "BookedCourse",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookedCourses_TeacherSubjects",
                table: "BookedCourse",
                column: "TeacherSubjectId",
                principalTable: "TeacherSubject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookedCourses_Students",
                table: "BookedCourse",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookedCourses_TeacherSubjects",
                table: "BookedCourse");

            migrationBuilder.DropIndex(
                name: "teacherSubjectId_Index",
                table: "BookedCourse");

            migrationBuilder.RenameColumn(
                name: "TeacherSubjectId",
                table: "BookedCourse",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "studentId_Index",
                table: "BookedCourse",
                newName: "IX_BookedCourse_StudentId");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "BookedCourse",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10, 2)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AuthDate",
                table: "BookedCourse",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "(getdate())");
        }
    }
}
