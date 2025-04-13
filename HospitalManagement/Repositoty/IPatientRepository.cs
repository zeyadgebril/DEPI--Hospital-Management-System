using HospitalManagement.Models;

namespace HospitalManagement.Repositoty
{
    public interface IPatientRepository
    {
        public void Add(Patient patient);
        public void Update(Patient patient);
        public void Delete(int id);
        public List<Patient> GetAll();
        public List<Patient> GetAllPagination(int page, int pageSize, string searchTerm);
        public int count(string searchTerm);
        public Patient GetById(int id);
        public Patient GetByUserId(string userId);
        public void Save();
    }
}
