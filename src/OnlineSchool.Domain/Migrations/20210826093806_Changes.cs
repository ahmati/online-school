using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineSchool.Domain.Migrations
{
    public partial class Changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartTime",
                table: "Session",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "Session",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Session",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Course",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Course",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Course",
                type: "decimal(10, 2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Course");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "Session",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "Session",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Course",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Course",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");
        }
    }
}
