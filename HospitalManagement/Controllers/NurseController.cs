using HospitalManagement.Models;
using HospitalManagement.Repositoty;
using HospitalManagement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    [Authorize(Roles = "Staff,Admin")]
    public class NurseController : Controller
    {
        INurseRepository nurseRepo;
        IDepartmentRepository deptRepo;
        public NurseController(INurseRepository _nurseRepo, IDepartmentRepository _deptRepo) 
        {
            nurseRepo = _nurseRepo;
            deptRepo = _deptRepo;
        }
        public IActionResult GetAll(int pageNumber, int pageSize = 5)
        {
            int totalRecord = nurseRepo.count();
            int totalPage = (int)Math.Ceiling((double)totalRecord / pageSize);

            if (pageNumber < 1) pageNumber = 1;

            if (pageNumber > totalPage && totalPage > 0) pageNumber = totalPage;
            List<Nurse> nurses = nurseRepo.GetAllPagination(pageNumber, pageSize);
            //Mapping
            var viewModel = new PaginationViewModel<Nurse>();
            viewModel.Items = nurses;
            viewModel.CurrentPage = pageNumber;
            viewModel.PageSize = pageSize;
            viewModel.TotalRecords = totalRecord;
            viewModel.TotalPages = totalPage;

            return View("GetAll", viewModel);
        }
        [HttpGet]
        public IActionResult Add()
        {
            Nurese_AddEditVM dataModel = new Nurese_AddEditVM();
            dataModel.DeptList = deptRepo.GetAll();
            return View("Add", dataModel);
        }
        [HttpPost]
        public IActionResult SaveAdd(Nurese_AddEditVM dataFromReq)
        {
            if (ModelState.IsValid)
            {
                if (dataFromReq.DepartmentId != 0)
                {
                    Nurse nurse = new Nurse();
                    nurse.Name = dataFromReq.Name;
                    nurse.Email = dataFromReq.Email;
                    nurse.ContactInfo = dataFromReq.ContactInfo;
                    nurse.Image = dataFromReq.Image;
                    nurse.ExperienceYears = dataFromReq.ExperienceYears;
                    nurse.DepartmentId = dataFromReq.DepartmentId;
                    
                    nurseRepo.Add(nurse);
                    nurseRepo.Save();
                    return RedirectToAction("GetAll");
                }
                else
                {
                    ModelState.AddModelError("DepartmentId", "Select Department");
                }
            }
            dataFromReq.DeptList = deptRepo.GetAll();
            return View("Add", dataFromReq);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Nurse nurse = nurseRepo.GetById(id);
            List<Department> deptList = deptRepo.GetAll();

            Nurese_AddEditVM nurseVM = new Nurese_AddEditVM();

           nurseVM.Id = nurse.Id;
           nurseVM.Name = nurse.Name;
           nurseVM.Email = nurse.Email;
           nurseVM.ContactInfo = nurse.ContactInfo;
           nurseVM.Image = nurse.Image;
           nurseVM.ExperienceYears = nurse.ExperienceYears;
           nurseVM.DepartmentId = nurse.DepartmentId;
           nurseVM.DeptList = deptList;
            return View("Edit", nurseVM);
        }
        [HttpPost]
        public IActionResult SaveEdit(Nurese_AddEditVM dataFromReq, int id)
        {
            if (ModelState.IsValid)
            {
                if (dataFromReq.DepartmentId != 0)
                {
                    Nurse nurse = nurseRepo.GetById(id);

                    nurse.Id = dataFromReq.Id;
                    nurse.Name = dataFromReq.Name;
                    nurse.Email = dataFromReq.Email;
                    nurse.ContactInfo = dataFromReq.ContactInfo;
                    nurse.Image = dataFromReq.Image;
                    nurse.ExperienceYears = dataFromReq.ExperienceYears;
                    nurse.DepartmentId = dataFromReq.DepartmentId;

                    nurseRepo.Update(nurse);
                    nurseRepo.Save();
                    return RedirectToAction("GetAll");
                }
                else
                {
                    ModelState.AddModelError("DepartmentId", "Select Department");
                }
            }
            dataFromReq.DeptList = deptRepo.GetAll();
            return View("Edit", dataFromReq);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                nurseRepo.Delete(id);
                nurseRepo.Save();
                return Json(new { success = true, message = "Doctor deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Failed to delete doctor." });
            }
        }
    }
}
