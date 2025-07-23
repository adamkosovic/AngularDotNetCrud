using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestPraktik.Models;

namespace TestPraktik.Data;

public class BookDbContext : IdentityDbContext<User>
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Quote> Quotes { get; set; }

    public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Quote>()
            .HasOne(q => q.User)
            .WithMany(u => u.Quotes)
            .HasForeignKey(q => q.UserId)
            .OnDelete(DeleteBehavior.Cascade);        
    }
}