using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UMS.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcademicCalendarEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    StartsAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndsAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicCalendarEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Batches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Program = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AcademicYear = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseCode = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Credits = table.Column<int>(type: "int", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LibraryBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(180)", maxLength: 180, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Isbn = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TotalCount = table.Column<int>(type: "int", nullable: false),
                    AvailableCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryBooks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LibraryTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LibraryBookId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibraryTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LibraryTransactions_LibraryBooks_LibraryBookId",
                        column: x => x.LibraryBookId,
                        principalTable: "LibraryBooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LibraryTransactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Staff_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Program = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BatchId = table.Column<int>(type: "int", nullable: true),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Students_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Credits = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    StaffId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subjects_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Subjects_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(180)", maxLength: 180, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    BatchId = table.Column<int>(type: "int", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaximumMarks = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rubric = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignments_Batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    BatchId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Session = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceRecords_Batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendanceRecords_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendanceRecords_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enrollments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollments_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Examinations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    BatchId = table.Column<int>(type: "int", nullable: false),
                    StartsAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndsAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Room = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Examinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Examinations_Batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Examinations_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimetableEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    Room = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    BatchId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimetableEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimetableEntries_Batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimetableEntries_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimetableEntries_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Marks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    AssignmentId = table.Column<int>(type: "int", nullable: true),
                    MarksObtained = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaximumMarks = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Marks_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Marks_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Marks_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignmentId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Submissions_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Submissions_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Batches",
                columns: new[] { "Id", "AcademicYear", "CreatedAt", "Name", "Program", "Semester", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "2022-26", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3783), "CSE 2022 - Section A", "B.Tech CSE", 6, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3783) },
                    { 2, "2022-26", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3785), "ECE 2022 - Section A", "B.Tech ECE", 6, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3785) },
                    { 3, "2023-27", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3786), "CSE 2023 - Section A", "B.Tech CSE", 4, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3786) }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CourseCode", "CreatedAt", "Credits", "Department", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "CSE-BT", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3750), 160, "Computer Science", "Undergraduate computer science programme", "B.Tech Computer Science", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3751) },
                    { 2, "ECE-BT", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3759), 160, "Electronics", "Undergraduate electronics programme", "B.Tech Electronics & Communication", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3760) }
                });

            migrationBuilder.InsertData(
                table: "LibraryBooks",
                columns: new[] { "Id", "Author", "AvailableCount", "Category", "CreatedAt", "Isbn", "Title", "TotalCount", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Cormen, Leiserson, Rivest", 4, "Computer Science", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(4018), "CS-ALGO-128", "Introduction to Algorithms", 5, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(4019) },
                    { 2, "Martin Kleppmann", 3, "Technology", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(4021), "CS-DATA-204", "Designing Data-Intensive Applications", 3, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(4021) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "DisplayName", "Email", "PasswordHash", "Role", "UpdatedAt", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3249), "Dr. Meera Nair", "admin@amrita.edu", "$2a$11$SUGWfqpmVZMDq1CflE5wn.REKjlyQQtIghM8H6HES6061SpWh.ObC", "Admin", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3253), "admin@amrita.edu" },
                    { 2, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3255), "Dr. Arjun Menon", "arjun.m@amrita.edu", "$2a$11$SUGWfqpmVZMDq1CflE5wn.REKjlyQQtIghM8H6HES6061SpWh.ObC", "Faculty", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3255), "AVV-F1024" },
                    { 3, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3256), "Dr. Lakshmi Iyer", "lakshmi.i@amrita.edu", "$2a$11$SUGWfqpmVZMDq1CflE5wn.REKjlyQQtIghM8H6HES6061SpWh.ObC", "Faculty", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3256), "AVV-F1038" },
                    { 4, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3257), "Ananya Rao", "ananya.rao@amrita.edu", "$2a$11$SUGWfqpmVZMDq1CflE5wn.REKjlyQQtIghM8H6HES6061SpWh.ObC", "Student", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3257), "CB.EN.U4CSE22101" },
                    { 5, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3258), "Ishaan Nair", "ishaan.nair@amrita.edu", "$2a$11$SUGWfqpmVZMDq1CflE5wn.REKjlyQQtIghM8H6HES6061SpWh.ObC", "Student", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3258), "CB.EN.U4CSE22118" },
                    { 6, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3259), "Diya Menon", "diya.menon@amrita.edu", "$2a$11$SUGWfqpmVZMDq1CflE5wn.REKjlyQQtIghM8H6HES6061SpWh.ObC", "Student", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3259), "CB.EN.U4ECE22206" },
                    { 7, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3260), "Kiran Das", "kiran.das@amrita.edu", "$2a$11$SUGWfqpmVZMDq1CflE5wn.REKjlyQQtIghM8H6HES6061SpWh.ObC", "Student", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3260), "CB.EN.U4MEC22311" },
                    { 8, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3261), "Naina Singh", "naina.singh@amrita.edu", "$2a$11$SUGWfqpmVZMDq1CflE5wn.REKjlyQQtIghM8H6HES6061SpWh.ObC", "Student", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3261), "CB.EN.U4CSE23142" }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "CreatedAt", "IsRead", "Message", "Title", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(4049), false, "AVL Tree Implementation is open for submission.", "Assignment published", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(4049), 4 },
                    { 2, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(4051), false, "Data Structures marks have been published.", "Marks published", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(4051), 4 }
                });

            migrationBuilder.InsertData(
                table: "Staff",
                columns: new[] { "Id", "CreatedAt", "Department", "Designation", "Email", "EmployeeId", "Name", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3714), "Computer Science", "Associate Professor", "arjun.m@amrita.edu", "AVV-F1024", "Dr. Arjun Menon", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3714), 2 },
                    { 2, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3716), "Computer Science", "Professor", "lakshmi.i@amrita.edu", "AVV-F1038", "Dr. Lakshmi Iyer", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3717), 3 }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "BatchId", "CreatedAt", "Department", "Email", "Name", "Program", "Semester", "StudentId", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3812), "Computer Science", "ananya.rao@amrita.edu", "Ananya Rao", "B.Tech CSE", 6, "CB.EN.U4CSE22101", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3812), 4 },
                    { 2, 1, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3815), "Computer Science", "ishaan.nair@amrita.edu", "Ishaan Nair", "B.Tech CSE", 6, "CB.EN.U4CSE22118", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3815), 5 },
                    { 3, 2, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3817), "Electronics", "diya.menon@amrita.edu", "Diya Menon", "B.Tech ECE", 6, "CB.EN.U4ECE22206", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3817), 6 },
                    { 4, 1, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3819), "Mechanical", "kiran.das@amrita.edu", "Kiran Das", "B.Tech ME", 6, "CB.EN.U4MEC22311", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3819), 7 },
                    { 5, 3, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3821), "Computer Science", "naina.singh@amrita.edu", "Naina Singh", "B.Tech CSE", 4, "CB.EN.U4CSE23142", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3821), 8 }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "Code", "CourseId", "CreatedAt", "Credits", "Name", "StaffId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "19CSE301", 1, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3846), 4, "Data Structures & Algorithms", 1, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3846) },
                    { 2, "19CSE312", 1, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3849), 4, "Database Management Systems", 2, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3849) },
                    { 3, "19CSE315", 1, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3850), 3, "Computer Networks", 1, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3851) }
                });

            migrationBuilder.InsertData(
                table: "Assignments",
                columns: new[] { "Id", "BatchId", "CreatedAt", "Description", "DueDate", "MaximumMarks", "Rubric", "StaffId", "SubjectId", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3950), "Implement AVL insert and delete operations.", new DateTime(2026, 10, 20, 23, 59, 0, 0, DateTimeKind.Utc), 100m, null, 1, 1, "AVL Tree Implementation", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3951) },
                    { 2, 1, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3960), "Normalize the supplied schema to 3NF.", new DateTime(2026, 10, 24, 23, 59, 0, 0, DateTimeKind.Utc), 50m, null, 2, 2, "Normalization Case Study", new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3960) }
                });

            migrationBuilder.InsertData(
                table: "Enrollments",
                columns: new[] { "Id", "CreatedAt", "StudentId", "SubjectId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3879), 1, 1, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3879) },
                    { 2, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3880), 1, 2, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3880) },
                    { 3, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3881), 2, 1, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3881) },
                    { 4, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3882), 2, 2, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3882) },
                    { 5, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3883), 5, 1, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3883) },
                    { 6, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3884), 5, 3, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3884) }
                });

            migrationBuilder.InsertData(
                table: "TimetableEntries",
                columns: new[] { "Id", "BatchId", "CreatedAt", "Day", "EndTime", "Room", "StaffId", "StartTime", "SubjectId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3914), "Monday", new TimeOnly(10, 0, 0), "C-301", 1, new TimeOnly(9, 0, 0), 1, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3914) },
                    { 2, 1, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3918), "Tuesday", new TimeOnly(11, 0, 0), "C-302", 2, new TimeOnly(10, 0, 0), 2, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3918) }
                });

            migrationBuilder.InsertData(
                table: "Marks",
                columns: new[] { "Id", "AssignmentId", "CreatedAt", "MarksObtained", "MaximumMarks", "PublishedAt", "Status", "StudentId", "SubjectId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3990), 92m, 100m, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Published", 1, 1, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3990) },
                    { 2, 1, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3996), 81m, 100m, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Published", 2, 1, new DateTime(2026, 10, 7, 6, 40, 28, 134, DateTimeKind.Utc).AddTicks(3996) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_BatchId",
                table: "Assignments",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_StaffId",
                table: "Assignments",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_SubjectId",
                table: "Assignments",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_BatchId",
                table: "AttendanceRecords",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_StudentId_SubjectId_BatchId_Date_Session",
                table: "AttendanceRecords",
                columns: new[] { "StudentId", "SubjectId", "BatchId", "Date", "Session" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_SubjectId",
                table: "AttendanceRecords",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseCode",
                table: "Courses",
                column: "CourseCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_StudentId_SubjectId",
                table: "Enrollments",
                columns: new[] { "StudentId", "SubjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_SubjectId",
                table: "Enrollments",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Examinations_BatchId",
                table: "Examinations",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Examinations_SubjectId",
                table: "Examinations",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryBooks_Isbn",
                table: "LibraryBooks",
                column: "Isbn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LibraryTransactions_LibraryBookId",
                table: "LibraryTransactions",
                column: "LibraryBookId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryTransactions_UserId",
                table: "LibraryTransactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Marks_AssignmentId",
                table: "Marks",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Marks_StudentId",
                table: "Marks",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Marks_SubjectId",
                table: "Marks",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_Email",
                table: "Staff",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Staff_EmployeeId",
                table: "Staff",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Staff_UserId",
                table: "Staff",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Students_BatchId",
                table: "Students",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_Email",
                table: "Students",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentId",
                table: "Students",
                column: "StudentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_Code",
                table: "Subjects",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_CourseId",
                table: "Subjects",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_StaffId",
                table: "Subjects",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_AssignmentId_StudentId",
                table: "Submissions",
                columns: new[] { "AssignmentId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_StudentId",
                table: "Submissions",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_TimetableEntries_BatchId",
                table: "TimetableEntries",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_TimetableEntries_StaffId",
                table: "TimetableEntries",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_TimetableEntries_SubjectId",
                table: "TimetableEntries",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademicCalendarEvents");

            migrationBuilder.DropTable(
                name: "AttendanceRecords");

            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "Examinations");

            migrationBuilder.DropTable(
                name: "LibraryTransactions");

            migrationBuilder.DropTable(
                name: "Marks");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.DropTable(
                name: "TimetableEntries");

            migrationBuilder.DropTable(
                name: "LibraryBooks");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Batches");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
