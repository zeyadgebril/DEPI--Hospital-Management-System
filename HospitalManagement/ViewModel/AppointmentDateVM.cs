using HospitalManagement.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.ViewModel
{
    public class AppointmentDateVM
    {
        public int Id { get; set; }
        [Required]
        public DateTime AppointmentDate { get; set; } = DateTime.Now; 
        [Required]
        public int PatientId { get; set; }
        [Required]
        public int DoctorId { get; set; }
        public List<Doctor>? DoctorList { get; set; }

        
        public List<Department>? DeptList{ get; set; }
    }
}
