using System.ComponentModel.DataAnnotations;

namespace itec420.Models
{
    public class Hospital
    {
        [Key]
        public int Id { get; set; }

        public string HospitalName { get; set; }

        public string Location { get; set; }

        // Навигационное свойство
        public ICollection<Department> Departments { get; set; }
    }
}
