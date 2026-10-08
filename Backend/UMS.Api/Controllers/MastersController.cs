using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UMS.Api.Data;
using UMS.Api.Models;

namespace UMS.Api.Controllers;

[ApiController]
[Route("api")]
[Authorize]
public class MastersController(AppDbContext db) : ControllerBase
{
    public record StaffMasterRequest(string EmployeeId, string Name, string Email, string Department, string Designation);
    public record UserMasterRequest(string Username, string Email, string DisplayName, int RoleMasterId, string? Password);

    private static object StaffResponse(Staff staff) => new
    {
        id = staff.EmployeeId,
        key = staff.Id,
        name = staff.Name,
        email = staff.Email,
        department = staff.Department,
        detail = staff.Designation,
        designation = staff.Designation,
        userId = staff.UserId,
        status = "Active"
    };

    private static object UserResponse(User user, string roleName) => new
    {
        id = user.Username,
        key = user.Id,
        name = user.DisplayName,
        email = user.Email,
        department = "Users",
        detail = roleName,
        status = "Active",
        user.Username,
        user.RoleMasterId
    };

    private static object StudentResponse(Student student, Batch? batch = null) => new
    {
        id = student.StudentId,
        key = student.Id,
        name = student.Name,
        email = student.Email,
        department = student.Department,
        program = student.Program,
        detail = "Semester " + student.Semester,
        semester = student.Semester,
        batchId = student.BatchId,
        userId = student.UserId,
        status = "Active",
        batch = batch?.Name ?? ""
    };

    [HttpGet("users")]
    public async Task<object> Users(string? search, int page = 1, int pageSize = 20) =>
        await Page(db.Users.AsNoTracking().Include(x => x.RoleMaster).Where(x => search == null || x.Username.Contains(search) || x.Email.Contains(search) || x.DisplayName.Contains(search))
            .Select(x => new { id = x.Username, key = x.Id, name = x.DisplayName, email = x.Email, department = "Users", detail = x.RoleMaster != null ? x.RoleMaster.Name : x.Role, status = "Active", x.Username, x.RoleMasterId }), page, pageSize);

    [HttpPost("users"), Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<object>> CreateUser(UserMasterRequest input)
    {
        if (string.IsNullOrWhiteSpace(input.Username)) return BadRequest(new { message = "Username or university ID is required." });
        if (string.IsNullOrWhiteSpace(input.DisplayName)) return BadRequest(new { message = "Display name is required." });
        if (string.IsNullOrWhiteSpace(input.Email)) return BadRequest(new { message = "Email is required." });
        if (string.IsNullOrWhiteSpace(input.Password)) return BadRequest(new { message = "Temporary password is required." });
        if (await db.Users.AnyAsync(x => x.Username == input.Username.Trim())) return Conflict(new { message = "A user with this username already exists." });
        if (await db.Users.AnyAsync(x => x.Email == input.Email.Trim())) return Conflict(new { message = "A user with this email already exists." });
        var role = await db.RoleMasters.FindAsync(input.RoleMasterId);
        if (role is null) return BadRequest(new { message = "Please select a valid role from Role Master." });
        var user = new User
        {
            Username = input.Username.Trim(),
            Email = input.Email.Trim(),
            DisplayName = input.DisplayName.Trim(),
            RoleMasterId = role.Id,
            Role = role.Name,
            PasswordHash = input.Password.Trim(),
            MustChangePassword = role.Name is Roles.Faculty or Roles.Student
        };
        db.Users.Add(user);
        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict(new { message = "The user could not be saved because the username or email already exists." });
        }
        return Created($"api/users/{user.Id}", UserResponse(user, role.Name));
    }

    [HttpPut("users/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateUser(int id, UserMasterRequest input)
    {
        var row = await db.Users.FindAsync(id);
        var role = await db.RoleMasters.FindAsync(input.RoleMasterId);
        if (row is null) return NotFound();
        if (string.IsNullOrWhiteSpace(input.Username)) return BadRequest(new { message = "Username or university ID is required." });
        if (string.IsNullOrWhiteSpace(input.DisplayName)) return BadRequest(new { message = "Display name is required." });
        if (string.IsNullOrWhiteSpace(input.Email)) return BadRequest(new { message = "Email is required." });
        if (role is null) return BadRequest(new { message = "Please select a valid role from Role Master." });
        var username = input.Username.Trim();
        var email = input.Email.Trim();
        if (await db.Users.AnyAsync(x => x.Id != id && x.Username == username)) return Conflict(new { message = "Another user already uses this username." });
        if (await db.Users.AnyAsync(x => x.Id != id && x.Email == email)) return Conflict(new { message = "Another user already uses this email." });
        row.Username = username; row.Email = email; row.DisplayName = input.DisplayName.Trim(); row.RoleMasterId = role.Id; row.Role = role.Name;
        if (!string.IsNullOrWhiteSpace(input.Password)) { row.PasswordHash = input.Password.Trim(); row.MustChangePassword = row.Role is Roles.Faculty or Roles.Student; }
        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict(new { message = "The user could not be updated because the username or email conflicts with another record." });
        }
        return NoContent();
    }

    [HttpDelete("users/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteUser(int id) { var row = await db.Users.FindAsync(id); if (row is null) return NotFound(); db.Remove(row); await db.SaveChangesAsync(); return NoContent(); }

    [HttpGet("staff")]
    public async Task<object> Staff(string? search, int page = 1, int pageSize = 20) =>
        await Page(db.Staff.AsNoTracking().Where(x => search == null || x.Name.Contains(search) || x.EmployeeId.Contains(search))
            .Select(x => new { id = x.EmployeeId, key = x.Id, name = x.Name, email = x.Email, department = x.Department, detail = x.Designation, designation = x.Designation, userId = x.UserId, status = "Active" }), page, pageSize);

    [HttpPost("staff"), Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<object>> CreateStaff(StaffMasterRequest input)
    {
        if (string.IsNullOrWhiteSpace(input.EmployeeId)) return BadRequest(new { message = "Employee ID is required." });
        if (string.IsNullOrWhiteSpace(input.Name)) return BadRequest(new { message = "Full name is required." });
        if (string.IsNullOrWhiteSpace(input.Email)) return BadRequest(new { message = "Email is required." });
        if (string.IsNullOrWhiteSpace(input.Department)) return BadRequest(new { message = "Please select a department from Department Master." });
        if (string.IsNullOrWhiteSpace(input.Designation)) return BadRequest(new { message = "Please select a designation from Designation Master." });
        if (await db.Staff.AnyAsync(x => x.EmployeeId == input.EmployeeId.Trim()) || await db.Users.AnyAsync(x => x.Username == input.EmployeeId.Trim())) return Conflict(new { message = "A staff/user record with this employee ID already exists." });
        if (await db.Staff.AnyAsync(x => x.Email == input.Email.Trim()) || await db.Users.AnyAsync(x => x.Email == input.Email.Trim())) return Conflict(new { message = "A staff/user record with this email already exists." });
        var role = await db.RoleMasters.FirstAsync(x => x.Name == Roles.Faculty);
        var user = new User { Username = input.EmployeeId.Trim(), Email = input.Email.Trim(), DisplayName = input.Name.Trim(), Role = Roles.Faculty, RoleMasterId = role.Id, PasswordHash = "amrita2026", MustChangePassword = true };
        var staff = new Staff { EmployeeId = input.EmployeeId.Trim(), Name = input.Name.Trim(), Email = input.Email.Trim(), Department = input.Department, Designation = input.Designation, User = user };
        db.Staff.Add(staff);
        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict(new { message = "The staff record could not be saved because the employee ID or email already exists. Please use a unique Employee ID and email." });
        }
        return Created($"api/staff/{staff.Id}", StaffResponse(staff));
    }

    [HttpPut("staff/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateStaff(int id, StaffMasterRequest input)
    {
        var row = await db.Staff.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
        if (row is null) return NotFound();
        if (string.IsNullOrWhiteSpace(input.EmployeeId)) return BadRequest(new { message = "Employee ID is required." });
        if (string.IsNullOrWhiteSpace(input.Name)) return BadRequest(new { message = "Full name is required." });
        if (string.IsNullOrWhiteSpace(input.Email)) return BadRequest(new { message = "Email is required." });
        if (string.IsNullOrWhiteSpace(input.Department)) return BadRequest(new { message = "Please select a department from Department Master." });
        if (string.IsNullOrWhiteSpace(input.Designation)) return BadRequest(new { message = "Please select a designation from Designation Master." });
        var employeeId = input.EmployeeId.Trim();
        var email = input.Email.Trim();
        if (await db.Staff.AnyAsync(x => x.Id != id && x.EmployeeId == employeeId) || await db.Users.AnyAsync(x => x.Id != row.UserId && x.Username == employeeId)) return Conflict(new { message = "Another staff/user record already uses this employee ID." });
        if (await db.Staff.AnyAsync(x => x.Id != id && x.Email == email) || await db.Users.AnyAsync(x => x.Id != row.UserId && x.Email == email)) return Conflict(new { message = "Another staff/user record already uses this email." });
        row.EmployeeId = input.EmployeeId.Trim(); row.Name = input.Name.Trim(); row.Email = input.Email.Trim(); row.Department = input.Department; row.Designation = input.Designation;
        if (row.User is null)
        {
            var role = await db.RoleMasters.FirstAsync(x => x.Name == Roles.Faculty);
            row.User = new User { Username = row.EmployeeId, Email = row.Email, DisplayName = row.Name, Role = Roles.Faculty, RoleMasterId = role.Id, PasswordHash = "amrita2026", MustChangePassword = true };
        }
        else
        {
            row.User.Username = row.EmployeeId; row.User.Email = row.Email; row.User.DisplayName = row.Name;
        }
        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict(new { message = "The staff record could not be updated because the employee ID or email conflicts with another record." });
        }
        return NoContent();
    }

    [HttpDelete("staff/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteStaff(int id) { var row = await db.Staff.FindAsync(id); if (row is null) return NotFound(); db.Remove(row); await db.SaveChangesAsync(); return NoContent(); }

    [HttpGet("students")]
    public async Task<object> Students(string? search, int page = 1, int pageSize = 20) =>
        await Page(db.Students.AsNoTracking().Include(x => x.Batch).Where(x => search == null || x.Name.Contains(search) || x.StudentId.Contains(search))
            .Select(x => new { id = x.StudentId, key = x.Id, name = x.Name, email = x.Email, department = x.Department, program = x.Program, detail = "Semester " + x.Semester, semester = x.Semester, batchId = x.BatchId, userId = x.UserId, status = "Active", batch = x.Batch!.Name }), page, pageSize);

    [HttpPost("students"), Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<object>> CreateStudent(Student input)
    {
        if (string.IsNullOrWhiteSpace(input.StudentId)) return BadRequest(new { message = "Student ID is required." });
        if (string.IsNullOrWhiteSpace(input.Name)) return BadRequest(new { message = "Full name is required." });
        if (string.IsNullOrWhiteSpace(input.Email)) return BadRequest(new { message = "Email is required." });
        if (string.IsNullOrWhiteSpace(input.Department)) return BadRequest(new { message = "Please select a department from Department Master." });
        if (input.BatchId is null) return BadRequest(new { message = "Please select a batch from Batch Master." });
        if (await db.Students.AnyAsync(x => x.StudentId == input.StudentId.Trim()) || await db.Users.AnyAsync(x => x.Username == input.StudentId.Trim())) return Conflict(new { message = "A student/user record with this student ID already exists." });
        if (await db.Students.AnyAsync(x => x.Email == input.Email.Trim()) || await db.Users.AnyAsync(x => x.Email == input.Email.Trim())) return Conflict(new { message = "A student/user record with this email already exists." });
        Batch? selectedBatch = null;
        if (input.BatchId is not null)
        {
            selectedBatch = await db.Batches.AsNoTracking().FirstOrDefaultAsync(x => x.Id == input.BatchId);
            if (selectedBatch is null) return BadRequest(new { message = "Please select a valid batch from Batch Master." });
            input.Program = selectedBatch.Program;
            input.Semester = selectedBatch.Semester;
        }
        var role = await db.RoleMasters.FirstAsync(x => x.Name == Roles.Student);
        input.StudentId = input.StudentId.Trim();
        input.Name = input.Name.Trim();
        input.Email = input.Email.Trim();
        input.User = new User { Username = input.StudentId.Trim(), Email = input.Email.Trim(), DisplayName = input.Name.Trim(), Role = Roles.Student, RoleMasterId = role.Id, PasswordHash = "amrita2026", MustChangePassword = true };
        db.Students.Add(input);
        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return Conflict(new { message = "The student could not be saved because the student ID or email already exists." });
        }
        return Created($"api/students/{input.Id}", StudentResponse(input, selectedBatch));
    }

    [HttpPut("students/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateStudent(int id, Student input)
    {
        var row = await db.Students.FindAsync(id);
        if (row is null) return NotFound();
        row.Name = input.Name.Trim(); row.Email = input.Email.Trim(); row.Department = input.Department; row.Program = input.Program; row.BatchId = input.BatchId; row.Semester = input.Semester;
        if (input.BatchId is not null)
        {
            var batch = await db.Batches.AsNoTracking().FirstOrDefaultAsync(x => x.Id == input.BatchId);
            if (batch is null) return BadRequest(new { message = "Please select a valid batch from Batch Master." });
            row.Program = batch.Program;
            row.Semester = batch.Semester;
        }
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("students/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteStudent(int id) { var row = await db.Students.FindAsync(id); if (row is null) return NotFound(); db.Remove(row); await db.SaveChangesAsync(); return NoContent(); }

    [HttpGet("courses")]
    public async Task<object> Courses(string? search, int page = 1, int pageSize = 20) =>
        await Page(db.Courses.AsNoTracking().Where(x => search == null || x.Name.Contains(search) || x.CourseCode.Contains(search))
            .Select(x => new { id = x.CourseCode, key = x.Id, name = x.Name, courseCode = x.CourseCode, department = x.Department, credits = x.Credits, description = x.Description, detail = x.Credits + " credits", status = "Active" }), page, pageSize);

    [HttpPost("courses"), Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<Course>> CreateCourse(Course input) { db.Courses.Add(input); await db.SaveChangesAsync(); return Created($"api/courses/{input.Id}", input); }
    [HttpPut("courses/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateCourse(int id, Course input) { var row = await db.Courses.FindAsync(id); if (row is null) return NotFound(); row.CourseCode = input.CourseCode; row.Name = input.Name; row.Credits = input.Credits; row.Department = input.Department; row.Description = input.Description; await db.SaveChangesAsync(); return NoContent(); }
    [HttpDelete("courses/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteCourse(int id) { var row = await db.Courses.FindAsync(id); if (row is null) return NotFound(); db.Remove(row); await db.SaveChangesAsync(); return NoContent(); }

    [HttpGet("batches")]
    public async Task<object> Batches(string? search, int page = 1, int pageSize = 20) =>
        await Page(db.Batches.AsNoTracking().Where(x => search == null || x.Name.Contains(search) || x.Program.Contains(search))
            .Select(x => new { id = x.Name, key = x.Id, name = x.Name, department = x.Program, program = x.Program, academicYear = x.AcademicYear, semester = x.Semester, detail = x.AcademicYear, status = "Semester " + x.Semester }), page, pageSize);

    [HttpPost("batches"), Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<Batch>> CreateBatch(Batch input) { db.Batches.Add(input); await db.SaveChangesAsync(); return Created($"api/batches/{input.Id}", input); }
    [HttpPut("batches/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateBatch(int id, Batch input) { var row = await db.Batches.FindAsync(id); if (row is null) return NotFound(); row.Name = input.Name; row.Program = input.Program; row.AcademicYear = input.AcademicYear; row.Semester = input.Semester; await db.SaveChangesAsync(); return NoContent(); }
    [HttpDelete("batches/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteBatch(int id) { var row = await db.Batches.FindAsync(id); if (row is null) return NotFound(); db.Remove(row); await db.SaveChangesAsync(); return NoContent(); }

    [HttpGet("subjects")]
    public async Task<object> Subjects(string? search, int page = 1, int pageSize = 20) =>
        await Page(db.Subjects.AsNoTracking().Include(x => x.Course).Where(x => search == null || x.Name.Contains(search) || x.Code.Contains(search))
            .Select(x => new { id = x.Code, key = x.Id, name = x.Name, code = x.Code, credits = x.Credits, semester = x.Semester, courseId = x.CourseId, department = x.Course!.Department, detail = x.Credits + " credits · Semester " + x.Semester, status = "Unallocated" }), page, pageSize);

    [HttpPost("subjects"), Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<Subject>> CreateSubject(Subject input)
    {
        if (await db.Subjects.AnyAsync(x => x.Code == input.Code.Trim())) return Conflict(new { message = "A subject with this code already exists." });
        db.Subjects.Add(input); await db.SaveChangesAsync(); return Created($"api/subjects/{input.Id}", input);
    }
    [HttpPut("subjects/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateSubject(int id, Subject input) { var row = await db.Subjects.FindAsync(id); if (row is null) return NotFound(); row.Code = input.Code; row.Name = input.Name; row.Credits = input.Credits; row.Semester = input.Semester; row.CourseId = input.CourseId; await db.SaveChangesAsync(); return NoContent(); }

    [HttpGet("department-masters")]
    public async Task<object> DepartmentMasters(string? search, int page = 1, int pageSize = 20) =>
        await Page(db.DepartmentMasters.AsNoTracking().Where(x => search == null || x.Name.Contains(search) || x.Code.Contains(search))
            .Select(x => new { id = x.Code, key = x.Id, name = x.Name, code = x.Code, department = "Department", detail = x.Code, status = "Active" }), page, pageSize);

    [HttpPost("department-masters"), Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<DepartmentMaster>> CreateDepartmentMaster(DepartmentMaster input) { db.DepartmentMasters.Add(input); await db.SaveChangesAsync(); return Created($"api/department-masters/{input.Id}", input); }
    [HttpPut("department-masters/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateDepartmentMaster(int id, DepartmentMaster input) { var row = await db.DepartmentMasters.FindAsync(id); if (row is null) return NotFound(); row.Name = input.Name; row.Code = input.Code; await db.SaveChangesAsync(); return NoContent(); }
    [HttpDelete("department-masters/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteDepartmentMaster(int id) { var row = await db.DepartmentMasters.FindAsync(id); if (row is null) return NotFound(); db.Remove(row); await db.SaveChangesAsync(); return NoContent(); }

    [HttpGet("designation-masters")]
    public async Task<object> DesignationMasters(string? search, int page = 1, int pageSize = 20) =>
        await Page(db.DesignationMasters.AsNoTracking().Where(x => search == null || x.Name.Contains(search))
            .Select(x => new { id = x.Id, key = x.Id, name = x.Name, department = "Designation", detail = "Staff role", status = "Active" }), page, pageSize);

    [HttpPost("designation-masters"), Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<DesignationMaster>> CreateDesignationMaster(DesignationMaster input) { db.DesignationMasters.Add(input); await db.SaveChangesAsync(); return Created($"api/designation-masters/{input.Id}", input); }
    [HttpPut("designation-masters/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateDesignationMaster(int id, DesignationMaster input) { var row = await db.DesignationMasters.FindAsync(id); if (row is null) return NotFound(); row.Name = input.Name; await db.SaveChangesAsync(); return NoContent(); }
    [HttpDelete("designation-masters/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteDesignationMaster(int id) { var row = await db.DesignationMasters.FindAsync(id); if (row is null) return NotFound(); db.Remove(row); await db.SaveChangesAsync(); return NoContent(); }

    [HttpGet("program-masters")]
    public async Task<object> ProgramMasters(string? search, int page = 1, int pageSize = 20) =>
        await Page(db.ProgramMasters.AsNoTracking().Where(x => search == null || x.Name.Contains(search) || x.Code.Contains(search))
            .Select(x => new { id = x.Code, key = x.Id, name = x.Name, code = x.Code, department = "Program", detail = x.Code, status = "Active" }), page, pageSize);

    [HttpPost("program-masters"), Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<ProgramMaster>> CreateProgramMaster(ProgramMaster input) { db.ProgramMasters.Add(input); await db.SaveChangesAsync(); return Created($"api/program-masters/{input.Id}", input); }
    [HttpPut("program-masters/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateProgramMaster(int id, ProgramMaster input) { var row = await db.ProgramMasters.FindAsync(id); if (row is null) return NotFound(); row.Name = input.Name; row.Code = input.Code; await db.SaveChangesAsync(); return NoContent(); }
    [HttpDelete("program-masters/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteProgramMaster(int id) { var row = await db.ProgramMasters.FindAsync(id); if (row is null) return NotFound(); db.Remove(row); await db.SaveChangesAsync(); return NoContent(); }

    [HttpGet("academic-sessions")]
    public async Task<object> AcademicSessions(string? search, int page = 1, int pageSize = 20) =>
        await Page(db.AcademicSessions.AsNoTracking().Where(x => search == null || x.AcademicYear.Contains(search))
            .Select(x => new { id = x.AcademicYear + "-S" + x.Semester, key = x.Id, name = x.AcademicYear, academicYear = x.AcademicYear, semester = x.Semester, department = "Academic", detail = "Semester " + x.Semester, status = x.IsActive ? "Active" : "Inactive", isActive = x.IsActive }), page, pageSize);

    [HttpPost("academic-sessions"), Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<AcademicSession>> CreateAcademicSession(AcademicSession input) { db.AcademicSessions.Add(input); await db.SaveChangesAsync(); return Created($"api/academic-sessions/{input.Id}", input); }
    [HttpPut("academic-sessions/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateAcademicSession(int id, AcademicSession input) { var row = await db.AcademicSessions.FindAsync(id); if (row is null) return NotFound(); row.AcademicYear = input.AcademicYear; row.Semester = input.Semester; row.IsActive = input.IsActive; await db.SaveChangesAsync(); return NoContent(); }
    [HttpDelete("academic-sessions/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteAcademicSession(int id) { var row = await db.AcademicSessions.FindAsync(id); if (row is null) return NotFound(); db.Remove(row); await db.SaveChangesAsync(); return NoContent(); }

    [HttpGet("role-masters")]
    public async Task<object> RoleMasters(string? search, int page = 1, int pageSize = 20) =>
        await Page(db.RoleMasters.AsNoTracking().Where(x => search == null || x.Name.Contains(search))
            .Select(x => new { id = x.Id, key = x.Id, name = x.Name, department = "Access Control", detail = x.Description, description = x.Description, status = "Active" }), page, pageSize);

    [HttpPost("role-masters"), Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<RoleMaster>> CreateRoleMaster(RoleMaster input) { db.RoleMasters.Add(input); await db.SaveChangesAsync(); return Created($"api/role-masters/{input.Id}", input); }
    [HttpPut("role-masters/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateRoleMaster(int id, RoleMaster input) { var row = await db.RoleMasters.FindAsync(id); if (row is null) return NotFound(); row.Name = input.Name; row.Description = input.Description; await db.SaveChangesAsync(); return NoContent(); }
    [HttpDelete("role-masters/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteRoleMaster(int id) { var row = await db.RoleMasters.FindAsync(id); if (row is null) return NotFound(); db.Remove(row); await db.SaveChangesAsync(); return NoContent(); }

    [HttpGet("reference-data")]
    public async Task<object> ReferenceData() => new
    {
        departments = await db.DepartmentMasters.AsNoTracking().OrderBy(x => x.Name).Select(x => new { value = x.Name, label = x.Name, x.Id, x.Code }).ToListAsync(),
        designations = await db.DesignationMasters.AsNoTracking().OrderBy(x => x.Name).Select(x => new { value = x.Name, label = x.Name, x.Id }).ToListAsync(),
        programs = await db.ProgramMasters.AsNoTracking().OrderBy(x => x.Name).Select(x => new { value = x.Name, label = x.Name, x.Id, x.Code }).ToListAsync(),
        academicSessions = await db.AcademicSessions.AsNoTracking().OrderBy(x => x.AcademicYear).ThenBy(x => x.Semester).Select(x => new { value = x.Id, label = x.AcademicYear + " - Semester " + x.Semester, x.Id, x.AcademicYear, x.Semester }).ToListAsync(),
        batches = await db.Batches.AsNoTracking().OrderBy(x => x.Name).Select(x => new { value = x.Id, label = x.Name, x.Id, x.Program, x.AcademicYear, x.Semester }).ToListAsync(),
        courses = await db.Courses.AsNoTracking().OrderBy(x => x.Name).Select(x => new { value = x.Id, label = x.CourseCode + " - " + x.Name, x.Id }).ToListAsync(),
        staff = await db.Staff.AsNoTracking().OrderBy(x => x.Name).Select(x => new { value = x.Id, label = x.EmployeeId + " - " + x.Name, x.Id }).ToListAsync(),
        students = await db.Students.AsNoTracking().OrderBy(x => x.StudentId).Select(x => new { value = x.Id, label = x.StudentId + " - " + x.Name, x.Id }).ToListAsync(),
        subjects = await db.Subjects.AsNoTracking().OrderBy(x => x.Name).Select(x => new { value = x.Id, label = x.Code + " - " + x.Name, x.Id }).ToListAsync(),
        assignments = await db.Assignments.AsNoTracking().OrderBy(x => x.Title).Select(x => new { value = x.Id, label = x.Title, x.Id }).ToListAsync(),
        roles = await db.RoleMasters.AsNoTracking().OrderBy(x => x.Name).Select(x => new { value = x.Id, label = x.Name, x.Id }).ToListAsync(),
        graceMarkCategories = await db.GraceMarkCategories.AsNoTracking().OrderBy(x => x.Name).Select(x => new { value = x.Id, label = x.Name, x.Id, x.MaximumMarks }).ToListAsync()
    };

    [HttpGet("subject-allocations")]
    public async Task<object> SubjectAllocations(int page = 1, int pageSize = 20) =>
        await Page(db.SubjectFacultyAllocations.AsNoTracking().Include(x => x.Subject).Include(x => x.Staff).Include(x => x.Batch)
            .Select(x => new { id = x.Id, key = x.Id, subject = x.Subject.Code + " - " + x.Subject.Name, faculty = x.Staff.EmployeeId + " - " + x.Staff.Name, batch = x.Batch != null ? x.Batch.Name : "All batches", x.AcademicYear, x.Semester }), page, pageSize);

    public record SubjectAllocationRequest(int SubjectId, int StaffId, int? BatchId, string AcademicYear, int Semester);

    [HttpPost("subject-allocations"), Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<SubjectFacultyAllocation>> CreateSubjectAllocation(SubjectAllocationRequest input)
    {
        var allocation = new SubjectFacultyAllocation
        {
            SubjectId = input.SubjectId,
            StaffId = input.StaffId,
            BatchId = input.BatchId,
            AcademicYear = input.AcademicYear,
            Semester = input.Semester
        };
        db.SubjectFacultyAllocations.Add(allocation);
        await db.SaveChangesAsync();
        return Created($"api/subject-allocations/{allocation.Id}", allocation);
    }
    [HttpDelete("subjects/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteSubject(int id) { var row = await db.Subjects.FindAsync(id); if (row is null) return NotFound(); db.Remove(row); await db.SaveChangesAsync(); return NoContent(); }

    private static async Task<object> Page<T>(IQueryable<T> query, int page, int pageSize)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);
        var total = await query.CountAsync();
        var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return new { data, total, page, pageSize };
    }
}
