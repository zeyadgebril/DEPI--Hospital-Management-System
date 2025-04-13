using HospitalManagement.Models;
using HospitalManagement.Repositoty;
using HospitalManagement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace HospitalManagement.Controllers
{
    public class AppointmentDateController : Controller
    {
        IAppointmentRepository appointmentRepo;
        IPatientRepository patientRepo;
        IDoctorRepository doctorRepo;
        IDepartmentRepository deptRepo;
        UserManager<ApplicationUser> userManager;
        public AppointmentDateController(IAppointmentRepository _appointmentRepo,
                                         UserManager<ApplicationUser> _userManager,
                                         IPatientRepository _petRepo,
                                         IDoctorRepository _doctorRepo,
                                         IDepartmentRepository _deptRepo) 
        {
            appointmentRepo = _appointmentRepo;
            userManager = _userManager;
            doctorRepo = _doctorRepo;
            deptRepo = _deptRepo;
            patientRepo = _petRepo;
        }
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 5, string searchTerm = "")
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) 
            {
                return RedirectToAction("Login", "Account");
            }
            var roles = await userManager.GetRolesAsync(user);
            if (pageNumber < 1) pageNumber = 1;
            List<Appointment> appointments;
            
            int totalRecord;

            // التحقق من الدور لعرض المواعيد بناءً عليه
            if (roles.Contains("Patient"))
            {
                var patient = patientRepo.GetByUserId(user.Id);
                if (patient == null)
                {
                    return RedirectToAction("Add", "Patient");
                }
                appointments = appointmentRepo.GetAppointmentByPatientIdPagination(patient.Id, pageNumber, pageSize, searchTerm);  // مواعيد المريض
                 totalRecord = appointmentRepo.GetAppointmentCountByPatientId(patient.Id, searchTerm);

            }
            else if (roles.Contains("Doctor"))
            {
                var doctor = doctorRepo.GetDoctorByAppUserId(user.Id);
                appointments = appointmentRepo.GetAppointmentByDoctorIdPagination(doctor.Id, pageNumber, pageSize, searchTerm);  // مواعيد الطبيب
                totalRecord = appointmentRepo.GetAppointmentCountByDoctorId(doctor.Id, searchTerm);
            }
            else if (roles.Contains("Staff")|| roles.Contains("Admin"))
            {
                appointments = appointmentRepo.GetAllPagination(pageNumber, pageSize, searchTerm);
                totalRecord = appointmentRepo.GetAllAppointmentCount(searchTerm);
            }
            else
            {
                return RedirectToAction("Login", "Account"); // في حال لم يكن هناك دور معروف
            }

            int totalPages = (int)Math.Ceiling((double)totalRecord / pageSize);

            if (pageNumber < 1) pageNumber = 1;
            if (pageNumber > totalPages && totalPages > 0) pageNumber = totalPages;

            // تجهيز ViewModel للـ pagination
            var viewModel = new PaginationViewModel<Appointment>
            {
                Items = appointments,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecord,
                TotalPages = totalPages,
                SearchTerm = searchTerm
            };

            return View("GetAll", viewModel);
        }

        [Authorize(Roles = "Patient")]
        [HttpGet]
        public IActionResult Add ()
        { 
            AppointmentDateVM dataModel = new AppointmentDateVM();
            var userId = userManager.GetUserId(User);
            var patient = patientRepo.GetByUserId(userId);
            //ViewBag.PatientName = patient.Name;
            dataModel.DoctorList = doctorRepo.GetAll();
            dataModel.DeptList = deptRepo.GetAll();
            return View("Add", dataModel);
        }


        [HttpPost]
        [Authorize(Roles = "Patient")]
        public IActionResult SaveAdd(AppointmentDateVM dataFromReq)
        {
            if (ModelState.IsValid) 
            {
                var userId = userManager.GetUserId(User);
                var patient = patientRepo.GetByUserId(userId);

                Appointment dataFromDB = new Appointment();
                dataFromDB.AppointmentDate = dataFromReq.AppointmentDate;
                dataFromDB.PatientId = patient.Id;
                dataFromDB.DoctorId = dataFromReq.DoctorId;
                appointmentRepo.Add(dataFromDB);
                appointmentRepo.Save();
                return RedirectToAction("GetAll");
            }
            dataFromReq.DoctorList = doctorRepo.GetAll();
            return View("Add", dataFromReq);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var userId = userManager.GetUserId(User);
            var patient = patientRepo.GetByUserId(userId);
            ViewBag.PatientName = patient.Name;
            
            Appointment appointment = appointmentRepo.GetById(id);
            List<Doctor> doctorsList = doctorRepo.GetAll();
            List<Department> deptList = deptRepo.GetAll();

            AppointmentDateVM appointmentDateVM = new AppointmentDateVM();
            appointmentDateVM.Id = appointment.Id;
            appointmentDateVM.AppointmentDate = appointment.AppointmentDate;
            appointmentDateVM.PatientId = patient.Id;
            appointmentDateVM.DoctorId = appointment.DoctorId;
            appointmentDateVM.DoctorList = doctorsList;
            appointmentDateVM.DeptList = deptList;

            return View("Edit", appointmentDateVM);
        }
        [HttpPost]
        public IActionResult SaveEdit(AppointmentDateVM dataFromReq, int id) 
        {
            if(ModelState.IsValid)
            {
                var userId = userManager.GetUserId(User);
                var patient = patientRepo.GetByUserId(userId);
                Appointment appointment = appointmentRepo.GetById(id);

                appointment.Id = dataFromReq.Id;
                appointment.AppointmentDate = dataFromReq.AppointmentDate;
                appointment.PatientId = patient.Id;
                appointment.DoctorId = dataFromReq.DoctorId;
                appointmentRepo.Update(appointment);
                appointmentRepo.Save();
                return RedirectToAction("MyAppointments");
            }
            var _userId = userManager.GetUserId(User);
            var _patient = patientRepo.GetByUserId(_userId);
            ViewBag.PatientName = _patient.Name;

            dataFromReq.DoctorList = doctorRepo.GetAll();
            dataFromReq.DeptList = deptRepo.GetAll();
            return View("Edit", dataFromReq);
        }

        public IActionResult Details(int id)
        {
            Appointment appointment = appointmentRepo.GetById(id);
            if (appointment == null)
            {
                return View("Error");
            }
            return View("Details", appointment);
        }

        public ActionResult Delete(int id) 
        {
            try
            {
                appointmentRepo.Delete(id);
                appointmentRepo.Save();
                return Json(new { success = true, message = "Appointment data deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Failed to delete Appointment da." });
            }
        }
        public IActionResult GetDoctorByDeptId(int deptId)
        {
            List<Doctor> doctorList = doctorRepo.GetByDeptId(deptId);
            return Json(doctorList);
        }
        [Authorize(Roles = "Patient")]
        public IActionResult MyAppointments()
        {
            var userId = userManager.GetUserId(User);
            var patient = patientRepo.GetByUserId(userId);
            if (patient == null)
            {
                return RedirectToAction("ADd", patient);
            }
            List<Appointment> appointments = appointmentRepo.GetByPatientId(patient.Id);
            return View("MyAppointments", appointments);
        }
    }
}
