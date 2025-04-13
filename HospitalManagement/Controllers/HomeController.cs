using System.Diagnostics;
using HospitalManagement.Models;
using HospitalManagement.Repositoty;
using HospitalManagement.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        UserManager<ApplicationUser> userManager;
        IPatientRepository patienttRepo;
        IDoctorRepository doctorRepo;
        IDepartmentRepository deptRepo;
        IStaffRepository staffRepo;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> _userManager,
                                         IPatientRepository _patienttRepo,
                                         IDoctorRepository _doctorRepo,
                                         IStaffRepository _StaffRepo)
        {
            _logger = logger;
            userManager = _userManager;
            patienttRepo = _patienttRepo;
            doctorRepo = _doctorRepo;
            staffRepo = _StaffRepo;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Privacy()
        {
            ApplicationUser user = await userManager.GetUserAsync(User);

            if (User.IsInRole("Admin"))
            {
                Staff staff = staffRepo.GetStaffByAppUserId(user.Id);
                if (staff != null) 
                {
                    PrivacyVM<Staff> staffData = new PrivacyVM<Staff>();
                    staffData.Object = staff;
                    staffData.user = user;
                    return View("Privacy", staffData);
                    
                }
            }
            if (User.IsInRole("Staff"))
            {
                Staff staff = staffRepo.GetStaffByAppUserId(user.Id);
                if (staff != null) 
                {
                    PrivacyVM<Staff> staffData = new PrivacyVM<Staff>();
                    staffData.Object = staff;
                    staffData.user = user;
                    return View("Privacy", staffData);
                }
            }
            if(User.IsInRole("Patient"))
            {
                Patient patient = patienttRepo.GetByUserId(user.Id);
                if (patient != null)
                {
                    PrivacyVM<Patient> patientData = new PrivacyVM<Patient>();
                    patientData.Object = patient;
                    patientData.user = user;
                    return View("Privacy", patientData); 
                }

            }
            if(User.IsInRole("Doctor"))
            {
                Doctor doctor = doctorRepo.GetDoctorByAppUserId(user.Id);
                if (doctor != null)
                {
                    PrivacyVM<Doctor> DoctorData = new PrivacyVM<Doctor>();
                    DoctorData.Object = doctor;
                    DoctorData.user = user;
                    return View("Privacy", DoctorData);  
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Signup()
        {
            return View();
        }

        public ActionResult ForgetPassword()
        {
            return View();
        }
    }
}
