using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TestPraktik.Models;
using TestPraktik.Services;
using TestPraktik.Dtos;

namespace TestPraktik.Controllers;

[ApiController]
[Route("api")]
public class QuoteController : ControllerBase
{
    private readonly QuoteService quoteService;

    public QuoteController(QuoteService quoteService)
    {
        this.quoteService = quoteService;
    }

    [HttpPost("quote")]
    // [Authorize]
    public IActionResult CreateQuote([FromBody] CreateQuoteDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "test-user-id";

        if (userId == null) return Unauthorized();

        var quote = quoteService.CreateQuote(dto.Text, dto.Author, userId);
        return Ok(new QuoteDto(quote));
    }

    [HttpGet("quotes")]
    // [Authorize]
    public List<QuoteDto> GetAllQuotes()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "test-user-id";
        return quoteService.GetAllQuotes(userId).Select(q => new QuoteDto(q)).ToList();
    }

    [HttpGet("quote/{id}")]
    // [Authorize]
    public IActionResult GetQuoteById(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "test-user-id";
        var quote = quoteService.GetQuoteById(id, userId);
        if (quote == null) return NotFound();
        return Ok(new QuoteDto(quote));
    }

    [HttpPut("quote/{id}")]
    // [Authorize]
    public IActionResult UpdateQuote(int id, [FromBody] CreateQuoteDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "test-user-id";
        var quote = quoteService.UpdateQuote(id, dto, userId);
        if (quote == null) return NotFound();
        return Ok(new QuoteDto(quote));
    }

    [HttpDelete("quote/{id}")]
    // [Authorize]
    public IActionResult DeleteQuote(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "test-user-id";
        var quote = quoteService.RemoveQuote(id, userId);
        if (quote == null) return NotFound();
        return Ok(new QuoteDto(quote));
    }
}
