
using TestPraktik.Models;

namespace TestPraktik.Dtos;

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