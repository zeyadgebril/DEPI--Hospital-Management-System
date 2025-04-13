using HospitalManagement.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.ViewModel
{
    public class Nurese_AddEditVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactInfo { get; set; }
        public int ExperienceYears { get; set; }
        public string Image { get; set; }
        public int DepartmentId { get; set; }
        public List<Department>? DeptList { get; set; }
    }

}
