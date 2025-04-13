using HospitalManagement.Models;

namespace HospitalManagement.Repositoty
{
    public interface IDoctorRepository
    {
        public void Add(Doctor doctor);
        public void Update(Doctor doctor);
        public void Delete(int id);
        public List<Doctor> GetAll();
        public List<Doctor> GetAllPagination(int page, int pageSize);
        public int count();
        public Doctor GetById(int id);
        public Doctor GetById_Includedept(int id);
        public List<Doctor> GetByDeptId(int deptId);
        public Doctor GetDoctorByAppUserId(string userId);
        public void Save();
    }
}
