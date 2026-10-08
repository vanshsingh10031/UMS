using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UMS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCalendarEventFeedFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BatchId",
                table: "AcademicCalendarEvents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "AcademicCalendarEvents",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StaffId",
                table: "AcademicCalendarEvents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "AcademicCalendarEvents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "AcademicCalendarEvents",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "Academic");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicCalendarEvents_BatchId",
                table: "AcademicCalendarEvents",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicCalendarEvents_StaffId",
                table: "AcademicCalendarEvents",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicCalendarEvents_SubjectId",
                table: "AcademicCalendarEvents",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicCalendarEvents_Batches_BatchId",
                table: "AcademicCalendarEvents",
                column: "BatchId",
                principalTable: "Batches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicCalendarEvents_Staff_StaffId",
                table: "AcademicCalendarEvents",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicCalendarEvents_Subjects_SubjectId",
                table: "AcademicCalendarEvents",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicCalendarEvents_Batches_BatchId",
                table: "AcademicCalendarEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_AcademicCalendarEvents_Staff_StaffId",
                table: "AcademicCalendarEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_AcademicCalendarEvents_Subjects_SubjectId",
                table: "AcademicCalendarEvents");

            migrationBuilder.DropIndex(
                name: "IX_AcademicCalendarEvents_BatchId",
                table: "AcademicCalendarEvents");

            migrationBuilder.DropIndex(
                name: "IX_AcademicCalendarEvents_StaffId",
                table: "AcademicCalendarEvents");

            migrationBuilder.DropIndex(
                name: "IX_AcademicCalendarEvents_SubjectId",
                table: "AcademicCalendarEvents");

            migrationBuilder.DropColumn(
                name: "BatchId",
                table: "AcademicCalendarEvents");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "AcademicCalendarEvents");

            migrationBuilder.DropColumn(
                name: "StaffId",
                table: "AcademicCalendarEvents");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "AcademicCalendarEvents");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "AcademicCalendarEvents");
        }
    }
}
