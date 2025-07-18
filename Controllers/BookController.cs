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
  [Authorize("create_book")]
  public IActionResult CreateBook([FromBody] CreateBookDto dto)
  {
    try
    {
      var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
  [Authorize("remove_book")]
  public IActionResult RemoveBook(int id)
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    Book? book = bookService.RemoveBook(id, userId);
    if (book == null)
    {
      return NotFound();
    }

    BookDto bookDto = new BookDto(book);
    return Ok(bookDto);
  }


  [HttpPut("book/{id}")]
  [Authorize("update_book")]
  public IActionResult UpdateBook(int id, [FromBody] CreateBookDto dto)
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
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
  [Authorize("get_books")]
  public List<BookDto> GetAllBooks()
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    return bookService.GetAllBooks(userId).Select(book => new BookDto(book)).ToList();
  }

  [HttpGet("book/{id}")]
  [Authorize("get_book")]
  public IActionResult GetBookById(int id)
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


    if (userId == null)
      return Unauthorized();

    var book = bookService.GetBookById(id, userId);

    if (book == null)
      return NotFound();

    return Ok(new BookDto(book));
  }
}