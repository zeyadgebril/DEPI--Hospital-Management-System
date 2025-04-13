using HospitalManagement.Models;

namespace HospitalManagement.Repositoty
{
    public interface INurseRepository
    {
        public void Add(Nurse nurse);
        public void Update(Nurse nurse);
        public void Delete(int id);
        public List<Nurse> GetAll();
        public Nurse GetById(int id);
        public void Save();
        public List<Nurse> GetAllPagination(int page, int pageSize);
        public int count();
    }
}
