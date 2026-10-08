using System.ComponentModel.DataAnnotations;

namespace UMS.Api.Models;

public static class Roles
{
    public const string Admin = "Admin";
    public const string Faculty = "Faculty";
    public const string Student = "Student";
}

public abstract class Entity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class User : Entity
{
    [MaxLength(80)] public string Username { get; set; } = "";
    [MaxLength(160)] public string Email { get; set; } = "";
    [MaxLength(40)] public string Role { get; set; } = "";
    public int? RoleMasterId { get; set; }
    public RoleMaster? RoleMaster { get; set; }
    [MaxLength(200)] public string DisplayName { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public bool MustChangePassword { get; set; }
    public Staff? Staff { get; set; }
    public Student? Student { get; set; }
}

public class RoleMaster : Entity
{
    [MaxLength(40)] public string Name { get; set; } = "";
    [MaxLength(300)] public string? Description { get; set; }
}

public class DepartmentMaster : Entity
{
    [MaxLength(100)] public string Name { get; set; } = "";
    [MaxLength(20)] public string Code { get; set; } = "";
}

public class DesignationMaster : Entity
{
    [MaxLength(100)] public string Name { get; set; } = "";
}

public class ProgramMaster : Entity
{
    [MaxLength(100)] public string Name { get; set; } = "";
    [MaxLength(20)] public string Code { get; set; } = "";
}

public class AcademicSession : Entity
{
    [MaxLength(20)] public string AcademicYear { get; set; } = "";
    public int Semester { get; set; }
    public bool IsActive { get; set; } = true;
}

public class Staff : Entity
{
    [MaxLength(40)] public string EmployeeId { get; set; } = "";
    [MaxLength(160)] public string Name { get; set; } = "";
    [MaxLength(160)] public string Email { get; set; } = "";
    [MaxLength(100)] public string Department { get; set; } = "";
    [MaxLength(100)] public string Designation { get; set; } = "";
    public int? UserId { get; set; }
    public User? User { get; set; }
}

public class Student : Entity
{
    [MaxLength(40)] public string StudentId { get; set; } = "";
    [MaxLength(160)] public string Name { get; set; } = "";
    [MaxLength(160)] public string Email { get; set; } = "";
    [MaxLength(100)] public string Department { get; set; } = "";
    [MaxLength(100)] public string Program { get; set; } = "";
    public int? BatchId { get; set; }
    public Batch? Batch { get; set; }
    public int Semester { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
}

public class Course : Entity
{
    [MaxLength(40)] public string CourseCode { get; set; } = "";
    [MaxLength(160)] public string Name { get; set; } = "";
    public int Credits { get; set; }
    [MaxLength(100)] public string Department { get; set; } = "";
    [MaxLength(500)] public string? Description { get; set; }
}

public class Batch : Entity
{
    [MaxLength(80)] public string Name { get; set; } = "";
    [MaxLength(100)] public string Program { get; set; } = "";
    [MaxLength(20)] public string AcademicYear { get; set; } = "";
    public int Semester { get; set; }
}

public class Subject : Entity
{
    [MaxLength(40)] public string Code { get; set; } = "";
    [MaxLength(160)] public string Name { get; set; } = "";
    public int Credits { get; set; }
    public int Semester { get; set; }
    public int? CourseId { get; set; }
    public Course? Course { get; set; }
}

public class SubjectFacultyAllocation : Entity
{
    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;
    public int StaffId { get; set; }
    public Staff Staff { get; set; } = null!;
    public int? BatchId { get; set; }
    public Batch? Batch { get; set; }
    [MaxLength(20)] public string AcademicYear { get; set; } = "";
    public int Semester { get; set; }
}

public class Enrollment : Entity
{
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;
}

public class AttendanceRecord : Entity
{
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;
    public int BatchId { get; set; }
    public Batch Batch { get; set; } = null!;
    public DateOnly Date { get; set; }
    [MaxLength(30)] public string Session { get; set; } = "Lecture";
    [MaxLength(20)] public string Status { get; set; } = "Present";
}

public class Assignment : Entity
{
    [MaxLength(180)] public string Title { get; set; } = "";
    [MaxLength(1000)] public string Description { get; set; } = "";
    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;
    public int BatchId { get; set; }
    public Batch Batch { get; set; } = null!;
    public int StaffId { get; set; }
    public Staff Staff { get; set; } = null!;
    public DateTime DueDate { get; set; }
    public decimal MaximumMarks { get; set; } = 100;
    [MaxLength(1000)] public string? Rubric { get; set; }
}

public class Submission : Entity
{
    public int AssignmentId { get; set; }
    public Assignment Assignment { get; set; } = null!;
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;
    [MaxLength(260)] public string FileName { get; set; } = "";
    [MaxLength(500)] public string FilePath { get; set; } = "";
    [MaxLength(500)] public string? Notes { get; set; }
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
}

public class Mark : Entity
{
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;
    public int? AssignmentId { get; set; }
    public Assignment? Assignment { get; set; }
    public decimal MarksObtained { get; set; }
    public decimal MaximumMarks { get; set; }
    [MaxLength(30)] public string Status { get; set; } = "Draft";
    public DateTime? PublishedAt { get; set; }
}

public class TimetableEntry : Entity
{
    [MaxLength(20)] public string Day { get; set; } = "";
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    [MaxLength(40)] public string Room { get; set; } = "";
    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;
    public int StaffId { get; set; }
    public Staff Staff { get; set; } = null!;
    public int BatchId { get; set; }
    public Batch Batch { get; set; } = null!;
}

public class Examination : Entity
{
    [MaxLength(120)] public string Name { get; set; } = "";
    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;
    public int BatchId { get; set; }
    public Batch Batch { get; set; } = null!;
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    [MaxLength(40)] public string Room { get; set; } = "";
}

public class LibraryBook : Entity
{
    [MaxLength(180)] public string Title { get; set; } = "";
    [MaxLength(160)] public string Author { get; set; } = "";
    [MaxLength(40)] public string Isbn { get; set; } = "";
    [MaxLength(100)] public string Category { get; set; } = "";
    public int TotalCount { get; set; }
    public int AvailableCount { get; set; }
}

public class LibraryTransaction : Entity
{
    public int LibraryBookId { get; set; }
    public LibraryBook LibraryBook { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime IssueDate { get; set; } = DateTime.UtcNow;
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    [MaxLength(30)] public string Status { get; set; } = "Issued";
}

public class Notification : Entity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    [MaxLength(160)] public string Title { get; set; } = "";
    [MaxLength(1000)] public string Message { get; set; } = "";
    public bool IsRead { get; set; }
}

public class AcademicCalendarEvent : Entity
{
    [MaxLength(160)] public string Title { get; set; } = "";
    [MaxLength(1000)] public string Description { get; set; } = "";
    [MaxLength(40)] public string Type { get; set; } = "Academic";
    [MaxLength(120)] public string Location { get; set; } = "";
    public int? SubjectId { get; set; }
    public Subject? Subject { get; set; }
    public int? BatchId { get; set; }
    public Batch? Batch { get; set; }
    public int? StaffId { get; set; }
    public Staff? Staff { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
}

public class StudyResource : Entity
{
    [MaxLength(180)] public string Title { get; set; } = "";
    [MaxLength(1000)] public string Description { get; set; } = "";
    [MaxLength(40)] public string ResourceType { get; set; } = "Notes";
    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;
    public int? BatchId { get; set; }
    public Batch? Batch { get; set; }
    public int StaffId { get; set; }
    public Staff Staff { get; set; } = null!;
    [MaxLength(260)] public string FileName { get; set; } = "";
    [MaxLength(500)] public string FilePath { get; set; } = "";
}

public class FacultyFeedback : Entity
{
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public int StaffId { get; set; }
    public Staff Staff { get; set; } = null!;
    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;
    public int BatchId { get; set; }
    public Batch Batch { get; set; } = null!;
    [Range(1, 5)] public int Rating { get; set; }
    [MaxLength(2000)] public string Comments { get; set; } = "";
    [MaxLength(20)] public string AcademicYear { get; set; } = "";
    public int Semester { get; set; }
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
}

public class EvaluationComponent : Entity
{
    [MaxLength(120)] public string Name { get; set; } = "";
    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;
    public decimal Weightage { get; set; }
    public decimal MaximumMarks { get; set; }
    [MaxLength(30)] public string Status { get; set; } = "Active";
}

public class GradeRule : Entity
{
    [MaxLength(120)] public string Name { get; set; } = "";
    public int? SubjectId { get; set; }
    public Subject? Subject { get; set; }
    public decimal MinimumPercentage { get; set; }
    [MaxLength(10)] public string Grade { get; set; } = "";
    public decimal GradePoint { get; set; }
    [MaxLength(30)] public string Status { get; set; } = "Active";
}

public class GraceMarkCategory : Entity
{
    [MaxLength(120)] public string Name { get; set; } = "";
    [MaxLength(500)] public string Description { get; set; } = "";
    public decimal MaximumMarks { get; set; }
    [MaxLength(30)] public string Status { get; set; } = "Active";
}

public class GraceMarkApplication : Entity
{
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;
    public int GraceMarkCategoryId { get; set; }
    public GraceMarkCategory GraceMarkCategory { get; set; } = null!;
    public decimal MarksAwarded { get; set; }
    [MaxLength(500)] public string Reason { get; set; } = "";
    [MaxLength(30)] public string Status { get; set; } = "Applied";
}

public class Survey : Entity
{
    [MaxLength(160)] public string Title { get; set; } = "";
    [MaxLength(1000)] public string Description { get; set; } = "";
    [MaxLength(30)] public string Audience { get; set; } = "Student";
    [MaxLength(30)] public string Status { get; set; } = "Draft";
    public DateTime? PublishedAt { get; set; }
    public DateTime? ClosesAt { get; set; }
}

public class SurveyResponse : Entity
{
    public int SurveyId { get; set; }
    public Survey Survey { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    [MaxLength(2000)] public string ResponseJson { get; set; } = "";
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
}
