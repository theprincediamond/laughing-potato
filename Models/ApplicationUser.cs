using Microsoft.AspNetCore.Identity;

namespace itec420.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        public ICollection<Appointment> Appointments { get; set; }

        // Если в коде используется ApplicationUser.Roles
        public ICollection<IdentityRole> Roles { get; set; }
    }
}
