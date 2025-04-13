using HospitalManagement.Models;

namespace HospitalManagement.Repositoty
{
    public interface IDepartmentRepository
    {
        public void Add(Department department);
        public void Update(Department department);
        public void Delete(int id);
        public List<Department> GetAll();
        public Department GetById(int id);
        public void Save();
    }
}
