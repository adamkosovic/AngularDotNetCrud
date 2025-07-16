using Microsoft.AspNetCore.Identity;

namespace TestPraktik.Models;

public class User : IdentityUser
{
  public List<Book> Books { get; set; }

  public User() { }
}
