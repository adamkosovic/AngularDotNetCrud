namespace TestPraktik.Models;

public class Quote
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public string UserId { get; set; }
    public User User { get; set; }

    public Quote() { }


    public Quote(string text, string author, User user)
    {
        this.Text = text;
        this.Author = author;
        this.CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
        this.User = user;
        this.UserId = user.Id;
    }
}
