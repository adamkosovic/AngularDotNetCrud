namespace TestPraktik.Models;


public class Book
{
  public int Id { get; set; }
  public string Title { get; set; }
  public string Author { get; set; }
  public DateTime PublishDate { get; set; }

  public string UserId { get; set; }
  public User User { get; set; }

  public Book() { }

  public Book(string title, string author, DateTime publishDate, User user)
  {
    this.Title = title;
    this.Author = author;
    this.PublishDate = DateTime.SpecifyKind(publishDate, DateTimeKind.Utc);
    this.User = user;
    this.UserId = user.Id;
  }
}