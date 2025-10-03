using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using itec420.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace itec420.Controllers;

[Authorize]
public class AppointmentController : Controller
{
    private const int V = 1;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public AppointmentController(ApplicationDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    public DateTime AppointmentDate { get; set; }
public TimeSpan Time { get; set; }
    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId;
        List<Appointment> appointments;

        if (User.IsInRole("Patient"))
        {

            appointments = await _context.Appointments
                .Include(a => a.Doctor)
                .ToListAsync();
        }
        else if (User.IsInRole("Doctor"))
        {
            appointments = await _context.Appointments
                .Include(a => a.AppUser)
                .ToListAsync();
        }
        else
        {
            appointments = new List<Appointment>();
        }

        return View(appointments);
    }

    [Authorize(Roles = "Patient")]
    public IActionResult CreateAppointment()
    {
        ViewBag.Doctors = _context.Doctors.ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> CreateAppointment(Appointment appointment, string userId)
    {
        appointment.AppUserId = userId;

        if (ModelState.IsValid)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        ViewBag.Doctors = _context.Doctors.ToList();
        return View(appointment);
    }

    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> EditAppointment(int id)
    {
        string userId =User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == id && a.AppUserId == userId);
        if (appointment == null)
        {
            return RedirectToAction("Index");
        }

        ViewBag.Doctors = _context.Doctors.ToList();
        return View(appointment);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> EditAppointment(Appointment updated)
    {
        var userId =User.FindFirstValue(ClaimTypes.NameIdentifier);
        var existing = await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == updated.AppointmentId && a.AppUserId == userId);
        if (existing == null)
        {
            return NotFound();
        }

        existing.DoctorId = updated.DoctorId;
        existing.AppointmentDate = updated.AppointmentDate;
        existing.Time = updated.Time;

        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Patient,Doctor")]
    public async Task<IActionResult> DeleteAppointment(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        bool isPatient = User.IsInRole("Patient");
        bool isDoctor = User.IsInRole("Doctor");

        int? doctorId = null;

        if (isDoctor)
        {
            doctorId = await _context.Doctors
                .Select(d => d.Id) 
                .FirstOrDefaultAsync();
        }

        var appointment = await _context.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.AppUser)
            .FirstOrDefaultAsync(a =>
                a.AppointmentId == id &&
                ((isPatient && a.AppUserId == userId)) || (isDoctor && a.DoctorId == doctorId)
            );

        if (appointment == null)
        {
            return NotFound();
        }

        return View(appointment);
    }


    [HttpPost, ActionName("DeleteAppointment")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Patient,Doctor")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        bool isPatient = User.IsInRole("Patient");
        bool isDoctor = User.IsInRole("Doctor");

        int? doctorId = null;

        if (isDoctor)
        {
            doctorId = await _context.Doctors
                .Where(d => d.AppUserId == userId)
                .Select(d => d.Id)
                .FirstOrDefaultAsync();
        }

        var appointment = await _context.Appointments.FindAsync(id);

        if (appointment == null ||
            (isPatient && appointment.AppUserId != userId) ||
            (isDoctor && appointment.DoctorId != doctorId))
        {
            return Unauthorized();
        }

        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }



    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> ShowAppointments()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var doctor = await _context.Doctors
            .FirstOrDefaultAsync(d => d.AppUserId == userId);

        if (doctor == null)
        {
            return NotFound("Doctor profile not found.");
        }

        var appointments = await _context.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.AppUser)
            .Where(a => a.DoctorId == doctor.Id)
            .ToListAsync();


        return View(appointments);
    }
}
