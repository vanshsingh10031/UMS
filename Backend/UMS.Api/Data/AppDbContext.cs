using Microsoft.EntityFrameworkCore;
using UMS.Api.Models;

namespace UMS.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<RoleMaster> RoleMasters => Set<RoleMaster>();
    public DbSet<DepartmentMaster> DepartmentMasters => Set<DepartmentMaster>();
    public DbSet<DesignationMaster> DesignationMasters => Set<DesignationMaster>();
    public DbSet<ProgramMaster> ProgramMasters => Set<ProgramMaster>();
    public DbSet<AcademicSession> AcademicSessions => Set<AcademicSession>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Batch> Batches => Set<Batch>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<SubjectFacultyAllocation> SubjectFacultyAllocations => Set<SubjectFacultyAllocation>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<AttendanceRecord> AttendanceRecords => Set<AttendanceRecord>();
    public DbSet<Assignment> Assignments => Set<Assignment>();
    public DbSet<Submission> Submissions => Set<Submission>();
    public DbSet<Mark> Marks => Set<Mark>();
    public DbSet<TimetableEntry> TimetableEntries => Set<TimetableEntry>();
    public DbSet<Examination> Examinations => Set<Examination>();
    public DbSet<LibraryBook> LibraryBooks => Set<LibraryBook>();
    public DbSet<LibraryTransaction> LibraryTransactions => Set<LibraryTransaction>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<AcademicCalendarEvent> AcademicCalendarEvents => Set<AcademicCalendarEvent>();
    public DbSet<StudyResource> StudyResources => Set<StudyResource>();
    public DbSet<FacultyFeedback> FacultyFeedback => Set<FacultyFeedback>();
    public DbSet<EvaluationComponent> EvaluationComponents => Set<EvaluationComponent>();
    public DbSet<GradeRule> GradeRules => Set<GradeRule>();
    public DbSet<GraceMarkCategory> GraceMarkCategories => Set<GraceMarkCategory>();
    public DbSet<GraceMarkApplication> GraceMarkApplications => Set<GraceMarkApplication>();
    public DbSet<Survey> Surveys => Set<Survey>();
    public DbSet<SurveyResponse> SurveyResponses => Set<SurveyResponse>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();
        modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<RoleMaster>().HasIndex(x => x.Name).IsUnique();
        modelBuilder.Entity<DepartmentMaster>().HasIndex(x => x.Name).IsUnique();
        modelBuilder.Entity<DesignationMaster>().HasIndex(x => x.Name).IsUnique();
        modelBuilder.Entity<ProgramMaster>().HasIndex(x => x.Name).IsUnique();
        modelBuilder.Entity<AcademicSession>().HasIndex(x => new { x.AcademicYear, x.Semester }).IsUnique();
        modelBuilder.Entity<Staff>().HasIndex(x => x.EmployeeId).IsUnique();
        modelBuilder.Entity<Staff>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<Student>().HasIndex(x => x.StudentId).IsUnique();
        modelBuilder.Entity<Student>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<Course>().HasIndex(x => x.CourseCode).IsUnique();
        modelBuilder.Entity<Subject>().HasIndex(x => x.Code).IsUnique();
        modelBuilder.Entity<LibraryBook>().HasIndex(x => x.Isbn).IsUnique();
        modelBuilder.Entity<Enrollment>().HasIndex(x => new { x.StudentId, x.SubjectId }).IsUnique();
        modelBuilder.Entity<AttendanceRecord>().HasIndex(x => new { x.StudentId, x.SubjectId, x.BatchId, x.Date, x.Session }).IsUnique();
        modelBuilder.Entity<Submission>().HasIndex(x => new { x.AssignmentId, x.StudentId }).IsUnique();
        modelBuilder.Entity<SubjectFacultyAllocation>().HasIndex(x => new { x.SubjectId, x.StaffId, x.BatchId, x.AcademicYear, x.Semester }).IsUnique();
        modelBuilder.Entity<EvaluationComponent>().HasIndex(x => new { x.SubjectId, x.Name }).IsUnique();
        modelBuilder.Entity<GradeRule>().HasIndex(x => new { x.SubjectId, x.Name, x.Grade }).IsUnique();
        modelBuilder.Entity<SurveyResponse>().HasIndex(x => new { x.SurveyId, x.UserId }).IsUnique();
        modelBuilder.Entity<StudyResource>().HasIndex(x => new { x.SubjectId, x.BatchId, x.StaffId, x.Title });
        modelBuilder.Entity<FacultyFeedback>().HasIndex(x => new { x.StudentId, x.StaffId, x.SubjectId, x.BatchId, x.Semester }).IsUnique();
        modelBuilder.Entity<Assignment>().Property(x => x.MaximumMarks).HasPrecision(8, 2);
        modelBuilder.Entity<Mark>().Property(x => x.MarksObtained).HasPrecision(8, 2);
        modelBuilder.Entity<Mark>().Property(x => x.MaximumMarks).HasPrecision(8, 2);
        modelBuilder.Entity<EvaluationComponent>().Property(x => x.Weightage).HasPrecision(5, 2);
        modelBuilder.Entity<EvaluationComponent>().Property(x => x.MaximumMarks).HasPrecision(8, 2);
        modelBuilder.Entity<GradeRule>().Property(x => x.MinimumPercentage).HasPrecision(5, 2);
        modelBuilder.Entity<GradeRule>().Property(x => x.GradePoint).HasPrecision(4, 2);
        modelBuilder.Entity<GraceMarkCategory>().Property(x => x.MaximumMarks).HasPrecision(8, 2);
        modelBuilder.Entity<GraceMarkApplication>().Property(x => x.MarksAwarded).HasPrecision(8, 2);

        modelBuilder.Entity<RoleMaster>().HasData(
            new RoleMaster { Id = 1, Name = Roles.Admin, Description = "University administrator" },
            new RoleMaster { Id = 2, Name = Roles.Faculty, Description = "Faculty member" },
            new RoleMaster { Id = 3, Name = Roles.Student, Description = "Student user" }
        );
        modelBuilder.Entity<DepartmentMaster>().HasData(
            new DepartmentMaster { Id = 1, Code = "CSE", Name = "Computer Science" },
            new DepartmentMaster { Id = 2, Code = "ECE", Name = "Electronics" },
            new DepartmentMaster { Id = 3, Code = "MAT", Name = "Mathematics" },
            new DepartmentMaster { Id = 4, Code = "MEC", Name = "Mechanical" },
            new DepartmentMaster { Id = 5, Code = "MGT", Name = "Management" }
        );
        modelBuilder.Entity<DesignationMaster>().HasData(
            new DesignationMaster { Id = 1, Name = "Professor" },
            new DesignationMaster { Id = 2, Name = "Associate Professor" },
            new DesignationMaster { Id = 3, Name = "Assistant Professor" },
            new DesignationMaster { Id = 4, Name = "Administrative Staff" }
        );
        modelBuilder.Entity<ProgramMaster>().HasData(
            new ProgramMaster { Id = 1, Code = "BT-CSE", Name = "B.Tech CSE" },
            new ProgramMaster { Id = 2, Code = "BT-ECE", Name = "B.Tech ECE" },
            new ProgramMaster { Id = 3, Code = "BT-ME", Name = "B.Tech ME" },
            new ProgramMaster { Id = 4, Code = "MBA", Name = "MBA" }
        );
        modelBuilder.Entity<AcademicSession>().HasData(
            new AcademicSession { Id = 1, AcademicYear = "2022-26", Semester = 6 },
            new AcademicSession { Id = 2, AcademicYear = "2023-27", Semester = 4 },
            new AcademicSession { Id = 3, AcademicYear = "2024-28", Semester = 2 }
        );

        modelBuilder.Entity<User>().HasData(
            UserSeed(1, "admin@amrita.edu", "admin@amrita.edu", Roles.Admin, "Dr. Meera Nair", 1),
            UserSeed(2, "AVV-F1024", "arjun.m@amrita.edu", Roles.Faculty, "Dr. Arjun Menon", 2),
            UserSeed(3, "AVV-F1038", "lakshmi.i@amrita.edu", Roles.Faculty, "Dr. Lakshmi Iyer", 2),
            UserSeed(4, "CB.EN.U4CSE22101", "ananya.rao@amrita.edu", Roles.Student, "Ananya Rao", 3),
            UserSeed(5, "CB.EN.U4CSE22118", "ishaan.nair@amrita.edu", Roles.Student, "Ishaan Nair", 3),
            UserSeed(6, "CB.EN.U4ECE22206", "diya.menon@amrita.edu", Roles.Student, "Diya Menon", 3),
            UserSeed(7, "CB.EN.U4MEC22311", "kiran.das@amrita.edu", Roles.Student, "Kiran Das", 3),
            UserSeed(8, "CB.EN.U4CSE23142", "naina.singh@amrita.edu", Roles.Student, "Naina Singh", 3)
        );

        modelBuilder.Entity<Staff>().HasData(
            new Staff { Id = 1, EmployeeId = "AVV-F1024", Name = "Dr. Arjun Menon", Email = "arjun.m@amrita.edu", Department = "Computer Science", Designation = "Associate Professor", UserId = 2 },
            new Staff { Id = 2, EmployeeId = "AVV-F1038", Name = "Dr. Lakshmi Iyer", Email = "lakshmi.i@amrita.edu", Department = "Computer Science", Designation = "Professor", UserId = 3 }
        );
        modelBuilder.Entity<Course>().HasData(
            new Course { Id = 1, CourseCode = "CSE-BT", Name = "B.Tech Computer Science", Credits = 160, Department = "Computer Science", Description = "Undergraduate computer science programme" },
            new Course { Id = 2, CourseCode = "ECE-BT", Name = "B.Tech Electronics & Communication", Credits = 160, Department = "Electronics", Description = "Undergraduate electronics programme" }
        );
        modelBuilder.Entity<Batch>().HasData(
            new Batch { Id = 1, Name = "CSE 2022 - Section A", Program = "B.Tech CSE", AcademicYear = "2022-26", Semester = 6 },
            new Batch { Id = 2, Name = "ECE 2022 - Section A", Program = "B.Tech ECE", AcademicYear = "2022-26", Semester = 6 },
            new Batch { Id = 3, Name = "CSE 2023 - Section A", Program = "B.Tech CSE", AcademicYear = "2023-27", Semester = 4 }
        );
        modelBuilder.Entity<Student>().HasData(
            new Student { Id = 1, StudentId = "CB.EN.U4CSE22101", Name = "Ananya Rao", Email = "ananya.rao@amrita.edu", Department = "Computer Science", Program = "B.Tech CSE", BatchId = 1, Semester = 6, UserId = 4 },
            new Student { Id = 2, StudentId = "CB.EN.U4CSE22118", Name = "Ishaan Nair", Email = "ishaan.nair@amrita.edu", Department = "Computer Science", Program = "B.Tech CSE", BatchId = 1, Semester = 6, UserId = 5 },
            new Student { Id = 3, StudentId = "CB.EN.U4ECE22206", Name = "Diya Menon", Email = "diya.menon@amrita.edu", Department = "Electronics", Program = "B.Tech ECE", BatchId = 2, Semester = 6, UserId = 6 },
            new Student { Id = 4, StudentId = "CB.EN.U4MEC22311", Name = "Kiran Das", Email = "kiran.das@amrita.edu", Department = "Mechanical", Program = "B.Tech ME", BatchId = 1, Semester = 6, UserId = 7 },
            new Student { Id = 5, StudentId = "CB.EN.U4CSE23142", Name = "Naina Singh", Email = "naina.singh@amrita.edu", Department = "Computer Science", Program = "B.Tech CSE", BatchId = 3, Semester = 4, UserId = 8 }
        );
        modelBuilder.Entity<Subject>().HasData(
            new Subject { Id = 1, Code = "19CSE301", Name = "Data Structures & Algorithms", Credits = 4, Semester = 6, CourseId = 1 },
            new Subject { Id = 2, Code = "19CSE312", Name = "Database Management Systems", Credits = 4, Semester = 6, CourseId = 1 },
            new Subject { Id = 3, Code = "19CSE315", Name = "Computer Networks", Credits = 3, Semester = 6, CourseId = 1 }
        );
        modelBuilder.Entity<SubjectFacultyAllocation>().HasData(
            new SubjectFacultyAllocation { Id = 1, SubjectId = 1, StaffId = 1, BatchId = 1, AcademicYear = "2022-26", Semester = 6 },
            new SubjectFacultyAllocation { Id = 2, SubjectId = 2, StaffId = 2, BatchId = 1, AcademicYear = "2022-26", Semester = 6 },
            new SubjectFacultyAllocation { Id = 3, SubjectId = 3, StaffId = 1, BatchId = 1, AcademicYear = "2022-26", Semester = 6 }
        );
        modelBuilder.Entity<Enrollment>().HasData(
            new Enrollment { Id = 1, StudentId = 1, SubjectId = 1 }, new Enrollment { Id = 2, StudentId = 1, SubjectId = 2 },
            new Enrollment { Id = 3, StudentId = 2, SubjectId = 1 }, new Enrollment { Id = 4, StudentId = 2, SubjectId = 2 },
            new Enrollment { Id = 5, StudentId = 5, SubjectId = 1 }, new Enrollment { Id = 6, StudentId = 5, SubjectId = 3 }
        );
        modelBuilder.Entity<TimetableEntry>().HasData(
            new TimetableEntry { Id = 1, Day = "Monday", StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(10, 0), Room = "C-301", SubjectId = 1, StaffId = 1, BatchId = 1 },
            new TimetableEntry { Id = 2, Day = "Tuesday", StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(11, 0), Room = "C-302", SubjectId = 2, StaffId = 2, BatchId = 1 }
        );
        modelBuilder.Entity<Assignment>().HasData(
            new Assignment { Id = 1, Title = "AVL Tree Implementation", Description = "Implement AVL insert and delete operations.", SubjectId = 1, BatchId = 1, StaffId = 1, DueDate = new DateTime(2026, 10, 20, 23, 59, 0, DateTimeKind.Utc), MaximumMarks = 100 },
            new Assignment { Id = 2, Title = "Normalization Case Study", Description = "Normalize the supplied schema to 3NF.", SubjectId = 2, BatchId = 1, StaffId = 2, DueDate = new DateTime(2026, 10, 24, 23, 59, 0, DateTimeKind.Utc), MaximumMarks = 50 }
        );
        modelBuilder.Entity<Mark>().HasData(
            new Mark { Id = 1, StudentId = 1, SubjectId = 1, AssignmentId = 1, MarksObtained = 92, MaximumMarks = 100, Status = "Published", PublishedAt = new DateTime(2026, 10, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Mark { Id = 2, StudentId = 2, SubjectId = 1, AssignmentId = 1, MarksObtained = 81, MaximumMarks = 100, Status = "Published", PublishedAt = new DateTime(2026, 10, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
        modelBuilder.Entity<LibraryBook>().HasData(
            new LibraryBook { Id = 1, Title = "Introduction to Algorithms", Author = "Cormen, Leiserson, Rivest", Isbn = "CS-ALGO-128", Category = "Computer Science", TotalCount = 5, AvailableCount = 4 },
            new LibraryBook { Id = 2, Title = "Designing Data-Intensive Applications", Author = "Martin Kleppmann", Isbn = "CS-DATA-204", Category = "Technology", TotalCount = 3, AvailableCount = 3 }
        );
        modelBuilder.Entity<Notification>().HasData(
            new Notification { Id = 1, UserId = 4, Title = "Assignment published", Message = "AVL Tree Implementation is open for submission." },
            new Notification { Id = 2, UserId = 4, Title = "Marks published", Message = "Data Structures marks have been published." }
        );
        modelBuilder.Entity<EvaluationComponent>().HasData(
            new EvaluationComponent { Id = 1, Name = "Internal Assessment 1", SubjectId = 1, Weightage = 20, MaximumMarks = 50 },
            new EvaluationComponent { Id = 2, Name = "Assignment", SubjectId = 1, Weightage = 20, MaximumMarks = 100 },
            new EvaluationComponent { Id = 3, Name = "End Semester", SubjectId = 1, Weightage = 60, MaximumMarks = 100 }
        );
        modelBuilder.Entity<GradeRule>().HasData(
            new GradeRule { Id = 1, Name = "Default A+", MinimumPercentage = 90, Grade = "A+", GradePoint = 10 },
            new GradeRule { Id = 2, Name = "Default A", MinimumPercentage = 80, Grade = "A", GradePoint = 9 },
            new GradeRule { Id = 3, Name = "Default B+", MinimumPercentage = 70, Grade = "B+", GradePoint = 8 },
            new GradeRule { Id = 4, Name = "Default B", MinimumPercentage = 60, Grade = "B", GradePoint = 7 },
            new GradeRule { Id = 5, Name = "Default C", MinimumPercentage = 50, Grade = "C", GradePoint = 6 }
        );
        modelBuilder.Entity<GraceMarkCategory>().HasData(
            new GraceMarkCategory { Id = 1, Name = "Sports Representation", Description = "Approved sports participation grace marks.", MaximumMarks = 5 },
            new GraceMarkCategory { Id = 2, Name = "Medical Consideration", Description = "Approved medical consideration grace marks.", MaximumMarks = 3 }
        );
        modelBuilder.Entity<GraceMarkApplication>().HasData(
            new GraceMarkApplication { Id = 1, StudentId = 1, SubjectId = 1, GraceMarkCategoryId = 1, MarksAwarded = 2, Reason = "Inter-university sports representation", Status = "Approved" }
        );
        modelBuilder.Entity<Survey>().HasData(
            new Survey { Id = 1, Title = "Course Feedback - Data Structures", Description = "Mid-semester course feedback.", Audience = "Student", Status = "Published", PublishedAt = new DateTime(2026, 10, 1, 0, 0, 0, DateTimeKind.Utc), ClosesAt = new DateTime(2026, 10, 31, 23, 59, 0, DateTimeKind.Utc) }
        );
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        foreach (var entry in ChangeTracker.Entries<Entity>())
        {
            if (entry.State == EntityState.Added) entry.Entity.CreatedAt = now;
            if (entry.State is EntityState.Added or EntityState.Modified) entry.Entity.UpdatedAt = now;
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    private static User UserSeed(int id, string username, string email, string role, string displayName, int roleMasterId) => new()
    {
        Id = id,
        Username = username,
        Email = email,
        Role = role,
        RoleMasterId = roleMasterId,
        DisplayName = displayName,
        PasswordHash = "amrita2026",
        MustChangePassword = role is Roles.Faculty or Roles.Student
    };
}
