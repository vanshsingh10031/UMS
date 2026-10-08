using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UMS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSubjectSemester : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Semester",
                table: "Subjects",
                type: "int",
                nullable: false,
                defaultValue: 6);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Semester",
                table: "Subjects");
        }
    }
}
