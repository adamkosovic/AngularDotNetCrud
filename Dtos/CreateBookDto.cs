namespace TestPraktik.Dtos;

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