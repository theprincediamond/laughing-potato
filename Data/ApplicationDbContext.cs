using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace itec420.Models;

public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<ApplicationUser> AppUsers { get; set; }
    public DbSet<IdentityRole> AppRoles { get; set; }
    public DbSet<Hospital> Hospitals { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Appointment> Appointments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, DepartmentName = "Cardiology", DepartmentDescription = "A" },
            new Department { Id = 2, DepartmentName = "Neurology", DepartmentDescription = "B" },
            new Department { Id = 3, DepartmentName = "Oncology", DepartmentDescription = "C" },
            new Department { Id = 4, DepartmentName = "Dermatology", DepartmentDescription = "D" }
        );

        modelBuilder.Entity<Doctor>().HasData(
            new Doctor { Id = 1, DoctorName = "Michael Jordan", DepartmentId = 1, PicOfDoc = "/img/1.png" },
            new Doctor { Id = 2, DoctorName = "Jalen Brunson", DepartmentId = 2, PicOfDoc = "/img/2.png" },
            new Doctor { Id = 3, DoctorName = "Lebron James", DepartmentId = 3, PicOfDoc = "/img/3.png" }
        );

        modelBuilder.Entity<Department>()
            .HasMany(d => d.Doctors)
            .WithOne()
            .HasForeignKey(d => d.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Doctor>()
            .HasOne(d => d.AppUser)
            .WithOne()
            .HasForeignKey<Doctor>(d => d.AppUserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.AppUser)
            .WithMany()
            .HasForeignKey(a => a.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Doctor)
            .WithMany()
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.AppUser)
            .WithMany()
            .HasForeignKey(r => r.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Hospital)
            .WithMany() 
            .HasForeignKey(r => r.HospitalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
