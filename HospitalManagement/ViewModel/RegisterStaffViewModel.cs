using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.ViewModel
{
    public class RegisterStaffViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string ContactInfo { get; set; }

        public string Address { get; set; }

        public string Role { get; set; } // "Staff" أو "Admin"
        public string? Image { get; set; }
    }
}

