using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specialty { get; set; }
        public string Email { get; set; }
        public string ContactInfo { get; set; }
        public string ExperienceYears { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; } 


        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public List<MedicalReport>? medicalReportList { get; set; }
        public List<Appointment>? appointmentList { get; set; }


        public string? ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser? ApplicationUser { get; set; }

    }
}
