using Microsoft.AspNetCore.Identity;

namespace itec420.Models;

public class AppRole : IdentityRole<int>
{
    public ICollection<AppUser> Users { get; set; }
}