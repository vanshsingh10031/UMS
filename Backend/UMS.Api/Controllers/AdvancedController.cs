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
public class AdvancedController(AppDbContext db) : ControllerBase
{
    [HttpGet("evaluation-components")]
    public async Task<object> EvaluationComponents()
    {
        var query = db.EvaluationComponents.AsNoTracking().Include(x => x.Subject).AsQueryable();
        if (User.IsInRole(Roles.Faculty))
        {
            var staffId = await CurrentStaffIdOrNull();
            query = query.Where(x => db.SubjectFacultyAllocations.Any(a => a.StaffId == staffId && a.SubjectId == x.SubjectId));
        }
        if (User.IsInRole(Roles.Student))
        {
            var studentId = await CurrentStudentId();
            var batchId = await db.Students.Where(x => x.Id == studentId).Select(x => x.BatchId).FirstAsync();
            query = query.Where(x => db.SubjectFacultyAllocations.Any(a => a.BatchId == batchId && a.SubjectId == x.SubjectId));
        }
        return await query.OrderBy(x => x.Subject.Code).ThenBy(x => x.Name)
            .Select(x => new { x.Id, x.Name, subject = x.Subject.Code + " - " + x.Subject.Name, x.SubjectId, x.Weightage, x.MaximumMarks, x.Status })
            .ToListAsync();
    }

    [HttpPost("evaluation-components"), Authorize(Roles = Roles.Admin + "," + Roles.Faculty)]
    public async Task<IActionResult> CreateEvaluationComponent(EvaluationComponent input)
    {
        if (input.Weightage <= 0 || input.Weightage > 100) return BadRequest(new { message = "Weightage must be between 1 and 100." });
        if (input.MaximumMarks <= 0) return BadRequest(new { message = "Maximum marks must be greater than 0." });
        if (User.IsInRole(Roles.Faculty) && !await FacultyOwnsSubject(input.SubjectId)) return Forbid();
        db.EvaluationComponents.Add(input);
        await db.SaveChangesAsync();
        return Created($"api/evaluation-components/{input.Id}", input);
    }

    [HttpPut("evaluation-components/{id:int}"), Authorize(Roles = Roles.Admin + "," + Roles.Faculty)]
    public async Task<IActionResult> UpdateEvaluationComponent(int id, EvaluationComponent input)
    {
        var row = await db.EvaluationComponents.FindAsync(id);
        if (row is null) return NotFound();
        if (User.IsInRole(Roles.Faculty) && !await FacultyOwnsSubject(input.SubjectId)) return Forbid();
        row.Name = input.Name; row.SubjectId = input.SubjectId; row.Weightage = input.Weightage; row.MaximumMarks = input.MaximumMarks; row.Status = input.Status;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("evaluation-components/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteEvaluationComponent(int id) => await Delete<EvaluationComponent>(id);

    [HttpGet("grade-rules")]
    public async Task<object> GradeRules() => await db.GradeRules.AsNoTracking().Include(x => x.Subject)
        .OrderByDescending(x => x.MinimumPercentage)
        .Select(x => new { x.Id, x.Name, subject = x.Subject != null ? x.Subject.Code + " - " + x.Subject.Name : "Default", x.SubjectId, x.MinimumPercentage, x.Grade, x.GradePoint, x.Status })
        .ToListAsync();

    [HttpPost("grade-rules"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> CreateGradeRule(GradeRule input)
    {
        db.GradeRules.Add(input);
        await db.SaveChangesAsync();
        return Created($"api/grade-rules/{input.Id}", input);
    }

    [HttpPut("grade-rules/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateGradeRule(int id, GradeRule input)
    {
        var row = await db.GradeRules.FindAsync(id);
        if (row is null) return NotFound();
        row.Name = input.Name; row.SubjectId = input.SubjectId; row.MinimumPercentage = input.MinimumPercentage; row.Grade = input.Grade; row.GradePoint = input.GradePoint; row.Status = input.Status;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("grade-rules/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteGradeRule(int id) => await Delete<GradeRule>(id);

    [HttpGet("grace-mark-categories")]
    public async Task<object> GraceMarkCategories() => await db.GraceMarkCategories.AsNoTracking().OrderBy(x => x.Name).ToListAsync();

    [HttpPost("grace-mark-categories"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> CreateGraceMarkCategory(GraceMarkCategory input)
    {
        db.GraceMarkCategories.Add(input);
        await db.SaveChangesAsync();
        return Created($"api/grace-mark-categories/{input.Id}", input);
    }

    [HttpPut("grace-mark-categories/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateGraceMarkCategory(int id, GraceMarkCategory input)
    {
        var row = await db.GraceMarkCategories.FindAsync(id);
        if (row is null) return NotFound();
        row.Name = input.Name; row.Description = input.Description; row.MaximumMarks = input.MaximumMarks; row.Status = input.Status;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("grace-mark-categories/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteGraceMarkCategory(int id) => await Delete<GraceMarkCategory>(id);

    [HttpGet("grace-mark-applications")]
    public async Task<object> GraceMarkApplications() => await db.GraceMarkApplications.AsNoTracking().Include(x => x.Student).Include(x => x.Subject).Include(x => x.GraceMarkCategory)
        .OrderByDescending(x => x.Id)
        .Select(x => new { x.Id, student = x.Student.StudentId + " - " + x.Student.Name, x.StudentId, subject = x.Subject.Code + " - " + x.Subject.Name, x.SubjectId, category = x.GraceMarkCategory.Name, graceMarkCategoryId = x.GraceMarkCategoryId, x.MarksAwarded, x.Reason, x.Status })
        .ToListAsync();

    [HttpPost("grace-mark-applications"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> CreateGraceMarkApplication(GraceMarkApplication input)
    {
        var category = await db.GraceMarkCategories.FindAsync(input.GraceMarkCategoryId);
        if (category is null) return BadRequest(new { message = "Invalid grace mark category." });
        if (input.MarksAwarded <= 0 || input.MarksAwarded > category.MaximumMarks) return BadRequest(new { message = $"Marks awarded must be between 1 and {category.MaximumMarks}." });
        db.GraceMarkApplications.Add(input);
        await db.SaveChangesAsync();
        return Created($"api/grace-mark-applications/{input.Id}", input);
    }

    [HttpPut("grace-mark-applications/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateGraceMarkApplication(int id, GraceMarkApplication input)
    {
        var row = await db.GraceMarkApplications.FindAsync(id);
        if (row is null) return NotFound();
        row.StudentId = input.StudentId; row.SubjectId = input.SubjectId; row.GraceMarkCategoryId = input.GraceMarkCategoryId; row.MarksAwarded = input.MarksAwarded; row.Reason = input.Reason; row.Status = input.Status;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("grace-mark-applications/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteGraceMarkApplication(int id) => await Delete<GraceMarkApplication>(id);

    [HttpGet("surveys")]
    public async Task<object> Surveys() => await db.Surveys.AsNoTracking()
        .OrderByDescending(x => x.Id)
        .Select(x => new { x.Id, x.Title, x.Description, x.Audience, x.Status, x.PublishedAt, x.ClosesAt, responses = db.SurveyResponses.Count(r => r.SurveyId == x.Id) })
        .ToListAsync();

    [HttpPost("surveys"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> CreateSurvey(Survey input)
    {
        if (input.Status == "Published" && input.PublishedAt == null) input.PublishedAt = DateTime.UtcNow;
        db.Surveys.Add(input);
        await db.SaveChangesAsync();
        return Created($"api/surveys/{input.Id}", input);
    }

    [HttpPut("surveys/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateSurvey(int id, Survey input)
    {
        var row = await db.Surveys.FindAsync(id);
        if (row is null) return NotFound();
        row.Title = input.Title; row.Description = input.Description; row.Audience = input.Audience; row.Status = input.Status; row.PublishedAt = input.PublishedAt; row.ClosesAt = input.ClosesAt;
        if (row.Status == "Published" && row.PublishedAt == null) row.PublishedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("surveys/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteSurvey(int id) => await Delete<Survey>(id);

    public record SurveyResponseRequest(string ResponseJson);

    [HttpPost("surveys/{id:int}/responses")]
    public async Task<IActionResult> SubmitSurveyResponse(int id, SurveyResponseRequest input)
    {
        var userId = CurrentUserId();
        var survey = await db.Surveys.FindAsync(id);
        if (survey is null) return NotFound();
        if (survey.Status != "Published") return BadRequest(new { message = "Survey is not published." });
        if (survey.ClosesAt is not null && survey.ClosesAt < DateTime.UtcNow) return BadRequest(new { message = "Survey is closed." });
        var existing = await db.SurveyResponses.FirstOrDefaultAsync(x => x.SurveyId == id && x.UserId == userId);
        if (existing is null) db.SurveyResponses.Add(new SurveyResponse { SurveyId = id, UserId = userId, ResponseJson = input.ResponseJson });
        else { existing.ResponseJson = input.ResponseJson; existing.SubmittedAt = DateTime.UtcNow; }
        await db.SaveChangesAsync();
        return Ok(new { message = "Survey response saved." });
    }

    [HttpGet("surveys/{id:int}/responses"), Authorize(Roles = Roles.Admin)]
    public async Task<object> SurveyResponses(int id) => await db.SurveyResponses.AsNoTracking().Include(x => x.User).Where(x => x.SurveyId == id)
        .OrderByDescending(x => x.SubmittedAt)
        .Select(x => new { x.Id, user = x.User.DisplayName, x.User.Username, x.ResponseJson, x.SubmittedAt })
        .ToListAsync();

    private int CurrentUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    private async Task<int> CurrentStudentId() => (await db.Students.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == CurrentUserId()))?.Id ?? 1;
    private async Task<int?> CurrentStaffIdOrNull() => (await db.Staff.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == CurrentUserId()))?.Id;
    private async Task<bool> FacultyOwnsSubject(int subjectId)
    {
        var staffId = await CurrentStaffIdOrNull();
        return staffId is not null && await db.SubjectFacultyAllocations.AnyAsync(x => x.StaffId == staffId && x.SubjectId == subjectId);
    }

    private async Task<IActionResult> Delete<T>(int id) where T : Entity
    {
        var row = await db.Set<T>().FindAsync(id);
        if (row is null) return NotFound();
        db.Remove(row);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
