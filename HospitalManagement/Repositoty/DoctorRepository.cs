using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Linq;

namespace HospitalManagement.Repositoty
{
    public class DoctorRepository:IDoctorRepository
    {
        APPContext context;
        public DoctorRepository(APPContext _context)
        {
            context = _context;
        }

        public void Add(Doctor doctor)
        {
            context.Add(doctor);
        }

        public void Delete(int id)
        {
            Doctor doctor = GetById(id);
            context.Remove(doctor);
        }

        public List<Doctor> GetAll()
        {
            return context.Doctors.ToList();
        }

        public List<Doctor> GetAllPagination(int page , int pageSize)
        {
            return context.Doctors
                .Include(d => d.Department)
                .Include(d => d.ApplicationUser)
                .OrderBy(d => d.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();   
        }

        public Doctor GetById(int id)
        {
            return context.Doctors
                .Include(d=>d.Department)
                .Include(d => d.ApplicationUser)
                .FirstOrDefault(d => d.Id == id);
        } 
        public Doctor GetById_Includedept(int id)
        {
            return context.Doctors.Include(d => d.Department).FirstOrDefault(d => d.Id == id);
        }
        public List<Doctor> GetByDeptId(int deptId)
        {
            return context.Doctors.Where(e => e.DepartmentId == deptId).ToList();
        }
        public Doctor GetDoctorByAppUserId(string userId)
        {
            return context.Doctors.FirstOrDefault(d => d.ApplicationUserId == userId);
        }
        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Doctor doctor)
        {
            context.Update(doctor);
        }
        public int count()
        {
            return context.Doctors.Count();
        }

    }
}
