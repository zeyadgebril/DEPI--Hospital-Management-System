using HospitalManagement.Models;
using HospitalManagement.Repositoty;
using HospitalManagement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    public class PatientController : Controller
    {
        IPatientRepository patientRepo;
        UserManager<ApplicationUser> userManager;
        public PatientController(IPatientRepository _patientRepo,
            UserManager<ApplicationUser> _userManager)
        {
            patientRepo = _patientRepo;
            userManager = _userManager;
        }
        [Authorize(Roles = "Staff,Admin")]
        public IActionResult GetAll(int pageNumber, int pageSize = 5, string searchTerm = "")
        {
            int totalRecord = patientRepo.count(searchTerm);
            int totalPage = (int)Math.Ceiling((double)totalRecord / pageSize);

            if (pageNumber < 1) pageNumber = 1;

            if (pageNumber > totalPage && totalPage > 0) pageNumber = totalPage;
            List<Patient> patient = patientRepo.GetAllPagination(pageNumber, pageSize,searchTerm);
            //Mapping
            var viewModel = new PaginationViewModel<Patient>();
            viewModel.Items = patient;
            viewModel.CurrentPage = pageNumber;
            viewModel.PageSize = pageSize;
            viewModel.TotalRecords = totalRecord;
            viewModel.TotalPages = totalPage;
            viewModel.SearchTerm = searchTerm;

            return View("GetAll", viewModel);
        }
        [HttpGet]
        [Authorize(Roles = "Patient")]
        public IActionResult Add()
        {
            return View("Add");
        }
        [HttpPost]
        [Authorize(Roles = "Patient")]
        public IActionResult SaveAdd(PAtientAddEditeVM dataFromReq)
        {
            
            if (ModelState.IsValid)
            {
                var userID = userManager.GetUserId(User);
                if (userID == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                Patient patient = new Patient();
                patient.Id = dataFromReq.Id;
                patient.Name = dataFromReq.Name;
                patient.DOB = dataFromReq.DOB;
                patient.Gender = dataFromReq.Gender;
                patient.Address = dataFromReq.Address;
                patient.ContactInfo = dataFromReq.ContactInfo;
                patient.Image = dataFromReq.Image;
                patient.ApplicationUserId = userID;
                patientRepo.Add(patient);
                patientRepo.Save();
                return RedirectToAction("Index", "Home");
            }
            return View("Add", dataFromReq);
        }
        [Authorize(Roles = "Patient")]
        public ActionResult Edit(int id)
        {
            Patient patient = patientRepo.GetById(id);

            PAtientAddEditeVM patientVM =new PAtientAddEditeVM();
            patientVM.Id = id;
            patientVM.Name = patient.Name;
            patientVM.DOB = patient.DOB;
            patientVM.Gender = patient.Gender;
            patientVM.Address = patient.Address;
            patientVM.ContactInfo = patient.ContactInfo;
            patientVM.Image = patient.Image;
            return View("Edit",patientVM);
        }
        [HttpPost]
        public IActionResult SaveEdit(PAtientAddEditeVM dataFromReq,int id)
        {
            if(ModelState.IsValid)
            {
                Patient patient = patientRepo.GetById(id);
                patient.Name = dataFromReq.Name;
                patient.DOB = dataFromReq.DOB;
                patient.Gender = dataFromReq.Gender;
                patient.Address = dataFromReq.Address;
                patient.ContactInfo = dataFromReq.ContactInfo;
                patient.Image = dataFromReq.Image;
                patientRepo.Update(patient);
                patientRepo.Save();
                return RedirectToAction("GetALL");
            }
            return View("Edit", dataFromReq);
        }
        public IActionResult Details(int id)
        {
            Patient patient = patientRepo.GetById(id);
            if (patient == null)
            {
                return View("Error");
            }
            return View("Details", patient);
        }
        [Authorize(Roles = "Staff,Admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                patientRepo.Delete(id);
                patientRepo.Save();
                return Json(new { success = true, message = "Patient deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Failed to delete PAtient." });
            }
        }
    }
}
