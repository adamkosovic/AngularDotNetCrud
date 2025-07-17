
namespace TestPraktik.Services;

using TestPraktik.Models;
using TestPraktik.Data;
using TestPraktik.Dtos;

public class BookService
{
  private BookDbContext context;

  public BookService(BookDbContext context)
  {
    this.context = context;
  }

  public Book CreateBook(string title, string author, DateTime publishDate, string id)
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

    User? user = context.Users.Find(id);
    if (user == null)
    {
      throw new ArgumentException("User not found");
    }

    Book book = new Book(title, author, publishDate, user);
    context.Books.Add(book);
    user.Books.Add(book);
    context.SaveChanges();
    return book;
  }

  public Book? RemoveBook(int id, string userId)
  {

    User? user = context.Users.Find(userId);
    if (user == null)
    {
      throw new ArgumentException("User not found");
    }

    List<Book> books = context.Books.Where(book => book.User.Id == userId && book.Id == id).ToList();
    if (books.Count == 0)
    {
      return null;
    }

    Book book = books[0];

    context.Books.Remove(book);
    context.SaveChanges();

    return book;
  }

  public Book? UpdateBook(int id, CreateBookDto dto, string userId)
  {
    User? user = context.Users.Find(userId);
    if (user == null)
    {
      throw new ArgumentException("User not found");
    }

    List<Book> books = context.Books.Where(book => book.User.Id == userId && book.Id == id).ToList();
    if (books.Count == 0)
    {
      return null;
    }

    Book book = books[0];
    book.Title = dto.Title;
    book.Author = dto.Author;
    book.PublishDate = DateTime.SpecifyKind(dto.PublishDate, DateTimeKind.Utc);

    context.SaveChanges();
    return book;
  }



  public List<Book> GetAllBooks(string userId)
  {
    return context.Books
    .Where(book => book.User.Id == userId)
    .ToList();
  }

  public Book? GetBookById(int id, string userId)
  {
    return context.Books.FirstOrDefault(b => b.Id == id && b.User.Id == userId);
  }
}