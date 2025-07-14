
namespace TestPraktik;

using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<BookDbContext>(
            options => options.UseNpgsql("Host=localhost;Database=bookappdb;Username=postgres;Password=password"));
        builder.Services.AddControllers();
        builder.Services.AddScoped<BookService>();

        var app = builder.Build();

        app.MapControllers();
        app.UseHttpsRedirection();

        app.Run();
    }
}


public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public DateTime PublishDate { get; set; }

    public Book(string title, string author, DateTime publishDate)
    {
        this.Title = title;
        this.Author = author;
        this.PublishDate = DateTime.SpecifyKind(publishDate, DateTimeKind.Utc);
    }
}

public class BookDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }

    public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
    {
    }
}

public class CreateBookDto
{
    public string Title { get; set; }
    public string Author { get; set; }
    public DateTime PublishDate { get; set; }

    public CreateBookDto(string title, string author, DateTime publishDate)
    {
        this.Title = title;
        this.Author = author;
        this.PublishDate = publishDate;
    }
}

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public DateTime PublishDate { get; set; }

    public BookDto(Book book)
    {
        this.Id = book.Id;
        this.Title = book.Title;
        this.Author = book.Author;
        this.PublishDate = book.PublishDate;
    }
}


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
    public IActionResult CreateBook([FromBody] CreateBookDto dto)
    {
        try
        {
            Book book = bookService.CreateBook(dto.Title, dto.Author, dto.PublishDate);
            return Ok(book);
        }
        catch (ArgumentException)
        {
            return BadRequest();
        }
    }


    [HttpDelete("book/{id}")]
    public IActionResult RemoveBook(int id)
    {
        Book? book = bookService.RemoveBook(id);
        if (book == null)
        {
            return NotFound();
        }

        return Ok(book);
    }


    [HttpPut("book/{id}")]
    public IActionResult UpdateBook(int id, [FromBody] CreateBookDto dto)
    {
        Book? book = bookService.UpdateBook(id, dto);
        if (book == null)
        {
            return NotFound();
        }

        return Ok(new BookDto(book));
    }


    [HttpGet("books")]
    public List<BookDto> GetAllBooks()
    {
        return bookService.GetAllBooks().Select(book => new BookDto(book)).ToList();
    }
}


public class BookService
{
    private BookDbContext context;

    public BookService(BookDbContext context)
    {
        this.context = context;
    }

    public Book CreateBook(string title, string author, DateTime publishDate)
    {

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title must not be null or empty");
        }
        if (string.IsNullOrWhiteSpace(author))
        {
            throw new ArgumentException("Author must not be null or empty");
        }
        if (publishDate > DateTime.Now)
        {
            throw new ArgumentException("Publish date must be in the past");
        }

        Book book = new Book(title, author, publishDate);
        context.Books.Add(book);
        context.SaveChanges();
        return book;
    }

    public Book? RemoveBook(int id)
    {
        Book? book = context.Books.Find(id);
        if (book == null)
        {
            return null;
        }
        context.Books.Remove(book);
        context.SaveChanges();
        return book;
    }

    public Book? UpdateBook(int id, CreateBookDto dto)
    {
        var book = context.Books.Find(id);
        if (book == null)
        {
            return null;
        }

        book.Title = dto.Title;
        book.Author = dto.Author;
        book.PublishDate = DateTime.SpecifyKind(dto.PublishDate, DateTimeKind.Utc);

        context.SaveChanges();
        return book;
    }



       public List<Book> GetAllBooks()
    {
        return context.Books.ToList();
    }
}
