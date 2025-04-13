using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositoty
{
    public class RoomRepository:IRoomRepository
    {
        public APPContext context { get; }
        public RoomRepository(APPContext _context)
        {
            context = _context;
        }

        public void Add(Room room)
        {
            context.Add(room);
        }

        public void Delete(int id)
        {
            Room room = GetById(id);
            context.Rooms.Remove(room);
        }
        public List<Room> GetAll()
        {
            return context.Rooms.ToList();
        }

        public Room GetById(int id)
        {
            return context.Rooms.FirstOrDefault(r => r.Id == id);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Room room)
        {
            context.Update(room);
        }
        public List<Room> GetAllPagination(int page, int pageSize)
        {
            return context.Rooms
                .Include(d => d.Department)
                .OrderBy(d => d.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();
        }
        public int Count()
        {
            return context.Rooms .Count();
        }
    }
}
