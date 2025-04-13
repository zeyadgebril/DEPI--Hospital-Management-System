using HospitalManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.ViewModel
{
    public class MedicalReportAddEditeVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Report Date")]
        public DateTime ReportDate { get; set; } = DateTime.Now;
        [Required]
        [Display(Name = "Diagnosis")]
        public string Diagnosis { get; set; }
        [Required]
        [Display(Name = "Prescription")]
        public string Prescription { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [Required]
        [Display(Name = "Patient")]
        public int PatientId { get; set; }

        [Required]
        [Display(Name = "Doctor")]
        public int DoctorId { get; set; }

        public List<Patient>? PatientList { get; set; }
        public List<Doctor>? DoctorList { get; set; }
    }
}