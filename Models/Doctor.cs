using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace itec420.Models
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DoctorName { get; set; }

        public string PicOfDoc { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }

        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public ApplicationUser AppUser { get; set; }
        public Department Department { get; set; }

    }
}
