using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models
{
    public class MedicalReport
    {
        [Key]
        public int Id { get; set; }
        public DateTime ReportDate { get; set; }
        public string Diagnosis { get; set; }
        public string Prescription { get; set; }
        public string Notes { get; set; }
       

        [ForeignKey("Patient")]
        public int patientId { get; set; }
        public Patient? Patient { get; set; }

        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
