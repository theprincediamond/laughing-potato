using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace itec420.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        public DateTime ReservationDate { get; set; }

        [ForeignKey("Hospital")]
        public int HospitalId { get; set; }
        public Hospital Hospital { get; set; }

        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }
        public ApplicationUser AppUser { get; set; }
    }
}
