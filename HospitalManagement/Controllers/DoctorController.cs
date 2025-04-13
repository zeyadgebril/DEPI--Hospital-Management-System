using HospitalManagement.Models;
using HospitalManagement.Repositoty;
using HospitalManagement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace HospitalManagement.Controllers
{
    public class DoctorController : Controller
    {
        IDoctorRepository doctorRepo;
        IDepartmentRepository deptRepo;
        UserManager<ApplicationUser> userManager;
        public DoctorController(IDoctorRepository _doctorRepository,
                                 IDepartmentRepository _deptRepo,
                                 UserManager<ApplicationUser> _userManager
                                )
        {
            doctorRepo = _doctorRepository;
            deptRepo = _deptRepo;
            userManager = _userManager;
        }

        public IActionResult GetAll(int pageNumber, int pageSize = 5)
        {
            int totalRecord = doctorRepo.count();
            int totalPage = (int)Math.Ceiling((double)totalRecord / pageSize);

            if (pageNumber < 1) pageNumber = 1;

            if (pageNumber > totalPage && totalPage > 0) pageNumber = totalPage;
            List<Doctor> doctors = doctorRepo.GetAllPagination(pageNumber, pageSize);
            //Mapping
            var viewModel = new PaginationViewModel<Doctor>();
            viewModel.Items = doctors;
            viewModel.CurrentPage = pageNumber;
            viewModel.PageSize = pageSize;
            viewModel.TotalRecords = totalRecord;
            viewModel.TotalPages = totalPage;


            return View("GetAll", viewModel);
        }

        [Authorize(Roles ="Staff,Admin")]
        [HttpGet]
        public IActionResult Add()
        {
            DoctorWithDepartmentAddEditeVM dataModel = new DoctorWithDepartmentAddEditeVM();
            dataModel.deptList = deptRepo.GetAll();
            return View("Add", dataModel);
        }
        [HttpPost]
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> SaveAdd(DoctorWithDepartmentAddEditeVM dataFromReq)
        {
            if (ModelState.IsValid)
            {
                if (dataFromReq.DepartmentId != 0)
                {
                    Doctor doctor = new Doctor();
                    doctor.Name = dataFromReq.Name;
                    doctor.Specialty = dataFromReq.Specialty;
                    doctor.ContactInfo = dataFromReq.ContactInfo;
                    doctor.Image = dataFromReq.Image;
                    doctor.Email = dataFromReq.Email;
                    doctor.ExperienceYears = dataFromReq.ExperienceYears;
                    doctor.DepartmentId = dataFromReq.DepartmentId;
                    //to user
                    
                    
                    ApplicationUser appUser = new ApplicationUser();
                    appUser.UserName = dataFromReq.UserName;
                    appUser.Email = dataFromReq.Email;

                    IdentityResult resule = await userManager.CreateAsync(appUser, dataFromReq.Password);
                    if (resule.Succeeded)
                    {
                        doctor.ApplicationUserId = appUser.Id;
                        doctorRepo.Add(doctor);
                        doctorRepo.Save();
                        await userManager.AddToRoleAsync(appUser, "Doctor");
                    }

                    return RedirectToAction("GetAll");
                }
                else
                {
                    ModelState.AddModelError("DepartmentId", "Select Department");
                }
            }
            dataFromReq.deptList = deptRepo.GetAll();
            return View("Add", dataFromReq);
        }
        [HttpGet]
        [Authorize(Roles = "Staff,Admin")]
        public IActionResult Edite(int id)
        {
            Doctor doctor = doctorRepo.GetById(id);
            List<Department> deptList = deptRepo.GetAll();

            DoctorWithDepartmentAddEditeVM doctorVM = new DoctorWithDepartmentAddEditeVM();

            doctorVM.Id = doctor.Id;
            doctorVM.Name = doctor.Name;
            doctorVM.ContactInfo = doctor.ContactInfo;
            doctorVM.Specialty = doctor.Specialty;
            doctorVM.Image = doctor.Image;
            doctorVM.Email = doctor.Email;
            if (doctor.ApplicationUser != null)
            {
                doctorVM.UserName = doctor.ApplicationUser.UserName;
            }
            else
            {
                doctorVM.UserName = ""; // أو ممكن تخليه null لو الـ ViewModel بيدعمه
            }
            
            doctorVM.ExperienceYears = doctor.ExperienceYears;
            doctorVM.DepartmentId = doctor.DepartmentId;
            doctorVM.deptList = deptList;
            return View("Edite", doctorVM);
        }
        [HttpPost]
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> SaveEdit(DoctorWithDepartmentAddEditeVM dataFromReq, int id)
        {
            if (ModelState.IsValid)
            {
                if (dataFromReq.DepartmentId != 0)
                {
                    Doctor doctor = doctorRepo.GetById(id);
                    doctor.Name = dataFromReq.Name;
                    doctor.ContactInfo = dataFromReq.ContactInfo;
                    doctor.Specialty = dataFromReq.Specialty;
                    doctor.Image = dataFromReq.Image;
                    doctor.Email = dataFromReq.Email;
                    doctor.ExperienceYears = dataFromReq.ExperienceYears;
                    doctor.DepartmentId = dataFromReq.DepartmentId;
                    if (doctor.ApplicationUser == null)
                    {
                        ApplicationUser AppUSer = new ApplicationUser();
                        AppUSer.UserName = dataFromReq.UserName;
                        AppUSer.Email = dataFromReq.Email;

                        IdentityResult result = await userManager.CreateAsync(AppUSer, dataFromReq.Password);
                        if (!result.Succeeded)
                        {
                            // إذا فشل إنشاء المستخدم، أضف الأخطاء إلى ModelState
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                            dataFromReq.deptList = deptRepo.GetAll(); // إعادة تحميل الأقسام
                            return View("Edite", dataFromReq); // العودة إلى الصفحة مع الأخطاء
                        }

                        await userManager.AddToRoleAsync(AppUSer, "Doctor");

                        doctor.ApplicationUser = AppUSer;

                    }
                    else
                    {
                        doctor.ApplicationUser.UserName = dataFromReq.UserName;
                        doctor.ApplicationUser.Email = dataFromReq.Email;
                        if (!string.IsNullOrEmpty(dataFromReq.Password))
                        {
                            var token = await userManager.GeneratePasswordResetTokenAsync(doctor.ApplicationUser);
                            var result = await userManager.ResetPasswordAsync(doctor.ApplicationUser, token, dataFromReq.Password);
                            if (!result.Succeeded)
                            {
                                // إذا فشل تحديث كلمة المرور، أضف الأخطاء إلى ModelState
                                foreach (var error in result.Errors)
                                {
                                    ModelState.AddModelError("", error.Description);
                                }

                                dataFromReq.deptList = deptRepo.GetAll();
                                return View("Edite", dataFromReq);
                            }
                        }
                    }

                    doctorRepo.Update(doctor);
                    doctorRepo.Save();
                    return RedirectToAction("GetAll");
                }
                else
                {
                    ModelState.AddModelError("DepartmentId", "Select Department");
                }
            }   
                    dataFromReq.deptList = deptRepo.GetAll();
                    return View("Edite", dataFromReq);
            
        }
        public IActionResult Details(int id)
        {
            Doctor doctor = doctorRepo.GetById(id);
            if (doctor == null)
            {
                return View("Error");
            }
            return View("Details", doctor);
        }
        [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Doctor doctor = doctorRepo.GetById(id);
                if (doctor == null)
                {
                    return Json(new { success = false, message = "Doctor not found." });
                }

                if (doctor.ApplicationUser == null)
                {
                    return Json(new { success = false, message = "Doctor not found." });
                }
                if (doctor.ApplicationUser != null) 
                {
                    IdentityResult result = await userManager.DeleteAsync(doctor.ApplicationUser);
                    if (!result.Succeeded)
                    {
                        return Json(new { success = false, message = "Failed to delete associated user." });
                    }
                }
                doctorRepo.Delete(id);
                doctorRepo.Save();
                return Json(new { success = true, message = "Doctor deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Failed to delete doctor." });
            }
        }
    }
}
