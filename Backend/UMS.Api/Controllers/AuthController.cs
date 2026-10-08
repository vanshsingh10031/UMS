using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UMS.Api.Data;
using UMS.Api.Models;
using UMS.Api.Services;

namespace UMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AppDbContext db, AuthService auth) : ControllerBase
{
    public record LoginRequest(string Username, string Password);
    public record ChangePasswordRequest(string CurrentPassword, string NewPassword);

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<object>> Login(LoginRequest request)
    {
        var user = await db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Username == request.Username || x.Email == request.Username);
        if (user is null || user.PasswordHash != request.Password)
            return Unauthorized(new { message = "Invalid username or password." });

        return Ok(new { token = auth.CreateToken(user), user = ToProfile(user) });
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<ActionResult<object>> ChangePassword(ChangePasswordRequest request)
    {
        var id = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
        var user = await db.Users.FirstAsync(x => x.Id == id);
        if (user.PasswordHash != request.CurrentPassword) return BadRequest(new { message = "Current password is incorrect." });
        if (string.IsNullOrWhiteSpace(request.NewPassword) || request.NewPassword.Length < 6) return BadRequest(new { message = "New password must be at least 6 characters." });
        user.PasswordHash = request.NewPassword;
        user.MustChangePassword = false;
        await db.SaveChangesAsync();
        return Ok(ToProfile(user));
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<object>> Me()
    {
        var id = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
        var user = await db.Users.AsNoTracking().FirstAsync(x => x.Id == id);
        return Ok(ToProfile(user));
    }

    private static object ToProfile(User user) => new
    {
        id = user.Id,
        username = user.Username,
        email = user.Email,
        role = user.Role.ToLowerInvariant(),
        name = user.DisplayName,
        title = user.Role,
        mustChangePassword = user.MustChangePassword,
        initials = string.Join("", user.DisplayName.Split(' ', StringSplitOptions.RemoveEmptyEntries).TakeLast(2).Select(x => x[0]))
    };
}
