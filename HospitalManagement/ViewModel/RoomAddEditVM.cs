using HospitalManagement.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.ViewModel
{
    public class RoomAddEditVM
    {
        public int Id { get; set; }
        public string RoomName { get; set; }

        public int DepartmentId { get; set; }
        public List<Department>? deptList { get; set; }
    }
}
