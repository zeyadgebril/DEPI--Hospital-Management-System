using HospitalManagement.Models;
using HospitalManagement.Repositoty;
using HospitalManagement.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    public class StaffController : Controller
    {
        IStaffRepository staffRepo;
        UserManager<ApplicationUser> userManager;
        public StaffController(IStaffRepository _staffRepo,UserManager<ApplicationUser> _userManger)
        {
            staffRepo = _staffRepo;
            userManager = _userManger;
        }

        public IActionResult GetAll(int pageNumber, int pageSize = 5, string searchTerm = "")
        {
            int totalRecord = staffRepo.count(searchTerm);
            int totalPage = (int)Math.Ceiling((double)totalRecord / pageSize);

            if (pageNumber < 1) pageNumber = 1;

            if (pageNumber > totalPage && totalPage > 0) pageNumber = totalPage;
            List<Staff> staff = staffRepo.GetAllPagination(pageNumber, pageSize,searchTerm);
            //Mapping
            var viewModel = new PaginationViewModel<Staff>();
            viewModel.Items = staff;
            viewModel.CurrentPage = pageNumber;
            viewModel.PageSize = pageSize;
            viewModel.TotalRecords = totalRecord;
            viewModel.TotalPages = totalPage;

            return View("GetAll", viewModel);
        }


        [HttpGet]
        public IActionResult Registerstaff()
        {
            return View("Registerstaff");
        }
        [HttpPost]
        public async Task<IActionResult> Registerstaff(RegisterStaffViewModel dataDromReq) 
        {
            if (ModelState.IsValid)
            {
                var user =new ApplicationUser();
                user.UserName = dataDromReq.UserName;
                user.Email = dataDromReq.Email;

                IdentityResult result = await userManager.CreateAsync(user,dataDromReq.Password);
                if (result.Succeeded) 
                {
                    await userManager.AddToRoleAsync(user, dataDromReq.Role);

                    Staff staff = new Staff();
                    staff.Name = dataDromReq.Name;
                    staff.Email = dataDromReq.Email;
                    staff.ContactInfo = dataDromReq.ContactInfo;
                    staff.address = dataDromReq.Address;
                    staff.ApplicationUserId = user.Id;
                    staff.Image = dataDromReq.Image;
                    staff.Role = dataDromReq.Role;
                    staffRepo.Add(staff);
                    staffRepo.Save();
                    return RedirectToAction("GetAll");
                }
                foreach (var error in result.Errors) 
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View("Registerstaff", dataDromReq);
        }
        public IActionResult Details(int id)
        {
            Staff staff = staffRepo.GetById(id);
            if (staff == null)
            {
                return View("Error");
            }
            return View("Details", staff);
        }
    }
}
