using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace itec420.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DepartmentName { get; set; }

        public string DepartmentDescription { get; set; }

        public ICollection<Doctor> Doctors { get; set; }
    }
}
