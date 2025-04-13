using HospitalManagement.Models;
using HospitalManagement.Repositoty;
using HospitalManagement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace HospitalManagement.Controllers
{
    public class MedicalReportController : Controller
    {
        IMedicalReportRepository medicalReportRepo;
        IPatientRepository patientRepo;
        IDoctorRepository doctorRepo;
        IDepartmentRepository deptRepo;
        UserManager<ApplicationUser> userManager;
        public MedicalReportController(IMedicalReportRepository _medicalReportRepo,
                                    IPatientRepository _patientRepo,
                                    IDoctorRepository _doctorRepo,
                                    IDepartmentRepository _deptRepo,
                                    UserManager<ApplicationUser> _userManager)
        {
            medicalReportRepo = _medicalReportRepo;
            patientRepo = _patientRepo;
            doctorRepo = _doctorRepo;
            deptRepo = _deptRepo;
            userManager = _userManager;
        }
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize = 5, string searchTerm = "")
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) {
                return RedirectToAction("Login", "Account");
            }
            var Role = await userManager.GetRolesAsync(user);
            if (pageNumber < 1) pageNumber = 1;
            List<MedicalReport> reports;
            
            int totalRecord;
            
            if (Role.Contains("Doctor"))
            {
                Doctor doctor = doctorRepo.GetDoctorByAppUserId(user.Id);
                reports = medicalReportRepo.GetReportsByDoctorIdPagination(doctor.Id, pageNumber, pageSize, searchTerm);
                totalRecord = medicalReportRepo.GetReportsCountByDoctorId(doctor.Id, searchTerm);

            }
            else if(Role.Contains("Patient"))
            {
                Patient patient = patientRepo.GetByUserId(user.Id);
                reports = medicalReportRepo.GetReportsByPatientIdPagination(patient.Id, pageNumber, pageSize, searchTerm);
                totalRecord = medicalReportRepo.GetReportsCountByPatientId(patient.Id, searchTerm);
            }
            else if(Role.Contains("Stuff")|| Role.Contains("Admin"))
            {
                reports = medicalReportRepo.GetAllPagination(pageNumber, pageSize, searchTerm);
                totalRecord = medicalReportRepo.GetAllReportsCount(searchTerm);
            }
            else
            {
                return RedirectToAction("login","Account");
            }
           
            int totalPage = (int)Math.Ceiling((double)totalRecord / pageSize);

            if (pageNumber < 1) pageNumber = 1;
            if (pageNumber > totalPage && totalPage > 0) pageNumber = totalPage;

            //Mapping
            var viewModel = new PaginationViewModel<MedicalReport>();
            viewModel.Items = reports;
            viewModel.CurrentPage = pageNumber;
            viewModel.PageSize = pageSize;
            viewModel.TotalRecords = totalRecord;
            viewModel.TotalPages = totalPage;
            viewModel.SearchTerm = searchTerm;

            return View("GetAll", viewModel);
        }



        [Authorize(Roles ="Doctor")]
        [HttpGet]
        public IActionResult Add()
        {

            MedicalReportAddEditeVM dataModel = new MedicalReportAddEditeVM();
            dataModel.PatientList =patientRepo.GetAll();
            dataModel.DoctorList = doctorRepo.GetAll();
            //dataModel.DeptList = deptRepo.GetAll();
            return View("Add", dataModel);
        }
        [HttpPost]
        public async Task<IActionResult> SaveAdd(MedicalReportAddEditeVM dataFromRq)
        {
            if (ModelState.IsValid) 
            {
                var user = await userManager.GetUserAsync(User);
                Doctor doctor = doctorRepo.GetDoctorByAppUserId(user.Id);

                MedicalReport dataFromDB = new MedicalReport();
                dataFromDB.ReportDate = DateTime.Now;
                dataFromDB.Diagnosis = dataFromRq.Diagnosis;
                dataFromDB.Prescription = dataFromRq.Prescription;
                dataFromDB.Notes = dataFromRq.Notes;
                dataFromDB.patientId = dataFromRq.PatientId;
                dataFromDB.DoctorId =doctor.Id;
                
                medicalReportRepo.Add(dataFromDB);
                medicalReportRepo.Save();
                return RedirectToAction("GetAll");
            }
            dataFromRq.PatientList = patientRepo.GetAll();
            return View("Add",dataFromRq);   
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await userManager.GetUserAsync(User);
            Doctor doctor = doctorRepo.GetDoctorByAppUserId(user.Id);

            MedicalReport medical =medicalReportRepo.GetById(id);

            MedicalReportAddEditeVM  medicalReportVM = new MedicalReportAddEditeVM();
            medicalReportVM.PatientList = patientRepo.GetAll();
            medicalReportVM.Id = medical.Id;
            medicalReportVM.ReportDate = medical.ReportDate;
            medicalReportVM.Diagnosis = medical.Diagnosis;
            medicalReportVM.Prescription= medical.Prescription;
            medicalReportVM.Notes = medical.Notes;
            medicalReportVM.PatientId = medical.patientId;
            medicalReportVM.DoctorId = doctor.Id;
            return View("Edit",medicalReportVM);
        }
        [HttpPost]
        public async Task<IActionResult> SaveEdit(MedicalReportAddEditeVM dataFromReq,int id)
        {
            if (ModelState.IsValid)
            {
                var user =  await userManager.GetUserAsync(User);
                Doctor doctor = doctorRepo.GetDoctorByAppUserId(user.Id);

                MedicalReport dataFromDB = medicalReportRepo.GetById(dataFromReq.Id);
                dataFromDB.ReportDate = dataFromReq.ReportDate;
                dataFromDB.Diagnosis = dataFromReq.Diagnosis;
                dataFromDB.Prescription = dataFromReq.Prescription;
                dataFromDB.Notes = dataFromReq.Notes;
                dataFromDB.patientId = dataFromReq.PatientId;
                dataFromDB.DoctorId = doctor.Id;
                medicalReportRepo.Update(dataFromDB);
                medicalReportRepo.Save();
                return RedirectToAction("GetAll");
            }
            dataFromReq.PatientList = patientRepo.GetAll();
            return View("Edit", dataFromReq);
        }

        public IActionResult Details(int id) 
        {
            MedicalReport medicalReport= medicalReportRepo.GetById(id);
            if (medicalReport == null)
            {
                return View("Error");
            }
            return View("Details", medicalReport);
        }
        [Authorize(Roles ="Doctore,Admin,staff")]
        public ActionResult Delete(int id)
        {
            try
            {
                medicalReportRepo.Delete(id);
                medicalReportRepo.Save();
                return Json(new { success = true, message = "Doctor deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Failed to delete doctor." });
            }
        }
    }
}
