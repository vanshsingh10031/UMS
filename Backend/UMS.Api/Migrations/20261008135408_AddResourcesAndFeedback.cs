using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UMS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddResourcesAndFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FacultyFeedback",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    BatchId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    AcademicYear = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacultyFeedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacultyFeedback_Batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacultyFeedback_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacultyFeedback_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacultyFeedback_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudyResources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(180)", maxLength: 180, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ResourceType = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    BatchId = table.Column<int>(type: "int", nullable: true),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudyResources_Batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudyResources_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudyResources_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacultyFeedback_BatchId",
                table: "FacultyFeedback",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_FacultyFeedback_StaffId",
                table: "FacultyFeedback",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_FacultyFeedback_StudentId_StaffId_SubjectId_BatchId_Semester",
                table: "FacultyFeedback",
                columns: new[] { "StudentId", "StaffId", "SubjectId", "BatchId", "Semester" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FacultyFeedback_SubjectId",
                table: "FacultyFeedback",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyResources_BatchId",
                table: "StudyResources",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyResources_StaffId",
                table: "StudyResources",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyResources_SubjectId_BatchId_StaffId_Title",
                table: "StudyResources",
                columns: new[] { "SubjectId", "BatchId", "StaffId", "Title" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "FacultyFeedback");
            migrationBuilder.DropTable(name: "StudyResources");
        }
    }
}
