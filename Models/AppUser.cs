using Microsoft.AspNetCore.Identity;

namespace itec420.Models;

public class AppUser : IdentityUser
{
    public string Fullname { get; set; } = null!;
    public ICollection<AppRole> Roles { get; set; }

}