using HospitalManagement.Models;

namespace HospitalManagement.Repositoty
{
    public class DepartmentRepository:IDepartmentRepository
    {
        APPContext context;
        public DepartmentRepository(APPContext _context)
        {
            context = _context;
        }

        public void Add(Department department)
        {
            context.Add(department);
        }

        public void Delete(int id)
        {
            Department department = GetById(id);
            context.Remove(department);
        }

        public List<Department> GetAll()
        {
            return context.Departments.ToList();
        }

        public Department GetById(int id)
        {
            return context.Departments.FirstOrDefault(d => d.Id == id);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Department department)
        {
            context.Update(department);
        }
    }
}
