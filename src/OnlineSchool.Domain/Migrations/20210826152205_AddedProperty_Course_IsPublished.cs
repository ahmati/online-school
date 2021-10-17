using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineSchool.Domain.Migrations
{
    public partial class AddedProperty_Course_IsPublished : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Course",
                type: "bit",
                nullable: false,
                defaultValueSql: "0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Course");
        }
    }
}
