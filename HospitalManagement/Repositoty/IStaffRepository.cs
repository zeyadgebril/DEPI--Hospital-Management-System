using HospitalManagement.Models;

namespace HospitalManagement.Repositoty
{
    public interface IStaffRepository
    {
        public void Add(Staff staff);
        public void Update(Staff staff);
        public void Delete(int id);
        public List<Staff> GetAll();
        public Staff GetById(int id);
        public Staff GetStaffByAppUserId(string userId);
        public void Save();

        public List<Staff> GetAllPagination(int page, int pageSize, string searchTerm);
        public int count(string searchTerm);

        
    }
}
