using Microsoft.AspNetCore.Identity;

namespace TestPraktik.Models;

public class User : IdentityUser
{
  public List<Book> Books { get; set; } = new List<Book>();

  public User() { }
}
