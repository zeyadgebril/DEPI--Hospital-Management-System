using HospitalManagement.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagement.ViewModel
{
    public class DoctorWithDepartmentAddEditeVM
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100,MinimumLength =3,ErrorMessage = "Name must be between 3 and 100 characters.")]
        public string Name { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Specialty cannot exceed 50 characters.")]
        public string Specialty { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Contact information is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string ContactInfo { get; set; }
        public string? ExperienceYears { get; set; }
        public string? Image { get; set; }
        [Required(ErrorMessage = "Department is required.")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        // Used for dropdown list selection in the View
        public List<Department>? deptList { get; set; }


        // to user
        public string? UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
