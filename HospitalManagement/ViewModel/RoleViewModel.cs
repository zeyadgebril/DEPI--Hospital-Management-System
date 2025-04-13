using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.ViewModel
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "Role name is required.")]
        [StringLength(100, ErrorMessage = "Role name can't be longer than 20 characters.")]
        public string RoleName { get; set; }
    }
}
