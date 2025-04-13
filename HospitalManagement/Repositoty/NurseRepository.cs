using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositoty
{
    public class NurseRepository:INurseRepository
    {
        APPContext context;
        public NurseRepository(APPContext _context) 
        { 
            context = _context;
        }

        public void Add(Nurse nurse)
        {
            context.Add(nurse);
        }

        public void Delete(int id)
        {
            Nurse nurse = GetById(id);
            context.Remove(nurse);
        }

        public List<Nurse> GetAll()
        {
            return context.Nurses.ToList();
        }
        public List<Nurse> GetAllPagination(int page, int pageSize)
        {
            return context.Nurses
                .Include(d => d.Department)
                .OrderBy(d => d.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();
        }
        public int count()
        {
            return context.Nurses.Count();
        }
        public Nurse GetById(int id)
        {
            return context.Nurses.FirstOrDefault(n=>n.Id==id);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Nurse nurse)
        {
            context.Update(nurse);
        }
    }
}
