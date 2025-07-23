using TestPraktik.Models;

namespace TestPraktik.Dtos;

public class QuoteDto
{
    public int Id { get; set; }
    public string Text { get; set; }
    public string Author { get; set; }

    public QuoteDto(Quote quote)
    {
        Id = quote.Id;
        Text = quote.Text;
        Author = quote.Author;
    }
}
