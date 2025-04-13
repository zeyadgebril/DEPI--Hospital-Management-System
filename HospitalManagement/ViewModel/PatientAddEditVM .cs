using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.ViewModel
{
    public class PAtientAddEditeVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Contact info is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(15)]
        public string ContactInfo { get; set; }
        [StringLength(200)]
        public string Address { get; set; }
        public string? Image { get; set; }

    }
}
