using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UMS.Api.Data;
using UMS.Api.Models;

namespace UMS.Api.Controllers;

[ApiController]
[Route("api")]
[Authorize]
public class AcademicController(AppDbContext db, IConfiguration config) : ControllerBase
{
    [HttpGet("dashboard/{role}")]
    public async Task<object> Dashboard(string role)
    {
        if (role == "admin") return new { totalStudents = await db.Students.CountAsync(), totalFaculty = await db.Staff.CountAsync(), totalCourses = await db.Courses.CountAsync(), totalSubjects = await db.Subjects.CountAsync(), activeBatches = await db.Batches.CountAsync(), averageAttendance = 90.5 };
        if (role == "faculty")
        {
            var staffId = await CurrentStaffIdOrNull();
            var staff = staffId is null ? null : await db.Staff.AsNoTracking().FirstOrDefaultAsync(x => x.Id == staffId);
            var today = DateTime.Today.DayOfWeek.ToString();
            var subjectIds = await db.SubjectFacultyAllocations.Where(x => staffId == null || x.StaffId == staffId).Select(x => x.SubjectId).Distinct().ToListAsync();
            var batchIds = await db.SubjectFacultyAllocations.Where(x => staffId == null || x.StaffId == staffId).Where(x => x.BatchId != null).Select(x => x.BatchId!.Value).Distinct().ToListAsync();
            var todays = await db.TimetableEntries.AsNoTracking().Include(x => x.Subject).Include(x => x.Batch)
                .Where(x => (staffId == null || x.StaffId == staffId) && x.Day == today)
                .OrderBy(x => x.StartTime)
                .Select(x => new { time = x.StartTime.ToString("HH\\:mm"), title = x.Subject.Name, location = x.Room + " · " + x.Batch.Name, status = x.StartTime.ToTimeSpan() < DateTime.Now.TimeOfDay && x.EndTime.ToTimeSpan() < DateTime.Now.TimeOfDay ? "Completed" : x.StartTime.ToTimeSpan() <= DateTime.Now.TimeOfDay && x.EndTime.ToTimeSpan() >= DateTime.Now.TimeOfDay ? "In progress" : "Upcoming" })
                .ToListAsync();
            var studentCount = await db.Students.CountAsync(x => x.BatchId != null && batchIds.Contains(x.BatchId.Value));
            var pendingTasks = await db.Assignments.CountAsync(x => staffId == null || x.StaffId == staffId);
            var facultyAttendanceRows = await db.AttendanceRecords.AsNoTracking().Where(x => subjectIds.Contains(x.SubjectId) && batchIds.Contains(x.BatchId)).ToListAsync();
            var facultyAttendancePercentage = facultyAttendanceRows.Count == 0 ? 0 : Math.Round(facultyAttendanceRows.Count(x => x.Status is "Present" or "OD") * 100m / facultyAttendanceRows.Count, 1);
            var nextClass = todays.FirstOrDefault(x => x.status is "Upcoming" or "In progress")?.time ?? "No class today";
            var recent = await db.Assignments.AsNoTracking().Include(x => x.Subject)
                .Where(x => staffId == null || x.StaffId == staffId)
                .OrderByDescending(x => x.CreatedAt)
                .Take(3)
                .Select(x => new { title = x.Title, owner = x.Subject.Name, time = x.DueDate.ToString("dd MMM") })
                .ToListAsync();
            return new { faculty = staff is null ? null : new { staff.EmployeeId, staff.Name, staff.Department, staff.Designation }, assignedSubjects = subjectIds.Count, todaysClasses = todays.Count, myStudents = studentCount, pendingTasks, schedule = todays, recentActivity = recent, attendancePercentage = facultyAttendancePercentage, nextClass, assignedBatches = batchIds.Count };
        }
        var studentId = await CurrentStudentId();
        var student = await db.Students.AsNoTracking().FirstAsync(x => x.Id == studentId);
        var attendanceRows = await db.AttendanceRecords.AsNoTracking().Where(x => x.StudentId == studentId).ToListAsync();
        var attendancePercentage = attendanceRows.Count == 0 ? 0 : Math.Round(attendanceRows.Count(x => x.Status is "Present" or "OD") * 100m / attendanceRows.Count, 1);
        var upcomingAssignments = await db.Assignments.CountAsync(x => x.BatchId == student.BatchId && x.DueDate >= DateTime.Now);
        var upcomingExams = await db.Examinations.CountAsync(x => x.BatchId == student.BatchId && x.StartsAt >= DateTime.Now);
        return new { currentSemester = student.Semester, enrolledSubjects = await db.Enrollments.CountAsync(x => x.StudentId == studentId), attendancePercentage, upcomingAssignments, upcomingExams, unreadNotifications = await db.Notifications.CountAsync(x => x.UserId == CurrentUserId() && !x.IsRead) };
    }

    [HttpGet("attendance/summary")]
    public async Task<object> AttendanceSummary(int? studentId = null)
    {
        if (User.IsInRole(Roles.Faculty) && studentId is null)
        {
            var staffId = await CurrentStaffIdOrNull();
            if (staffId is null) return Array.Empty<object>();
            var allowed = await db.SubjectFacultyAllocations.AsNoTracking()
                .Where(x => x.StaffId == staffId && x.BatchId != null)
                .Select(x => new { x.SubjectId, BatchId = x.BatchId!.Value })
                .ToListAsync();
            var subjectIds = allowed.Select(x => x.SubjectId).Distinct().ToList();
            var batchIds = allowed.Select(x => x.BatchId).Distinct().ToList();
            var scopedRows = await db.AttendanceRecords.AsNoTracking().Include(x => x.Subject)
                .Where(x => subjectIds.Contains(x.SubjectId) && batchIds.Contains(x.BatchId))
                .ToListAsync();
            return scopedRows.GroupBy(x => x.Subject.Name).Select(g =>
            {
                var total = g.Count();
                var attended = g.Count(x => x.Status is "Present" or "OD");
                return new { subject = g.Key, totalClasses = total, attendedClasses = attended, absentClasses = g.Count(x => x.Status == "Absent"), odClasses = g.Count(x => x.Status == "OD"), percentage = total == 0 ? 0 : Math.Round(attended * 100m / total, 2), warning = false };
            });
        }
        var sid = studentId ?? await CurrentStudentId();
        var rows = await db.AttendanceRecords.AsNoTracking().Include(x => x.Subject).Where(x => x.StudentId == sid).ToListAsync();
        return rows.GroupBy(x => x.Subject.Name).Select(g =>
        {
            var total = g.Count();
            var attended = g.Count(x => x.Status is "Present" or "OD");
            return new { subject = g.Key, totalClasses = total, attendedClasses = attended, absentClasses = g.Count(x => x.Status == "Absent"), odClasses = g.Count(x => x.Status == "OD"), percentage = total == 0 ? 0 : Math.Round(attended * 100m / total, 2), warning = total > 0 && attended * 100m / total < 75 };
        });
    }

    public record AttendanceSubmit(int SubjectId, int BatchId, DateOnly Date, string Session, List<AttendanceLine> Records);
    public record AttendanceLine(int StudentId, string Status);

    [HttpGet("faculty/subjects"), Authorize(Roles = Roles.Faculty + "," + Roles.Admin)]
    public async Task<object> FacultySubjects()
    {
        var staffId = await CurrentStaffIdOrNull();
        var query = db.SubjectFacultyAllocations.AsNoTracking().Include(x => x.Subject).Include(x => x.Batch).AsQueryable();
        if (staffId is not null) query = query.Where(x => x.StaffId == staffId);
        return await query
            .GroupBy(x => new { x.Subject.Id, x.Subject.Code, x.Subject.Name, x.Subject.Credits })
            .Select(g => new { id = g.Key.Code, key = g.Key.Id, subjectId = g.Key.Id, name = g.Key.Name, detail = g.Key.Credits + " credits", batches = g.Select(x => x.Batch != null ? x.Batch.Name : "All batches").Distinct().Count(), status = "Allocated" })
            .ToListAsync();
    }

    [HttpGet("faculty/timetable"), Authorize(Roles = Roles.Faculty + "," + Roles.Admin)]
    public async Task<object> FacultyTimetable()
    {
        var staffId = await CurrentStaffIdOrNull();
        var query = db.TimetableEntries.AsNoTracking().Include(x => x.Subject).Include(x => x.Staff).Include(x => x.Batch).AsQueryable();
        if (staffId is not null) query = query.Where(x => x.StaffId == staffId);
        return await query.OrderBy(x => x.Day).ThenBy(x => x.StartTime)
            .Select(x => new { x.Id, x.Day, x.StartTime, x.EndTime, x.Room, x.SubjectId, x.StaffId, x.BatchId, subject = x.Subject.Name, faculty = x.Staff.Name, batch = x.Batch.Name, mine = staffId == null || x.StaffId == staffId })
            .ToListAsync();
    }

    [HttpGet("faculty/batches"), Authorize(Roles = Roles.Faculty + "," + Roles.Admin)]
    public async Task<object> FacultyBatches(int? subjectId = null)
    {
        var staffId = await CurrentStaffIdOrNull();
        var query = db.SubjectFacultyAllocations.AsNoTracking().Include(x => x.Subject).Include(x => x.Batch).AsQueryable();
        if (staffId is not null) query = query.Where(x => x.StaffId == staffId);
        if (subjectId is not null) query = query.Where(x => x.SubjectId == subjectId);
        return await query.Where(x => x.BatchId != null)
            .Select(x => new { id = x.BatchId, batchId = x.BatchId, name = x.Batch!.Name, program = x.Batch.Program, academicYear = x.Batch.AcademicYear, x.Semester, subjectId = x.SubjectId, subject = x.Subject.Code + " - " + x.Subject.Name })
            .Distinct()
            .ToListAsync();
    }

    [HttpGet("faculty/students"), Authorize(Roles = Roles.Faculty + "," + Roles.Admin)]
    public async Task<object> FacultyStudents(int? subjectId = null, int? batchId = null)
    {
        var staffId = await CurrentStaffIdOrNull();
        var allocations = db.SubjectFacultyAllocations.AsNoTracking().AsQueryable();
        if (staffId is not null) allocations = allocations.Where(x => x.StaffId == staffId);
        if (subjectId is not null) allocations = allocations.Where(x => x.SubjectId == subjectId);
        if (batchId is not null) allocations = allocations.Where(x => x.BatchId == batchId);
        var allowedBatchIds = await allocations.Where(x => x.BatchId != null).Select(x => x.BatchId!.Value).Distinct().ToListAsync();
        return await db.Students.AsNoTracking().Include(x => x.Batch)
            .Where(x => x.BatchId != null && allowedBatchIds.Contains(x.BatchId.Value))
            .OrderBy(x => x.StudentId)
            .Select(x => new { id = x.Id, x.StudentId, name = x.Name, email = x.Email, department = x.Department, program = x.Program, batch = x.Batch!.Name, semester = x.Semester, status = "Active" })
            .ToListAsync();
    }

    [HttpGet("attendance/slots"), Authorize(Roles = Roles.Faculty + "," + Roles.Admin)]
    public async Task<object> AttendanceSlots(int subjectId, int batchId, int year, int month)
    {
        var staffId = await CurrentStaffIdOrNull();
        var query = db.TimetableEntries.AsNoTracking().Include(x => x.Subject).Include(x => x.Batch).Where(x => x.SubjectId == subjectId && x.BatchId == batchId);
        if (staffId is not null) query = query.Where(x => x.StaffId == staffId);
        var timetable = await query.ToListAsync();
        var today = DateOnly.FromDateTime(DateTime.Now);
        var days = Enumerable.Range(1, DateTime.DaysInMonth(year, month)).Select(day => new DateOnly(year, month, day)).Where(date => date <= today).ToList();
        var slots = new List<AttendanceSlotDto>();
        foreach (var entry in timetable)
        {
            foreach (var date in days.Where(date => date.DayOfWeek.ToString() == entry.Day))
            {
                var session = $"{entry.StartTime:HH\\:mm}-{entry.EndTime:HH\\:mm}";
                var submitted = await db.AttendanceRecords.AnyAsync(x => x.SubjectId == subjectId && x.BatchId == batchId && x.Date == date && x.Session == session);
                slots.Add(new AttendanceSlotDto($"{entry.Id}-{date:yyyyMMdd}", entry.Id, date, session, $"{date:dd MMM yyyy} · {session} · {entry.Subject.Name} · {entry.Batch.Name}", date > today, submitted));
            }
        }
        return slots.OrderBy(x => x.Date).ThenBy(x => x.Session).ToList();
    }

    public record AttendanceSlotDto(string Id, int TimetableId, DateOnly Date, string Session, string Label, bool Disabled, bool Submitted);

    [HttpGet("attendance/roster"), Authorize(Roles = Roles.Faculty + "," + Roles.Admin)]
    public async Task<object> AttendanceRoster(int batchId, int? subjectId = null, DateOnly? date = null, string? session = null)
    {
        if (User.IsInRole(Roles.Faculty) && subjectId is not null && !await FacultyCanAccess(subjectId.Value, batchId)) return Forbid();
        return await db.Students.AsNoTracking()
        .Where(x => x.BatchId == batchId)
        .OrderBy(x => x.StudentId)
        .Select(x => new { x.Id, x.StudentId, x.Name, status = date != null && session != null ? db.AttendanceRecords.Where(a => a.StudentId == x.Id && a.SubjectId == subjectId && a.BatchId == batchId && a.Date == date && a.Session == session).Select(a => a.Status).FirstOrDefault() ?? "Present" : "Present" })
        .ToListAsync();
    }

    [HttpPost("attendance"), Authorize(Roles = Roles.Faculty + "," + Roles.Admin)]
    public async Task<IActionResult> SubmitAttendance(AttendanceSubmit request)
    {
        if (User.IsInRole(Roles.Faculty) && !await FacultyCanAccess(request.SubjectId, request.BatchId)) return Forbid();
        foreach (var line in request.Records)
        {
            if (!new[] { "Present", "Absent", "OD" }.Contains(line.Status)) return BadRequest(new { message = "Invalid attendance status." });
            var existing = await db.AttendanceRecords.FirstOrDefaultAsync(x => x.StudentId == line.StudentId && x.SubjectId == request.SubjectId && x.BatchId == request.BatchId && x.Date == request.Date && x.Session == request.Session);
            if (existing is null) db.AttendanceRecords.Add(new AttendanceRecord { StudentId = line.StudentId, SubjectId = request.SubjectId, BatchId = request.BatchId, Date = request.Date, Session = request.Session, Status = line.Status });
            else existing.Status = line.Status;
        }
        await db.SaveChangesAsync();
        return Ok(new { message = "Attendance saved." });
    }

    [HttpGet("timetable")]
    public async Task<object> Timetable()
    {
        var query = db.TimetableEntries.AsNoTracking().Include(x => x.Subject).Include(x => x.Staff).Include(x => x.Batch).AsQueryable();
        if (User.IsInRole(Roles.Student))
        {
            var studentId = await CurrentStudentId();
            var batchId = await db.Students.Where(x => x.Id == studentId).Select(x => x.BatchId).FirstAsync();
            query = query.Where(x => x.BatchId == batchId);
        }
        if (User.IsInRole(Roles.Faculty))
        {
            var staffId = await CurrentStaffIdOrNull();
            query = query.Where(x => x.StaffId == staffId);
        }
        return await query.Select(x => new { x.Id, x.Day, x.StartTime, x.EndTime, x.Room, x.SubjectId, x.StaffId, x.BatchId, subject = x.Subject.Name, faculty = x.Staff.Name, batch = x.Batch.Name }).ToListAsync();
    }

    [HttpGet("student/calendar"), Authorize(Roles = Roles.Student + "," + Roles.Admin + "," + Roles.Faculty)]
    public async Task<object> StudentCalendar()
    {
        var student = await db.Students.AsNoTracking().Include(x => x.Batch).FirstOrDefaultAsync(x => x.UserId == CurrentUserId())
            ?? await db.Students.AsNoTracking().Include(x => x.Batch).FirstAsync();
        var today = DateTime.Today;
        var horizon = today.AddDays(45);
        var events = new List<StudentCalendarEvent>();

        var timetable = await db.TimetableEntries.AsNoTracking().Include(x => x.Subject).Include(x => x.Staff).Include(x => x.Batch)
            .Where(x => x.BatchId == student.BatchId)
            .ToListAsync();
        foreach (var slot in timetable)
        {
            foreach (var date in Enumerable.Range(0, 28).Select(offset => today.AddDays(offset)).Where(date => date.DayOfWeek.ToString() == slot.Day))
            {
                var start = date.Date.Add(slot.StartTime.ToTimeSpan());
                var end = date.Date.Add(slot.EndTime.ToTimeSpan());
                events.Add(new StudentCalendarEvent(
                    $"class-{slot.Id}-{date:yyyyMMdd}",
                    "Class",
                    slot.Subject.Name,
                    start,
                    end,
                    slot.Room,
                    slot.Staff.Name,
                    $"Scheduled class for {slot.Subject.Code}. Faculty: {slot.Staff.Name}. Batch: {slot.Batch.Name}.",
                    slot.Subject.Name,
                    "Class"
                ));
            }
        }

        var assignments = await db.Assignments.AsNoTracking().Include(x => x.Subject).Include(x => x.Staff).Include(x => x.Batch)
            .Where(x => x.BatchId == student.BatchId && x.DueDate >= today.AddDays(-7) && x.DueDate <= horizon)
            .ToListAsync();
        events.AddRange(assignments.Select(x => new StudentCalendarEvent(
            $"assignment-{x.Id}",
            "Assignment",
            x.Title,
            x.DueDate,
            x.DueDate,
            "Online submission",
            x.Staff.Name,
            $"{x.Description} Maximum marks: {x.MaximumMarks}. Subject: {x.Subject.Name}.",
            x.Subject.Name,
            x.DueDate.Date < today ? "Overdue" : x.DueDate.Date == today ? "Due today" : "Upcoming"
        )));

        var exams = await db.Examinations.AsNoTracking().Include(x => x.Subject).Include(x => x.Batch)
            .Where(x => x.BatchId == student.BatchId && x.StartsAt >= today.AddDays(-7) && x.StartsAt <= horizon)
            .ToListAsync();
        events.AddRange(exams.Select(x => new StudentCalendarEvent(
            $"exam-{x.Id}",
            x.Name.Contains("quiz", StringComparison.OrdinalIgnoreCase) ? "Quiz" : "Exam",
            x.Name,
            x.StartsAt,
            x.EndsAt,
            x.Room,
            "Examination cell",
            $"Assessment for {x.Subject.Name}. Starts {x.StartsAt:dd MMM yyyy HH:mm} and ends {x.EndsAt:HH:mm}.",
            x.Subject.Name,
            x.StartsAt.Date < today ? "Completed" : x.StartsAt.Date == today ? "Today" : "Scheduled"
        )));

        var components = await db.EvaluationComponents.AsNoTracking().Include(x => x.Subject)
            .Where(x => x.Status == "Active" && timetable.Select(t => t.SubjectId).Contains(x.SubjectId))
            .ToListAsync();
        var nextMonday = today.AddDays(((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7);
        events.AddRange(components.Select((x, index) => new StudentCalendarEvent(
            $"component-{x.Id}",
            x.Name.Contains("quiz", StringComparison.OrdinalIgnoreCase) ? "Quiz" : "Assessment",
            x.Name,
            nextMonday.AddDays(index % 10).AddHours(9 + index % 5),
            nextMonday.AddDays(index % 10).AddHours(10 + index % 5),
            "To be announced",
            "Faculty",
            $"{x.Weightage}% weightage, {x.MaximumMarks} marks. This is part of the internal assessment plan for {x.Subject.Name}.",
            x.Subject.Name,
            "Planned"
        )));

        var facultyEvents = await db.AcademicCalendarEvents.AsNoTracking().Include(x => x.Subject).Include(x => x.Batch).Include(x => x.Staff)
            .Where(x => x.StartsAt >= today.AddDays(-7) && x.StartsAt <= horizon && (x.BatchId == null || x.BatchId == student.BatchId))
            .ToListAsync();
        events.AddRange(facultyEvents.Select(x => new StudentCalendarEvent(
            $"calendar-{x.Id}",
            x.Type,
            x.Title,
            x.StartsAt,
            x.EndsAt,
            string.IsNullOrWhiteSpace(x.Location) ? "To be announced" : x.Location,
            x.Staff != null ? x.Staff.Name : "Academic team",
            x.Description,
            x.Subject != null ? x.Subject.Name : "General",
            x.StartsAt.Date < today ? "Completed" : x.StartsAt.Date == today ? "Today" : "Scheduled"
        )));

        return new
        {
            student = new { student.StudentId, student.Name, batch = student.Batch?.Name, student.Semester },
            window = new { from = today, to = horizon },
            events = events.OrderBy(x => x.StartsAt).ThenBy(x => x.Type).ToList(),
            summary = events.GroupBy(x => x.Type).Select(g => new { type = g.Key, count = g.Count() }).OrderBy(x => x.type).ToList()
        };
    }

    public record StudentCalendarEvent(string Id, string Type, string Title, DateTime StartsAt, DateTime EndsAt, string Location, string Owner, string Description, string Subject, string Status);

    [HttpGet("calendar-events"), Authorize(Roles = Roles.Faculty + "," + Roles.Admin)]
    public async Task<object> CalendarEvents()
    {
        var staffId = await CurrentStaffIdOrNull();
        return await db.AcademicCalendarEvents.AsNoTracking().Include(x => x.Subject).Include(x => x.Batch).Include(x => x.Staff)
            .Where(x => User.IsInRole(Roles.Admin) || x.StaffId == staffId)
            .OrderByDescending(x => x.StartsAt)
            .Select(x => new { x.Id, x.Title, x.Description, x.Type, x.Location, x.SubjectId, subject = x.Subject != null ? x.Subject.Name : "General", x.BatchId, batch = x.Batch != null ? x.Batch.Name : "All assigned batches", x.StartsAt, x.EndsAt, owner = x.Staff != null ? x.Staff.Name : "Academic team" })
            .ToListAsync();
    }

    public record CalendarEventRequest(string Title, string Description, string Type, string Location, int? SubjectId, int? BatchId, DateTime StartsAt, DateTime EndsAt);

    [HttpPost("calendar-events"), Authorize(Roles = Roles.Faculty + "," + Roles.Admin)]
    public async Task<IActionResult> CreateCalendarEvent(CalendarEventRequest request)
    {
        var staffId = await CurrentStaffIdOrNull();
        if (request.SubjectId is not null && request.BatchId is not null && User.IsInRole(Roles.Faculty) && !await FacultyCanAccess(request.SubjectId.Value, request.BatchId.Value)) return Forbid();
        if (string.IsNullOrWhiteSpace(request.Title)) return BadRequest(new { message = "Title is required." });
        if (request.EndsAt < request.StartsAt) return BadRequest(new { message = "End time must be after start time." });
        var input = new AcademicCalendarEvent
        {
            Title = request.Title,
            Description = request.Description,
            Type = request.Type,
            Location = request.Location,
            SubjectId = request.SubjectId,
            BatchId = request.BatchId,
            StaffId = staffId,
            StartsAt = request.StartsAt,
            EndsAt = request.EndsAt
        };
        db.AcademicCalendarEvents.Add(input);
        await db.SaveChangesAsync();
        return Created($"api/calendar-events/{input.Id}", new
        {
            input.Id,
            input.Title,
            input.Description,
            input.Type,
            input.Location,
            input.SubjectId,
            subject = "General",
            input.BatchId,
            batch = input.BatchId is null ? "All assigned batches" : "",
            input.StartsAt,
            input.EndsAt,
            owner = "Academic team"
        });
    }

    public record TimetableRequest(string Day, TimeOnly StartTime, TimeOnly EndTime, string Room, int SubjectId, int StaffId, int BatchId);

    [HttpPost("timetable"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> CreateTimetable(TimetableRequest request)
    {
        var input = new TimetableEntry { Day = request.Day, StartTime = request.StartTime, EndTime = request.EndTime, Room = request.Room, SubjectId = request.SubjectId, StaffId = request.StaffId, BatchId = request.BatchId };
        var conflict = await db.TimetableEntries.AnyAsync(x => x.Day == input.Day && x.Id != input.Id && input.StartTime < x.EndTime && input.EndTime > x.StartTime && (x.Room == input.Room || x.StaffId == input.StaffId || x.BatchId == input.BatchId));
        if (conflict) return Conflict(new { message = "Timetable conflict for room, faculty, or batch." });
        db.TimetableEntries.Add(input); await db.SaveChangesAsync(); return Created($"api/timetable/{input.Id}", input);
    }

    [HttpPut("timetable/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateTimetable(int id, TimetableRequest request)
    {
        var row = await db.TimetableEntries.FindAsync(id);
        if (row is null) return NotFound();
        var conflict = await db.TimetableEntries.AnyAsync(x => x.Id != id && x.Day == request.Day && request.StartTime < x.EndTime && request.EndTime > x.StartTime && (x.Room == request.Room || x.StaffId == request.StaffId || x.BatchId == request.BatchId));
        if (conflict) return Conflict(new { message = "Timetable conflict for room, faculty, or batch." });
        row.Day = request.Day; row.StartTime = request.StartTime; row.EndTime = request.EndTime; row.Room = request.Room; row.SubjectId = request.SubjectId; row.StaffId = request.StaffId; row.BatchId = request.BatchId;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("timetable/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteTimetable(int id)
    {
        var row = await db.TimetableEntries.FindAsync(id);
        if (row is null) return NotFound();
        db.Remove(row);
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("assignments")]
    public async Task<object> Assignments()
    {
        var currentStudentId = User.IsInRole(Roles.Student) ? await CurrentStudentId() : (int?)null;
        var query = db.Assignments.AsNoTracking().Include(x => x.Subject).Include(x => x.Batch).Include(x => x.Staff).AsQueryable();
        if (User.IsInRole(Roles.Faculty))
        {
            var staffId = await CurrentStaffIdOrNull();
            query = query.Where(x => x.StaffId == staffId);
        }
        if (User.IsInRole(Roles.Student))
        {
            var studentId = await CurrentStudentId();
            var batchId = await db.Students.Where(x => x.Id == studentId).Select(x => x.BatchId).FirstAsync();
            query = query.Where(x => x.BatchId == batchId);
        }
        return await query.Select(x => new
        {
            x.Id,
            x.Title,
            x.Description,
            subject = x.Subject.Name,
            subjectId = x.SubjectId,
            batch = x.Batch.Name,
            batchId = x.BatchId,
            staffId = x.StaffId,
            faculty = x.Staff.Name,
            due = x.DueDate,
            progress = User.IsInRole(Roles.Faculty) || User.IsInRole(Roles.Admin)
                ? db.Students.Count(s => s.BatchId == x.BatchId) == 0 ? 0 : Math.Round(db.Submissions.Count(s => s.AssignmentId == x.Id) * 100m / db.Students.Count(s => s.BatchId == x.BatchId), 0)
                : db.Submissions.Any(s => s.AssignmentId == x.Id && s.StudentId == currentStudentId) ? 100 : 0,
            status = User.IsInRole(Roles.Student)
                ? db.Submissions.Any(s => s.AssignmentId == x.Id && s.StudentId == currentStudentId) ? "Submitted" : x.DueDate < DateTime.UtcNow ? "Overdue" : "Pending"
                : x.DueDate < DateTime.UtcNow ? "Closed" : "In progress",
            submissions = db.Submissions.Count(s => s.AssignmentId == x.Id),
            x.MaximumMarks,
            x.Rubric
        }).ToListAsync();
    }

    public record AssignmentRequest(string Title, string Description, int SubjectId, int BatchId, int StaffId, DateTime DueDate, decimal MaximumMarks, string? Rubric);

    [HttpPost("assignments"), Authorize(Roles = Roles.Faculty + "," + Roles.Admin)]
    public async Task<ActionResult<Assignment>> CreateAssignment(AssignmentRequest request)
    {
        var staffId = User.IsInRole(Roles.Faculty) ? await CurrentStaffIdOrNull() : request.StaffId;
        if (staffId is null) return BadRequest(new { message = "Faculty profile is not linked." });
        if (User.IsInRole(Roles.Faculty) && !await FacultyCanAccess(request.SubjectId, request.BatchId)) return Forbid();
        var input = new Assignment
        {
            Title = request.Title,
            Description = request.Description,
            SubjectId = request.SubjectId,
            BatchId = request.BatchId,
            StaffId = staffId.Value,
            DueDate = request.DueDate,
            MaximumMarks = request.MaximumMarks,
            Rubric = request.Rubric
        };
        db.Assignments.Add(input); await db.SaveChangesAsync();
        var students = await db.Students.Where(x => x.BatchId == input.BatchId && x.UserId != null).ToListAsync();
        foreach (var student in students) db.Notifications.Add(new Notification { UserId = student.UserId!.Value, Title = "New assignment", Message = input.Title });
        await db.SaveChangesAsync();
        return Created($"api/assignments/{input.Id}", input);
    }

    [HttpGet("assignments/{id:int}/submissions"), Authorize(Roles = Roles.Faculty + "," + Roles.Admin)]
    public async Task<IActionResult> AssignmentSubmissions(int id)
    {
        var assignment = await db.Assignments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (assignment is null) return NotFound();
        if (User.IsInRole(Roles.Faculty))
        {
            var staffId = await CurrentStaffIdOrNull();
            if (assignment.StaffId != staffId) return Forbid();
        }
        var rows = await db.Submissions.AsNoTracking().Include(x => x.Student)
            .Where(x => x.AssignmentId == id)
            .OrderByDescending(x => x.SubmittedAt)
            .Select(x => new { x.Id, studentId = x.Student.StudentId, student = x.Student.Name, x.FileName, x.Notes, x.SubmittedAt })
            .ToListAsync();
        return Ok(rows);
    }

    [HttpPost("assignments/{id:int}/submit"), Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> SubmitAssignment(int id, IFormFile file, [FromForm] string? notes)
    {
        if (file.Length > config.GetValue<long>("Uploads:MaxBytes")) return BadRequest(new { message = "File is too large." });
        var studentId = await CurrentStudentId();
        var studentBatchId = await db.Students.Where(x => x.Id == studentId).Select(x => x.BatchId).FirstAsync();
        var assignment = await db.Assignments.FirstOrDefaultAsync(x => x.Id == id);
        if (assignment is null) return NotFound();
        if (assignment.BatchId != studentBatchId) return Forbid();
        var dir = Path.Combine(AppContext.BaseDirectory, config["Uploads:Directory"] ?? "uploads");
        Directory.CreateDirectory(dir);
        var stored = $"{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}";
        await using var stream = System.IO.File.Create(Path.Combine(dir, stored));
        await file.CopyToAsync(stream);
        var existing = await db.Submissions.FirstOrDefaultAsync(x => x.AssignmentId == id && x.StudentId == studentId);
        if (existing is null) db.Submissions.Add(new Submission { AssignmentId = id, StudentId = studentId, FileName = file.FileName, FilePath = stored, Notes = notes });
        else { existing.FileName = file.FileName; existing.FilePath = stored; existing.Notes = notes; existing.SubmittedAt = DateTime.UtcNow; }
        await db.SaveChangesAsync();
        return Ok(new { message = "Submitted." });
    }

    [HttpGet("attendance/recent")]
    public async Task<object> RecentAttendance()
    {
        var query = db.AttendanceRecords.AsNoTracking().Include(x => x.Subject).Include(x => x.Batch).Include(x => x.Student).AsQueryable();
        if (User.IsInRole(Roles.Student))
        {
            var studentId = await CurrentStudentId();
            query = query.Where(x => x.StudentId == studentId);
        }
        if (User.IsInRole(Roles.Faculty))
        {
            var staffId = await CurrentStaffIdOrNull();
            query = query.Where(x => db.SubjectFacultyAllocations.Any(a => a.StaffId == staffId && a.SubjectId == x.SubjectId && a.BatchId == x.BatchId));
        }
        return await query.OrderByDescending(x => x.Date).ThenByDescending(x => x.Session).Take(10)
            .Select(x => new { x.Id, date = x.Date.ToString("dd MMM yyyy"), subject = x.Subject.Name, student = x.Student.Name, batch = x.Batch.Name, x.Session, x.Status })
            .ToListAsync();
    }

    [HttpGet("marks")]
    public async Task<object> Marks()
    {
        var query = db.Marks.AsNoTracking().Include(x => x.Student).Include(x => x.Subject).AsQueryable();
        if (User.IsInRole(Roles.Student))
        {
            var studentId = await CurrentStudentId();
            query = query.Where(x => x.StudentId == studentId && x.Status == "Published");
        }
        if (User.IsInRole(Roles.Faculty))
        {
            var staffId = await CurrentStaffIdOrNull();
            query = query.Where(x => db.SubjectFacultyAllocations.Any(a => a.StaffId == staffId && a.SubjectId == x.SubjectId && a.BatchId == x.Student.BatchId));
        }
        return await query.Select(x => new { x.Id, studentId = x.StudentId, subjectId = x.SubjectId, student = x.Student.Name, subject = x.Subject.Name, x.MarksObtained, x.MaximumMarks, x.Status, x.PublishedAt }).ToListAsync();
    }

    public record MarkRequest(int StudentId, int SubjectId, int? AssignmentId, decimal MarksObtained, decimal MaximumMarks, string Status);

    [HttpPost("marks"), Authorize(Roles = Roles.Faculty + "," + Roles.Admin)]
    public async Task<IActionResult> SaveMark(MarkRequest request)
    {
        if (User.IsInRole(Roles.Faculty))
        {
            var batchId = await db.Students.Where(x => x.Id == request.StudentId).Select(x => x.BatchId).FirstOrDefaultAsync();
            if (batchId is null || !await FacultyCanAccess(request.SubjectId, batchId.Value)) return Forbid();
        }
        var input = new Mark
        {
            StudentId = request.StudentId,
            SubjectId = request.SubjectId,
            AssignmentId = request.AssignmentId,
            MarksObtained = request.MarksObtained,
            MaximumMarks = request.MaximumMarks,
            Status = request.Status
        };
        if (input.MarksObtained < 0 || input.MarksObtained > input.MaximumMarks) return BadRequest(new { message = "Marks must be between 0 and maximum marks." });
        if (input.Status == "Published") input.PublishedAt = DateTime.UtcNow;
        db.Marks.Add(input); await db.SaveChangesAsync(); return Ok(input);
    }

    [HttpGet("results")]
    public async Task<object> Results()
    {
        var studentId = await CurrentStudentId();
        var marks = await db.Marks.AsNoTracking().Include(x => x.Subject).Where(x => x.StudentId == studentId && x.Status == "Published").ToListAsync();
        var grades = marks.Select(m => { var pct = m.MarksObtained * 100 / m.MaximumMarks; var grade = pct >= 90 ? "A+" : pct >= 80 ? "A" : pct >= 70 ? "B+" : pct >= 60 ? "B" : pct >= 50 ? "C" : "F"; return new { course = m.Subject.Name, code = m.Subject.Code, credit = m.Subject.Credits, grade, point = config.GetValue<decimal>($"GradePoints:{grade}") }; }).ToList();
        var credits = grades.Sum(x => x.credit);
        var sgpa = credits == 0 ? 0 : Math.Round(grades.Sum(x => x.credit * x.point) / credits, 2);
        return new { sgpa, cgpa = sgpa, grades };
    }

    [HttpGet("examinations")]
    public async Task<object> Exams() => await db.Examinations.AsNoTracking().Include(x => x.Subject).Include(x => x.Batch).Select(x => new { x.Id, x.Name, subject = x.Subject.Name, batch = x.Batch.Name, x.StartsAt, x.EndsAt, x.Room }).ToListAsync();

    public record ExaminationRequest(string Name, int SubjectId, int BatchId, DateTime StartsAt, DateTime EndsAt, string Room);

    [HttpPost("examinations"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> CreateExam(ExaminationRequest request)
    {
        var input = new Examination { Name = request.Name, SubjectId = request.SubjectId, BatchId = request.BatchId, StartsAt = request.StartsAt, EndsAt = request.EndsAt, Room = request.Room };
        var conflict = await db.Examinations.AnyAsync(x => input.StartsAt < x.EndsAt && input.EndsAt > x.StartsAt && (x.Room == input.Room || x.BatchId == input.BatchId));
        if (conflict) return Conflict(new { message = "Examination schedule conflict." });
        db.Examinations.Add(input); await db.SaveChangesAsync(); return Ok(input);
    }

    [HttpGet("notifications")]
    public async Task<object> Notifications() { var uid = CurrentUserId(); return await db.Notifications.AsNoTracking().Where(x => x.UserId == uid).OrderByDescending(x => x.CreatedAt).ToListAsync(); }

    [HttpPost("notifications/{id:int}/read")]
    public async Task<IActionResult> MarkNotificationRead(int id)
    {
        var uid = CurrentUserId();
        var notification = await db.Notifications.FirstOrDefaultAsync(x => x.Id == id && x.UserId == uid);
        if (notification is null) return NotFound();
        notification.IsRead = true;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("resources")]
    public async Task<object> StudyResources()
    {
        var query = db.StudyResources.AsNoTracking().Include(x => x.Subject).Include(x => x.Batch).Include(x => x.Staff).AsQueryable();
        if (User.IsInRole(Roles.Faculty))
        {
            var staffId = await CurrentStaffIdOrNull();
            query = query.Where(x => x.StaffId == staffId);
        }
        if (User.IsInRole(Roles.Student))
        {
            var studentId = await CurrentStudentId();
            var student = await db.Students.AsNoTracking().FirstAsync(x => x.Id == studentId);
            var subjectIds = await db.SubjectFacultyAllocations.AsNoTracking()
                .Where(x => x.BatchId == student.BatchId)
                .Select(x => x.SubjectId)
                .Distinct()
                .ToListAsync();
            query = query.Where(x => subjectIds.Contains(x.SubjectId) && (x.BatchId == null || x.BatchId == student.BatchId));
        }
        return await query.OrderByDescending(x => x.CreatedAt)
            .Select(x => new { x.Id, x.Title, x.Description, x.ResourceType, subject = x.Subject.Code + " - " + x.Subject.Name, x.SubjectId, batch = x.Batch != null ? x.Batch.Name : "All assigned batches", x.BatchId, faculty = x.Staff.Name, x.FileName, x.CreatedAt })
            .ToListAsync();
    }

    [HttpPost("resources"), Authorize(Roles = Roles.Faculty + "," + Roles.Admin)]
    public async Task<IActionResult> CreateStudyResource([FromForm] string title, [FromForm] string description, [FromForm] string resourceType, [FromForm] int subjectId, [FromForm] int? batchId, IFormFile? file)
    {
        if (string.IsNullOrWhiteSpace(title)) return BadRequest(new { message = "Title is required." });
        var staffId = await CurrentStaffIdOrNull();
        if (staffId is null)
        {
            var firstStaff = await db.Staff.AsNoTracking().OrderBy(x => x.Id).FirstOrDefaultAsync();
            if (firstStaff is null) return BadRequest(new { message = "At least one staff profile is required to upload resources." });
            staffId = firstStaff.Id;
        }
        if (User.IsInRole(Roles.Faculty) && batchId is not null && !await FacultyCanAccess(subjectId, batchId.Value)) return Forbid();
        var fileName = file?.FileName ?? "Link / classroom resource";
        var stored = "";
        if (file is not null && file.Length > 0)
        {
            if (file.Length > config.GetValue<long>("Uploads:MaxBytes")) return BadRequest(new { message = "File is too large." });
            var dir = Path.Combine(AppContext.BaseDirectory, config["Uploads:Directory"] ?? "uploads");
            Directory.CreateDirectory(dir);
            stored = $"{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}";
            await using var stream = System.IO.File.Create(Path.Combine(dir, stored));
            await file.CopyToAsync(stream);
        }
        var input = new StudyResource { Title = title, Description = description, ResourceType = string.IsNullOrWhiteSpace(resourceType) ? "Notes" : resourceType, SubjectId = subjectId, BatchId = batchId, StaffId = staffId.Value, FileName = fileName, FilePath = stored };
        db.StudyResources.Add(input);
        await db.SaveChangesAsync();
        return Created($"api/resources/{input.Id}", input);
    }

    [HttpGet("resources/{id:int}/download")]
    public async Task<IActionResult> DownloadStudyResource(int id)
    {
        var resource = await db.StudyResources.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (resource is null) return NotFound();
        if (string.IsNullOrWhiteSpace(resource.FilePath)) return BadRequest(new { message = "No uploaded file is attached to this resource." });
        if (User.IsInRole(Roles.Faculty))
        {
            var staffId = await CurrentStaffIdOrNull();
            if (resource.StaffId != staffId) return Forbid();
        }
        if (User.IsInRole(Roles.Student))
        {
            var studentId = await CurrentStudentId();
            var student = await db.Students.AsNoTracking().FirstAsync(x => x.Id == studentId);
            var allowed = await db.SubjectFacultyAllocations.AnyAsync(x => x.BatchId == student.BatchId && x.SubjectId == resource.SubjectId);
            if (!allowed || (resource.BatchId is not null && resource.BatchId != student.BatchId)) return Forbid();
        }
        var path = Path.Combine(AppContext.BaseDirectory, config["Uploads:Directory"] ?? "uploads", resource.FilePath);
        if (!System.IO.File.Exists(path)) return NotFound(new { message = "The uploaded file could not be found on the server." });
        return PhysicalFile(path, "application/octet-stream", resource.FileName);
    }

    [HttpGet("feedback/options"), Authorize(Roles = Roles.Student)]
    public async Task<object> FeedbackOptions()
    {
        var studentId = await CurrentStudentId();
        var student = await db.Students.AsNoTracking().FirstAsync(x => x.Id == studentId);
        return await db.SubjectFacultyAllocations.AsNoTracking().Include(x => x.Subject).Include(x => x.Staff).Include(x => x.Batch)
            .Where(x => x.BatchId == student.BatchId)
            .OrderBy(x => x.Subject.Code)
            .Select(x => new { x.SubjectId, subject = x.Subject.Code + " - " + x.Subject.Name, x.StaffId, faculty = x.Staff.Name, batchId = x.BatchId, batch = x.Batch != null ? x.Batch.Name : "", x.AcademicYear, x.Semester })
            .ToListAsync();
    }

    public record FeedbackRequest(int StaffId, int SubjectId, int BatchId, int Rating, string Comments, string AcademicYear, int Semester);

    [HttpPost("feedback"), Authorize(Roles = Roles.Student)]
    public async Task<IActionResult> SubmitFeedback(FeedbackRequest request)
    {
        var studentId = await CurrentStudentId();
        var student = await db.Students.AsNoTracking().FirstAsync(x => x.Id == studentId);
        if (student.BatchId != request.BatchId) return Forbid();
        var allowed = await db.SubjectFacultyAllocations.AnyAsync(x => x.StaffId == request.StaffId && x.SubjectId == request.SubjectId && x.BatchId == request.BatchId && x.Semester == request.Semester);
        if (!allowed) return Forbid();
        if (request.Rating is < 1 or > 5) return BadRequest(new { message = "Rating must be between 1 and 5." });
        var existing = await db.FacultyFeedback.FirstOrDefaultAsync(x => x.StudentId == studentId && x.StaffId == request.StaffId && x.SubjectId == request.SubjectId && x.BatchId == request.BatchId && x.Semester == request.Semester);
        if (existing is null) db.FacultyFeedback.Add(new FacultyFeedback { StudentId = studentId, StaffId = request.StaffId, SubjectId = request.SubjectId, BatchId = request.BatchId, Rating = request.Rating, Comments = request.Comments, AcademicYear = request.AcademicYear, Semester = request.Semester, SubmittedAt = DateTime.UtcNow });
        else { existing.Rating = request.Rating; existing.Comments = request.Comments; existing.SubmittedAt = DateTime.UtcNow; }
        await db.SaveChangesAsync();
        return Ok(new { message = "Anonymous feedback saved." });
    }

    [HttpGet("feedback"), Authorize(Roles = Roles.Faculty + "," + Roles.Admin)]
    public async Task<object> FeedbackSummary()
    {
        var query = db.FacultyFeedback.AsNoTracking().Include(x => x.Staff).Include(x => x.Subject).Include(x => x.Batch).AsQueryable();
        if (User.IsInRole(Roles.Faculty))
        {
            var staffId = await CurrentStaffIdOrNull();
            query = query.Where(x => x.StaffId == staffId);
        }
        return await query.GroupBy(x => new { x.StaffId, Faculty = x.Staff.Name, x.SubjectId, Subject = x.Subject.Code + " - " + x.Subject.Name, x.BatchId, Batch = x.Batch.Name, x.AcademicYear, x.Semester })
            .Select(g => new { id = g.Key.StaffId + "-" + g.Key.SubjectId + "-" + g.Key.BatchId, faculty = g.Key.Faculty, subject = g.Key.Subject, batch = g.Key.Batch, g.Key.AcademicYear, g.Key.Semester, responses = g.Count(), averageRating = Math.Round(g.Average(x => x.Rating), 2), comments = g.Where(x => x.Comments != "").OrderByDescending(x => x.SubmittedAt).Select(x => x.Comments).Take(5).ToList() })
            .ToListAsync();
    }

    [HttpGet("reports/{name}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<object> Reports(string name) => name switch
    {
        "student-list" => await db.Students.AsNoTracking().ToListAsync(),
        "faculty-list" => await db.Staff.AsNoTracking().ToListAsync(),
        "course-list" => await db.Courses.AsNoTracking().ToListAsync(),
        "attendance-summary" => await db.AttendanceRecords.AsNoTracking().Include(x => x.Student).Include(x => x.Subject)
            .GroupBy(x => new { x.Student.StudentId, x.Student.Name, Subject = x.Subject.Name })
            .Select(g => new { studentId = g.Key.StudentId, student = g.Key.Name, subject = g.Key.Subject, total = g.Count(), attended = g.Count(x => x.Status == "Present" || x.Status == "OD"), absent = g.Count(x => x.Status == "Absent"), percentage = g.Count() == 0 ? 0 : Math.Round(g.Count(x => x.Status == "Present" || x.Status == "OD") * 100m / g.Count(), 2) }).ToListAsync(),
        "student-attendance-detail" => await db.AttendanceRecords.AsNoTracking().Include(x => x.Student).Include(x => x.Subject).OrderByDescending(x => x.Date)
            .Select(x => new { x.Student.StudentId, student = x.Student.Name, subject = x.Subject.Name, x.Date, x.Session, x.Status }).ToListAsync(),
        "marks-summary" => await db.Marks.AsNoTracking().Include(x => x.Student).Include(x => x.Subject)
            .Select(x => new { x.Student.StudentId, student = x.Student.Name, subject = x.Subject.Name, x.MarksObtained, x.MaximumMarks, x.Status, x.PublishedAt }).ToListAsync(),
        "grade-result-report" => await db.Marks.AsNoTracking().Include(x => x.Student).Include(x => x.Subject).Where(x => x.Status == "Published")
            .Select(x => new { x.Student.StudentId, student = x.Student.Name, subject = x.Subject.Name, x.Subject.Credits, percentage = Math.Round(x.MarksObtained * 100m / x.MaximumMarks, 2), grade = x.MarksObtained * 100m / x.MaximumMarks >= 90 ? "A+" : x.MarksObtained * 100m / x.MaximumMarks >= 80 ? "A" : x.MarksObtained * 100m / x.MaximumMarks >= 70 ? "B+" : x.MarksObtained * 100m / x.MaximumMarks >= 60 ? "B" : x.MarksObtained * 100m / x.MaximumMarks >= 50 ? "C" : "F" }).ToListAsync(),
        "arrear-failure-summary" => await db.Marks.AsNoTracking().Include(x => x.Student).Include(x => x.Subject).Where(x => x.Status == "Published" && x.MarksObtained * 100m / x.MaximumMarks < 50)
            .Select(x => new { x.Student.StudentId, student = x.Student.Name, subject = x.Subject.Name, percentage = Math.Round(x.MarksObtained * 100m / x.MaximumMarks, 2), status = "Arrear" }).ToListAsync(),
        "evaluation-components" => await db.EvaluationComponents.AsNoTracking().Include(x => x.Subject)
            .Select(x => new { component = x.Name, subject = x.Subject.Code + " - " + x.Subject.Name, x.Weightage, x.MaximumMarks, x.Status }).ToListAsync(),
        "grade-rules" => await db.GradeRules.AsNoTracking().Include(x => x.Subject)
            .Select(x => new { rule = x.Name, subject = x.Subject != null ? x.Subject.Code + " - " + x.Subject.Name : "Default", x.MinimumPercentage, x.Grade, x.GradePoint, x.Status }).ToListAsync(),
        "grace-mark-summary" => await db.GraceMarkApplications.AsNoTracking().Include(x => x.Student).Include(x => x.Subject).Include(x => x.GraceMarkCategory)
            .Select(x => new { x.Student.StudentId, student = x.Student.Name, subject = x.Subject.Name, category = x.GraceMarkCategory.Name, x.MarksAwarded, x.Status }).ToListAsync(),
        "survey-submission-summary" => await db.Surveys.AsNoTracking()
            .Select(x => new { survey = x.Title, x.Audience, x.Status, responses = db.SurveyResponses.Count(r => r.SurveyId == x.Id), x.ClosesAt }).ToListAsync(),
        _ => new { message = "Report endpoint available.", report = name }
    };

    private int CurrentUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    private async Task<int> CurrentStudentId() => (await db.Students.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == CurrentUserId()))?.Id ?? 1;
    private async Task<int?> CurrentStaffIdOrNull() => (await db.Staff.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == CurrentUserId()))?.Id;
    private async Task<bool> FacultyCanAccess(int subjectId, int batchId)
    {
        var staffId = await CurrentStaffIdOrNull();
        return staffId is not null && await db.SubjectFacultyAllocations.AnyAsync(x => x.StaffId == staffId && x.SubjectId == subjectId && x.BatchId == batchId);
    }
}
