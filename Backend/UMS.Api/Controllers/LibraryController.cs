using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UMS.Api.Data;
using UMS.Api.Models;

namespace UMS.Api.Controllers;

[ApiController]
[Route("api/library")]
[Authorize]
public class LibraryController(AppDbContext db) : ControllerBase
{
    [HttpGet("books")]
    public async Task<object> Books(string? search = null) => await db.LibraryBooks.AsNoTracking()
        .Where(x => search == null || x.Title.Contains(search) || x.Author.Contains(search) || x.Isbn.Contains(search))
        .Select(x => new { x.Id, x.Title, x.Author, code = x.Isbn, x.Category, available = x.AvailableCount > 0, x.TotalCount, x.AvailableCount })
        .ToListAsync();

    [HttpPost("books"), Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<LibraryBook>> CreateBook(LibraryBook input)
    {
        input.AvailableCount = input.AvailableCount == 0 ? input.TotalCount : input.AvailableCount;
        db.LibraryBooks.Add(input); await db.SaveChangesAsync(); return Created($"api/library/books/{input.Id}", input);
    }

    [HttpPut("books/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateBook(int id, LibraryBook input)
    {
        var row = await db.LibraryBooks.FindAsync(id);
        if (row is null) return NotFound();
        row.Title = input.Title; row.Author = input.Author; row.Isbn = input.Isbn; row.Category = input.Category; row.TotalCount = input.TotalCount; row.AvailableCount = input.AvailableCount;
        await db.SaveChangesAsync(); return NoContent();
    }

    [HttpDelete("books/{id:int}"), Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteBook(int id) { var row = await db.LibraryBooks.FindAsync(id); if (row is null) return NotFound(); db.Remove(row); await db.SaveChangesAsync(); return NoContent(); }

    [HttpPost("books/{id:int}/issue")]
    public async Task<IActionResult> Issue(int id)
    {
        var book = await db.LibraryBooks.FindAsync(id);
        if (book is null) return NotFound();
        if (book.AvailableCount <= 0) return Conflict(new { message = "No copies are available." });
        book.AvailableCount -= 1;
        db.LibraryTransactions.Add(new LibraryTransaction { LibraryBookId = id, UserId = CurrentUserId(), DueDate = DateTime.UtcNow.AddDays(14) });
        await db.SaveChangesAsync();
        return Ok(new { message = "Book issued." });
    }

    [HttpPost("transactions/{id:int}/return")]
    public async Task<IActionResult> Return(int id)
    {
        var tx = await db.LibraryTransactions.Include(x => x.LibraryBook).FirstOrDefaultAsync(x => x.Id == id);
        if (tx is null) return NotFound();
        if (tx.Status == "Returned") return Conflict(new { message = "Book already returned." });
        tx.Status = "Returned"; tx.ReturnDate = DateTime.UtcNow; tx.LibraryBook.AvailableCount += 1;
        await db.SaveChangesAsync();
        return Ok(new { message = "Book returned." });
    }

    [HttpGet("transactions")]
    public async Task<object> Transactions() => await db.LibraryTransactions.AsNoTracking().Include(x => x.LibraryBook).Where(x => x.UserId == CurrentUserId())
        .Select(x => new { x.Id, book = x.LibraryBook.Title, x.IssueDate, x.DueDate, x.ReturnDate, x.Status }).ToListAsync();

    private int CurrentUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
}
