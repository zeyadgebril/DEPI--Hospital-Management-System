using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HospitalManagement.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
        public string Name { get; set; }

        public List<Doctor>? doctorList { get; set; }
        public List<Nurse>? nurseList { get; set; }
        public List<Room>? roomList { get; set; }
        
    }
}
