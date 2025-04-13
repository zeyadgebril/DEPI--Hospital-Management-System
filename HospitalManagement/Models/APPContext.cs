using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HospitalManagement.Models
{
    public class APPContext : IdentityDbContext<ApplicationUser>//DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Nurse> Nurses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalReport> MedicalReports { get; set; }
        public DbSet<Billing> Billings { get; set; }
        public DbSet<Staff> Staffs { get; set; }

        public APPContext(DbContextOptions options) : base(options)
        {
        
        }
        
    }
}
