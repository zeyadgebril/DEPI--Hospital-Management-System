using HospitalManagement.Models;

namespace HospitalManagement.Repositoty
{
    public interface IRoomRepository
    {
        public void Add(Room room);
        public void Update(Room room);
        public void Delete(int id);
        public List<Room> GetAll();
        public Room GetById(int id);
        public void Save();
        public List<Room> GetAllPagination(int page, int pageSize);
        public int Count();
    }
}
