using HospitalManagement.Models;
using HospitalManagement.Repositoty;
using HospitalManagement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    [Authorize(Roles = "Staff,Admin")]
    public class RoomController : Controller
    {
        IRoomRepository roomRepo;
        IDepartmentRepository deptRepo;
        public RoomController(IRoomRepository _roomRepo, IDepartmentRepository _deptRepo)
        {
            roomRepo = _roomRepo;
            deptRepo = _deptRepo;
        }
        public IActionResult GetAll(int pageNumber, int pageSize = 5)
        {
            int totalRecord = roomRepo.Count();
            int totalPage = (int)Math.Ceiling((double)totalRecord / pageSize);

            if (pageNumber < 1) pageNumber = 1;

            if (pageNumber > totalPage && totalPage > 0) pageNumber = totalPage;
            List<Room> appointments = roomRepo.GetAllPagination(pageNumber, pageSize);
            //Mapping
            var viewModel = new PaginationViewModel<Room>();
            viewModel.Items = appointments;
            viewModel.CurrentPage = pageNumber;
            viewModel.PageSize = pageSize;
            viewModel.TotalRecords = totalRecord;
            viewModel.TotalPages = totalPage;

            return View("GetAll", viewModel);
        }
        public IActionResult Add()
        {
            RoomAddEditVM dataModel = new RoomAddEditVM();
            dataModel.deptList = deptRepo.GetAll();
            return View("Add", dataModel);
        }
        [HttpPost]
        public ActionResult SaveAdd(RoomAddEditVM dataFromReq)
        {
            if (ModelState.IsValid)
            {
                if(dataFromReq.DepartmentId != 0)
                {
                    Room room = new Room();
                    room.RoomName = dataFromReq.RoomName;
                    room.DepartmentId = dataFromReq.DepartmentId;
                    roomRepo.Add(room);
                    roomRepo.Save();
                    return RedirectToAction("GetAll");
                }
                else
                {
                    ModelState.AddModelError("DepartmentId", "Select Department");
                }
            }
            dataFromReq.deptList =deptRepo.GetAll();
            return View("Add", dataFromReq);
        }
        [HttpGet]
        public ActionResult Edite(int id)
        {
            Room room = roomRepo.GetById(id);
            List<Department> DeptList = deptRepo.GetAll();

            RoomAddEditVM dataModel = new RoomAddEditVM();
            dataModel.Id = room.Id;
            dataModel.RoomName = room.RoomName;
            dataModel.DepartmentId = room.DepartmentId;
            dataModel.deptList = DeptList;

            return View("Edit",dataModel);
        }
        [HttpPost]
        public IActionResult SaveEdit(RoomAddEditVM dataFromReq, int id)
        {
            if (ModelState.IsValid)
            {
                if (dataFromReq.DepartmentId != 0)
                {
                    Room room = new Room();
                    room.Id = dataFromReq.Id;
                    room.RoomName = dataFromReq.RoomName;
                    room.DepartmentId = dataFromReq.DepartmentId;
                    roomRepo.Update(room);
                    roomRepo.Save();
                    return RedirectToAction("GetAll");
                }
                else
                {
                    ModelState.AddModelError("DepartmentId", "Select Department");
                }
            }
            dataFromReq.deptList = deptRepo.GetAll();
            return View("Edit", dataFromReq);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                roomRepo.Delete(id);
                roomRepo.Save();
                return Json(new { success = true, message = "Doctor deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Failed to delete doctor." });
            }
        }
    }
}
