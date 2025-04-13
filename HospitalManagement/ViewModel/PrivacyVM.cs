using HospitalManagement.Models;
using Microsoft.AspNetCore.Identity;

namespace HospitalManagement.ViewModel
{
    public class PrivacyVM<T>
    {
        public T Object { get; set; }
        public ApplicationUser user { get; set; }
    }
}
