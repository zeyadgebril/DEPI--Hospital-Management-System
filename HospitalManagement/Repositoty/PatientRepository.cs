using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositoty
{
    public class PatientRepository : IPatientRepository
    {
        public APPContext context { get; }
        public PatientRepository(APPContext _context)
        {
            context = _context;
        }

        public void Add(Patient patient)
        {
            context.Patients.Add(patient);
        }

        public void Delete(int id)
        {
            Patient patient = GetById(id);
            context.Patients.Remove(patient);
        }

        public List<Patient> GetAll()
        {
            return context.Patients.ToList();
        }

        public Patient GetById(int id)
        {
            return context.Patients.FirstOrDefault(p => p.Id == id);
        }
        public Patient GetByUserId(string userId)
        {
            return context.Patients.FirstOrDefault(p => p.ApplicationUserId == userId);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Patient patient)
        {
            context.Update(patient);
        }

        public List<Patient> GetAllPagination(int page, int pageSize, string searchTerm)
        {
            return context.Patients
                .Where(p => p.Name.Contains(searchTerm))
               .OrderBy(p=> p.Id)
               .Skip((page - 1) * pageSize)
               .Take(pageSize).ToList();
        }

        public int count(string searchTerm)
        {
            return context.Patients
                .Where(p => string.IsNullOrEmpty(searchTerm) || p.Name.Contains(searchTerm))
                .Count();
        }
    }
}
