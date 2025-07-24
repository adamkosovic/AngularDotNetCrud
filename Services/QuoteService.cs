using TestPraktik.Data;
using TestPraktik.Models;
using TestPraktik.Dtos;


namespace TestPraktik.Services;

public class QuoteService
{
    private readonly BookDbContext db;

    public QuoteService(BookDbContext db)
    {
        this.db = db;
    }

    public Quote CreateQuote(string text, string author, string userId)
    {
        var quote = new Quote
        {
            Text = text,
            Author = author,
            UserId = userId,
            CreatedAt = DateTime.Today // Use today's date since the column is 'date' type
        };
        db.Quotes.Add(quote);
        db.SaveChanges();
        return quote;
    }

    public List<Quote> GetAllQuotes(string userId)
    {
        return db.Quotes.Where(q => q.UserId == userId).ToList();
    }

    public Quote? GetQuoteById(int id, string userId)
    {
        return db.Quotes.FirstOrDefault(q => q.Id == id && q.UserId == userId);
    }

    public Quote? UpdateQuote(int id, CreateQuoteDto dto, string userId)
    {
        var quote = GetQuoteById(id, userId);
        if (quote == null) return null;

        quote.Text = dto.Text;
        quote.Author = dto.Author;
        db.SaveChanges();

        return quote;
    }

    public Quote? RemoveQuote(int id, string userId)
    {
        var quote = GetQuoteById(id, userId);
        if (quote == null) return null;

        db.Quotes.Remove(quote);
        db.SaveChanges();

        return quote;
    }
}
