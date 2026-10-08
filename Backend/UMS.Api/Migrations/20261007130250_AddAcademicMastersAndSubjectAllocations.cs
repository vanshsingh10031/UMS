using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UMS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddAcademicMastersAndSubjectAllocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Staff_StaffId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_StaffId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "StaffId",
                table: "Subjects");

            migrationBuilder.AddColumn<int>(
                name: "RoleMasterId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AcademicSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AcademicYear = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentMasters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DesignationMasters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignationMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProgramMasters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleMasters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubjectFacultyAllocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    BatchId = table.Column<int>(type: "int", nullable: true),
                    AcademicYear = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectFacultyAllocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectFacultyAllocations_Batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubjectFacultyAllocations_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectFacultyAllocations_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AcademicSessions",
                columns: new[] { "Id", "AcademicYear", "CreatedAt", "IsActive", "Semester", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "2022-26", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2316), true, 6, new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2317) },
                    { 2, "2023-27", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2318), true, 4, new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2318) },
                    { 3, "2024-28", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2319), true, 2, new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2319) }
                });

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

            migrationBuilder.InsertData(
                table: "DepartmentMasters",
                columns: new[] { "Id", "Code", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "CSE", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2234), "Computer Science", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2234) },
                    { 2, "ECE", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2235), "Electronics", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2236) },
                    { 3, "MAT", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2236), "Mathematics", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2237) },
                    { 4, "MEC", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2237), "Mechanical", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2238) },
                    { 5, "MGT", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2238), "Management", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2239) }
                });

            migrationBuilder.InsertData(
                table: "DesignationMasters",
                columns: new[] { "Id", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2260), "Professor", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2261) },
                    { 2, new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2261), "Associate Professor", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2262) },
                    { 3, new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2262), "Assistant Professor", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2263) },
                    { 4, new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2263), "Administrative Staff", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2264) }
                });

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

            migrationBuilder.InsertData(
                table: "ProgramMasters",
                columns: new[] { "Id", "Code", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "BT-CSE", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2292), "B.Tech CSE", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2292) },
                    { 2, "BT-ECE", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2293), "B.Tech ECE", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2293) },
                    { 3, "BT-ME", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2294), "B.Tech ME", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2295) },
                    { 4, "MBA", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2295), "MBA", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2296) }
                });

            migrationBuilder.InsertData(
                table: "RoleMasters",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(1997), "University administrator", "Admin", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2001) },
                    { 2, new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2002), "Faculty member", "Faculty", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2002) },
                    { 3, new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2003), "Student user", "Student", new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2004) }
                });

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

            migrationBuilder.InsertData(
                table: "SubjectFacultyAllocations",
                columns: new[] { "Id", "AcademicYear", "BatchId", "CreatedAt", "Semester", "StaffId", "SubjectId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "2022-26", 1, new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3095), 6, 1, 1, new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3096) },
                    { 2, "2022-26", 1, new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3097), 6, 2, 2, new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3097) },
                    { 3, "2022-26", 1, new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3098), 6, 1, 3, new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(3098) }
                });

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
                columns: new[] { "CreatedAt", "RoleMasterId", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2345), 1, new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2345) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "RoleMasterId", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2346), 2, new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2346) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "RoleMasterId", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2347), 2, new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2347) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "RoleMasterId", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2348), 3, new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2348) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "RoleMasterId", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2349), 3, new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2349) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "RoleMasterId", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2350), 3, new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2350) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "RoleMasterId", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2351), 3, new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2351) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "RoleMasterId", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2352), 3, new DateTime(2026, 10, 7, 13, 2, 49, 329, DateTimeKind.Utc).AddTicks(2352) });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleMasterId",
                table: "Users",
                column: "RoleMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicSessions_AcademicYear_Semester",
                table: "AcademicSessions",
                columns: new[] { "AcademicYear", "Semester" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentMasters_Name",
                table: "DepartmentMasters",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DesignationMasters_Name",
                table: "DesignationMasters",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProgramMasters_Name",
                table: "ProgramMasters",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleMasters_Name",
                table: "RoleMasters",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubjectFacultyAllocations_BatchId",
                table: "SubjectFacultyAllocations",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectFacultyAllocations_StaffId",
                table: "SubjectFacultyAllocations",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectFacultyAllocations_SubjectId_StaffId_BatchId_AcademicYear_Semester",
                table: "SubjectFacultyAllocations",
                columns: new[] { "SubjectId", "StaffId", "BatchId", "AcademicYear", "Semester" },
                unique: true,
                filter: "[BatchId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_RoleMasters_RoleMasterId",
                table: "Users",
                column: "RoleMasterId",
                principalTable: "RoleMasters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_RoleMasters_RoleMasterId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "AcademicSessions");

            migrationBuilder.DropTable(
                name: "DepartmentMasters");

            migrationBuilder.DropTable(
                name: "DesignationMasters");

            migrationBuilder.DropTable(
                name: "ProgramMasters");

            migrationBuilder.DropTable(
                name: "RoleMasters");

            migrationBuilder.DropTable(
                name: "SubjectFacultyAllocations");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleMasterId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoleMasterId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "StaffId",
                table: "Subjects",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3950), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3951) });

            migrationBuilder.UpdateData(
                table: "Assignments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3960), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3960) });

            migrationBuilder.UpdateData(
                table: "Batches",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3783), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3783) });

            migrationBuilder.UpdateData(
                table: "Batches",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3785), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3785) });

            migrationBuilder.UpdateData(
                table: "Batches",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3786), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3786) });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3750), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3751) });

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3759), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3760) });

            migrationBuilder.UpdateData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3879), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3879) });

            migrationBuilder.UpdateData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3880), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3880) });

            migrationBuilder.UpdateData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3881), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3881) });

            migrationBuilder.UpdateData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3882), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3882) });

            migrationBuilder.UpdateData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3883), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3883) });

            migrationBuilder.UpdateData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3884), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3884) });

            migrationBuilder.UpdateData(
                table: "LibraryBooks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(4018), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(4019) });

            migrationBuilder.UpdateData(
                table: "LibraryBooks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(4021), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(4021) });

            migrationBuilder.UpdateData(
                table: "Marks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3990), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3990) });

            migrationBuilder.UpdateData(
                table: "Marks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3996), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3996) });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(4049), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(4049) });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(4051), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(4051) });

            migrationBuilder.UpdateData(
                table: "Staff",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3714), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3714) });

            migrationBuilder.UpdateData(
                table: "Staff",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3716), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3717) });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3812), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3812) });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3815), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3815) });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3817), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3817) });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3819), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3819) });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3821), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3821) });

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "StaffId", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3846), 1, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3846) });

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "StaffId", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3849), 2, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3849) });

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "StaffId", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3850), 1, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3851) });

            migrationBuilder.UpdateData(
                table: "TimetableEntries",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3914), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3914) });

            migrationBuilder.UpdateData(
                table: "TimetableEntries",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3918), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3918) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3249), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3253) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3255), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3255) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3256), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3256) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3257), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3257) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3258), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3258) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3259), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3259) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3260), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3260) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3261), new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3261) });

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_StaffId",
                table: "Subjects",
                column: "StaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Staff_StaffId",
                table: "Subjects",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "Id");
        }
    }
}
