
using Microsoft.AspNetCore.Identity;

namespace HospitalManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        
        public Doctor? Doctor { get; set; }
        public Patient? Patient { get; set; }
        public Staff? Staff { get; set; }

    }
}
