using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UMS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddAdvancedAcademicModules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "MaximumMarks",
                table: "Marks",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MarksObtained",
                table: "Marks",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MaximumMarks",
                table: "Assignments",
                type: "decimal(8,2)",
                precision: 8,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateTable(
                name: "EvaluationComponents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    Weightage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    MaximumMarks = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvaluationComponents_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GraceMarkCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MaximumMarks = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraceMarkCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GradeRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: true),
                    MinimumPercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Grade = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    GradePoint = table.Column<decimal>(type: "decimal(4,2)", precision: 4, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradeRules_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Audience = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosesAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surveys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GraceMarkApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    GraceMarkCategoryId = table.Column<int>(type: "int", nullable: false),
                    MarksAwarded = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraceMarkApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GraceMarkApplications_GraceMarkCategories_GraceMarkCategoryId",
                        column: x => x.GraceMarkCategoryId,
                        principalTable: "GraceMarkCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GraceMarkApplications_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GraceMarkApplications_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurveyResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurveyId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ResponseJson = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyResponses_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SurveyResponses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AcademicSessions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5339), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5339) });

            migrationBuilder.UpdateData(
                table: "AcademicSessions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5340), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5341) });

            migrationBuilder.UpdateData(
                table: "AcademicSessions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5341), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5342) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5707), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5707) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5713), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5713) });

            migrationBuilder.UpdateData(
                table: "Batches",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5477), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5477) });

            migrationBuilder.UpdateData(
                table: "Batches",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5479), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5479) });

            migrationBuilder.UpdateData(
                table: "Batches",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5480), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5480) });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5448), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5448) });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5450), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5450) });

            migrationBuilder.UpdateData(
                table: "DepartmentMasters",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5227), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5228) });

            migrationBuilder.UpdateData(
                table: "DepartmentMasters",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5229), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5229) });

            migrationBuilder.UpdateData(
                table: "DepartmentMasters",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5230), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5230) });

            migrationBuilder.UpdateData(
                table: "DepartmentMasters",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5231), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5231) });

            migrationBuilder.UpdateData(
                table: "DepartmentMasters",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5232), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5232) });

            migrationBuilder.UpdateData(
                table: "DesignationMasters",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5266), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5266) });

            migrationBuilder.UpdateData(
                table: "DesignationMasters",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5267), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5267) });

            migrationBuilder.UpdateData(
                table: "DesignationMasters",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5268), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5268) });

            migrationBuilder.UpdateData(
                table: "DesignationMasters",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5269), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5269) });

            migrationBuilder.UpdateData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5639), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5640) });

            migrationBuilder.UpdateData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5641), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5641) });

            migrationBuilder.UpdateData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5642), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5642) });

            migrationBuilder.UpdateData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5643), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5643) });

            migrationBuilder.UpdateData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5644), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5644) });

            migrationBuilder.UpdateData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5645), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5645) });

            migrationBuilder.InsertData(
                table: "EvaluationComponents",
                columns: new[] { "Id", "CreatedAt", "MaximumMarks", "Name", "Status", "SubjectId", "UpdatedAt", "Weightage" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5820), 50m, "Internal Assessment 1", "Active", 1, new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5821), 20m },
                    { 2, new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5822), 100m, "Assignment", "Active", 1, new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5823), 20m },
                    { 3, new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5824), 100m, "End Semester", "Active", 1, new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5824), 60m }
                });

            migrationBuilder.InsertData(
                table: "GraceMarkCategories",
                columns: new[] { "Id", "CreatedAt", "Description", "MaximumMarks", "Name", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5873), "Approved sports participation grace marks.", 5m, "Sports Representation", "Active", new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5873) },
                    { 2, new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5875), "Approved medical consideration grace marks.", 3m, "Medical Consideration", "Active", new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5875) }
                });

            migrationBuilder.InsertData(
                table: "GradeRules",
                columns: new[] { "Id", "CreatedAt", "Grade", "GradePoint", "MinimumPercentage", "Name", "Status", "SubjectId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5845), "A+", 10m, 90m, "Default A+", "Active", null, new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5845) },
                    { 2, new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5848), "A", 9m, 80m, "Default A", "Active", null, new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5848) },
                    { 3, new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5849), "B+", 8m, 70m, "Default B+", "Active", null, new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5849) },
                    { 4, new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5850), "B", 7m, 60m, "Default B", "Active", null, new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5851) },
                    { 5, new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5852), "C", 6m, 50m, "Default C", "Active", null, new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5852) }
                });

            migrationBuilder.UpdateData(
                table: "LibraryBooks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5771), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5772) });

            migrationBuilder.UpdateData(
                table: "LibraryBooks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5773), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5774) });

            migrationBuilder.UpdateData(
                table: "Marks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5734), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5734) });

            migrationBuilder.UpdateData(
                table: "Marks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5744), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5744) });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5799), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5800) });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5801), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5801) });

            migrationBuilder.UpdateData(
                table: "ProgramMasters",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5306), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5307) });

            migrationBuilder.UpdateData(
                table: "ProgramMasters",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5308), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5308) });

            migrationBuilder.UpdateData(
                table: "ProgramMasters",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5309), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5310) });

            migrationBuilder.UpdateData(
                table: "ProgramMasters",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5310), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5311) });

            migrationBuilder.UpdateData(
                table: "RoleMasters",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(4874), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(4878) });

            migrationBuilder.UpdateData(
                table: "RoleMasters",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(4879), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(4880) });

            migrationBuilder.UpdateData(
                table: "RoleMasters",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(4880), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(4881) });

            migrationBuilder.UpdateData(
                table: "Staff",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5415), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5416) });

            migrationBuilder.UpdateData(
                table: "Staff",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5419), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5419) });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5505), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5505) });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5508), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5508) });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5510), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5510) });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5512), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5512) });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5516), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5516) });

            migrationBuilder.UpdateData(
                table: "SubjectFacultyAllocations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5607), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5607) });

            migrationBuilder.UpdateData(
                table: "SubjectFacultyAllocations",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5609), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5609) });

            migrationBuilder.UpdateData(
                table: "SubjectFacultyAllocations",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5610), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5610) });

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5549), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5550) });

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5551), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5551) });

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5583), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5583) });

            migrationBuilder.InsertData(
                table: "Surveys",
                columns: new[] { "Id", "Audience", "ClosesAt", "CreatedAt", "Description", "PublishedAt", "Status", "Title", "UpdatedAt" },
                values: new object[] { 1, "Student", new DateTime(2026, 10, 31, 23, 59, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5920), "Mid-semester course feedback.", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Published", "Course Feedback - Data Structures", new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5920) });

            migrationBuilder.UpdateData(
                table: "TimetableEntries",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5675), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5675) });

            migrationBuilder.UpdateData(
                table: "TimetableEntries",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5679), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5679) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5370), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5371) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5375), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5375) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5376), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5376) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5377), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5377) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5378), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5378) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5379), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5379) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5380), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5380) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5380), new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5381) });

            migrationBuilder.InsertData(
                table: "GraceMarkApplications",
                columns: new[] { "Id", "CreatedAt", "GraceMarkCategoryId", "MarksAwarded", "Reason", "Status", "StudentId", "SubjectId", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5895), 1, 2m, "Inter-university sports representation", "Approved", 1, 1, new DateTime(2026, 10, 7, 17, 57, 4, 995, DateTimeKind.Utc).AddTicks(5895) });

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationComponents_SubjectId_Name",
                table: "EvaluationComponents",
                columns: new[] { "SubjectId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GraceMarkApplications_GraceMarkCategoryId",
                table: "GraceMarkApplications",
                column: "GraceMarkCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_GraceMarkApplications_StudentId",
                table: "GraceMarkApplications",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_GraceMarkApplications_SubjectId",
                table: "GraceMarkApplications",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeRules_SubjectId_Name_Grade",
                table: "GradeRules",
                columns: new[] { "SubjectId", "Name", "Grade" },
                unique: true,
                filter: "[SubjectId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponses_SurveyId_UserId",
                table: "SurveyResponses",
                columns: new[] { "SurveyId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponses_UserId",
                table: "SurveyResponses",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvaluationComponents");

            migrationBuilder.DropTable(
                name: "GraceMarkApplications");

            migrationBuilder.DropTable(
                name: "GradeRules");

            migrationBuilder.DropTable(
                name: "SurveyResponses");

            migrationBuilder.DropTable(
                name: "GraceMarkCategories");

            migrationBuilder.DropTable(
                name: "Surveys");

            migrationBuilder.AlterColumn<decimal>(
                name: "MaximumMarks",
                table: "Marks",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "MarksObtained",
                table: "Marks",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "MaximumMarks",
                table: "Assignments",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,2)",
                oldPrecision: 8,
                oldScale: 2);

            migrationBuilder.UpdateData(
                table: "AcademicSessions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2316), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2317) });

            migrationBuilder.UpdateData(
                table: "AcademicSessions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2318), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2318) });

            migrationBuilder.UpdateData(
                table: "AcademicSessions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2319), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2319) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3181), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3181) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3190), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3190) });

            migrationBuilder.UpdateData(
                table: "Batches",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2435), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2436) });

            migrationBuilder.UpdateData(
                table: "Batches",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2437), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3010) });

            migrationBuilder.UpdateData(
                table: "Batches",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3012), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3012) });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2407), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2407) });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2409), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2409) });

            migrationBuilder.UpdateData(
                table: "DepartmentMasters",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2234), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2234) });

            migrationBuilder.UpdateData(
                table: "DepartmentMasters",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2235), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2236) });

            migrationBuilder.UpdateData(
                table: "DepartmentMasters",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2236), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2237) });

            migrationBuilder.UpdateData(
                table: "DepartmentMasters",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2237), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2238) });

            migrationBuilder.UpdateData(
                table: "DepartmentMasters",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2238), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2239) });

            migrationBuilder.UpdateData(
                table: "DesignationMasters",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2260), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2261) });

            migrationBuilder.UpdateData(
                table: "DesignationMasters",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2261), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2262) });

            migrationBuilder.UpdateData(
                table: "DesignationMasters",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2262), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2263) });

            migrationBuilder.UpdateData(
                table: "DesignationMasters",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2263), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2264) });

            migrationBuilder.UpdateData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3123), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3123) });

            migrationBuilder.UpdateData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3124), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3124) });

            migrationBuilder.UpdateData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3125), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3125) });

            migrationBuilder.UpdateData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3126), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3126) });

            migrationBuilder.UpdateData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3127), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3127) });

            migrationBuilder.UpdateData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3128), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3128) });

            migrationBuilder.UpdateData(
                table: "LibraryBooks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3248), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3248) });

            migrationBuilder.UpdateData(
                table: "LibraryBooks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3251), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3251) });

            migrationBuilder.UpdateData(
                table: "Marks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3215), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3215) });

            migrationBuilder.UpdateData(
                table: "Marks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3222), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3223) });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3272), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3272) });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3274), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3274) });

            migrationBuilder.UpdateData(
                table: "ProgramMasters",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2292), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2292) });

            migrationBuilder.UpdateData(
                table: "ProgramMasters",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2293), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2293) });

            migrationBuilder.UpdateData(
                table: "ProgramMasters",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2294), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2295) });

            migrationBuilder.UpdateData(
                table: "ProgramMasters",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2295), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2296) });

            migrationBuilder.UpdateData(
                table: "RoleMasters",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(1997), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2001) });

            migrationBuilder.UpdateData(
                table: "RoleMasters",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2002), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2002) });

            migrationBuilder.UpdateData(
                table: "RoleMasters",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2003), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2004) });

            migrationBuilder.UpdateData(
                table: "Staff",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2377), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2378) });

            migrationBuilder.UpdateData(
                table: "Staff",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2379), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2380) });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3037), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3037) });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3040), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3040) });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3042), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3042) });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3043), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3044) });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3045), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3045) });

            migrationBuilder.UpdateData(
                table: "SubjectFacultyAllocations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3095), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3096) });

            migrationBuilder.UpdateData(
                table: "SubjectFacultyAllocations",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3097), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3097) });

            migrationBuilder.UpdateData(
                table: "SubjectFacultyAllocations",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3098), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3098) });

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3071), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3071) });

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3072), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3073) });

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3074), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3074) });

            migrationBuilder.UpdateData(
                table: "TimetableEntries",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3153), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3153) });

            migrationBuilder.UpdateData(
                table: "TimetableEntries",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3157), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3157) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2345), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2345) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2346), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2346) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2347), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2347) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2348), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2348) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2349), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2349) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2350), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2350) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2351), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2351) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2352), new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2352) });
        }
    }
}
