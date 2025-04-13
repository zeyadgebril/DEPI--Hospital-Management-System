using HospitalManagement.Models;

namespace HospitalManagement.Repositoty
{
    public class StaffRepository : IStaffRepository
    {
        public APPContext context { get; }
        public StaffRepository(APPContext _context) {
            context = _context;
        }

        public void Add(Staff staff)
        {
            context.Add(staff);
        }

        public void Delete(int id)
        {
            Staff staff = GetById(id);
            context.Staffs.Remove(staff);
        }
        public List<Staff> GetAll()
        {
            return context.Staffs.ToList();
        }

        public Staff GetById(int id)
        {
            return context.Staffs.FirstOrDefault(s => s.Id == id);
        }
        public Staff GetStaffByAppUserId(string userId)
        {
            return context.Staffs.FirstOrDefault(d => d.ApplicationUserId == userId);
        }
        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Staff staff)
        {
            context.Update(staff);
        }

        public List<Staff> GetAllPagination(int page, int pageSize, string searchTerm)
        {
            return context.Staffs
                .Where(p => p.Name.Contains(searchTerm))
               .OrderBy(p => p.Id)
               .Skip((page - 1) * pageSize)
               .Take(pageSize).ToList();
        }

        public int count(string searchTerm)
        {
            return context.Staffs
                .Where(p => string.IsNullOrEmpty(searchTerm) || p.Name.Contains(searchTerm))
                .Count();
        }
    }
}
