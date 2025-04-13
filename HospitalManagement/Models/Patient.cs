using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.Models
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string ContactInfo { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }

        public List<MedicalReport> medicalReportList { get; set; }
        public List<Appointment> appointmentList { get; set; }
        public List<Billing> billingList { get; set; }



        public string? ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
