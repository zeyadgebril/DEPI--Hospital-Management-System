using HospitalManagement.Models;
using HospitalManagement.Repositoty;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    public class DepartmentController : Controller
    {
        IDepartmentRepository deptRepo;
        public DepartmentController(IDepartmentRepository _deptRepo) 
        {
            deptRepo = _deptRepo;
        }
        [HttpGet]
        public IActionResult GetDepartment()
        {
            List<Department> deptList = deptRepo.GetAll();
            return View("GetDepartment",deptList);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View("Add");
        }
        [HttpPost]
        public IActionResult SaveAdd(Department dept)
        {
            if (ModelState.IsValid)
            {
                deptRepo.Add(dept);
                deptRepo.Save();
                return RedirectToAction("GetDepartment"); 
            }
            return View("Add",dept);
        }
        [HttpGet]
        public IActionResult Edite(int id)
        {
            Department department = deptRepo.GetById(id);
            return View("Edite",department);
        }
        public IActionResult SaveEdit(Department deptFromreq,int id)
        {
            if (ModelState.IsValid)
            {
                Department dept = deptRepo.GetById(id);
                dept.Name = deptFromreq.Name;
                deptRepo.Update(dept);
                deptRepo.Save();
                return RedirectToAction("GetDepartment");
            }
            return View("Edite", deptFromreq);
        }
       
        public IActionResult Delete(int id)
        {
            try
            {
                deptRepo.Delete(id);
                deptRepo.Save();
                return Json(new { success = true, message = "Doctor deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Failed to delete doctor." });
            }
        }
            
    }
}
