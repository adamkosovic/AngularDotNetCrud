using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TestPraktik.Services;
using TestPraktik.Models;
using TestPraktik.Dtos;

namespace TestPraktik.Controllers;

[ApiController]
[Route("api")]
public class BookController : ControllerBase
{
  private BookService bookService;
  public BookController(BookService bookService)
  {
    this.bookService = bookService;
  }


  [HttpPost("book")]
  // [Authorize]  // Temporarily remove authentication for testing
  public IActionResult CreateBook([FromBody] CreateBookDto dto)
  {
    try
    {
      // For testing, use a default user ID
      var id = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "test-user-id";
      if (id == null)
      {
        return Unauthorized("User not authenticated");
      }

      Book book = bookService.CreateBook(dto.Title, dto.Author, dto.PublishDate, id);

      BookDto bookDto = new BookDto(book);
      return Ok(bookDto);
    }
    catch (ArgumentException)
    {
      return BadRequest();
    }
  }


  [HttpDelete("book/{id}")]
  // [Authorize]  // Temporarily remove authentication for testing
  public IActionResult RemoveBook(int id)
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "test-user-id";
    Book? book = bookService.RemoveBook(id, userId);
    if (book == null)
    {
      return NotFound();
    }

    BookDto bookDto = new BookDto(book);
    return Ok(bookDto);
  }


  [HttpPut("book/{id}")]
  // [Authorize]  // Temporarily remove authentication for testing
  public IActionResult UpdateBook(int id, [FromBody] CreateBookDto dto)
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "test-user-id";
    if (userId == null)
    {
      return NotFound();
    }

    Book? book = bookService.UpdateBook(id, dto, userId);
    if (book == null)
    {
      return NotFound();
    }

    BookDto bookDto = new BookDto(book);
    return Ok(bookDto);
  }


  [HttpGet("books")]
  // [Authorize]  // Temporarily remove authentication for testing
  public List<BookDto> GetAllBooks()
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "test-user-id";
    return bookService.GetAllBooks(userId).Select(book => new BookDto(book)).ToList();
  }

  [HttpGet("book/{id}")]
  // [Authorize]  // Temporarily remove authentication for testing
  public IActionResult GetBookById(int id)
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "test-user-id";

    if (userId == null)
      return Unauthorized();

    var book = bookService.GetBookById(id, userId);

    if (book == null)
      return NotFound();

    return Ok(new BookDto(book));
  }
}