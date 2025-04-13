using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositoty
{
    public class MedicalReportRepository:IMedicalReportRepository
    {
        APPContext context;
        public MedicalReportRepository(APPContext _context)
        {
            context = _context;
        }

        public void Add(MedicalReport medicalReport)
        {
            context.Add(medicalReport);
        }

        public void Delete(int id)
        {
            MedicalReport medicalReport = GetById(id);
            context.Remove(medicalReport);
        }

        public List<MedicalReport> GetAll()
        {
            return context.MedicalReports.ToList();
        }

        public MedicalReport GetById(int id)
        {
            return context.MedicalReports      
                .Include(d => d.Patient)
                .Include(d => d.Doctor)
                .ThenInclude(doc => doc.Department)
                .FirstOrDefault(d => d.Id == id);
        }
        public void Update(MedicalReport medicalReport)
        {
            context.Update(medicalReport);
        }
        public void Save()
        {
            context.SaveChanges();
        }
        public List<MedicalReport> GetAllPagination(int page, int pageSize,string searchTerm)
        {
            return context.MedicalReports
                .Where(p => p.Patient.Name.Contains(searchTerm))
                .Include(d => d.Patient)
                .Include(d => d.Doctor)
                .ThenInclude(d=>d.Department)
                .OrderBy(d => d.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();
        }
        public List<MedicalReport> GetReportsByDoctorIdPagination(int doctorId, int page, int pageSize, string searchTerm)
        {
         return context.MedicalReports
         .Where(r => r.DoctorId == doctorId && r.Patient.Name.Contains(searchTerm)) // أضفنا التصفية هنا قبل الـ Include
         .Include(r => r.Doctor)
         .ThenInclude(d => d.Department)
         .Include(r => r.Patient)
         .OrderBy(r => r.Id) // يمكنك ترتيب البيانات حسب الحاجة (مثلاً حسب ID التقرير)
         .Skip((page - 1) * pageSize) // التخطي حسب الـ pagination
         .Take(pageSize) // تحديد عدد السجلات المعروضة في الصفحة
         .ToList();
        }

        public List<MedicalReport> GetReportsByPatientIdPagination(int _patientId, int page, int pageSize, string searchTerm)
        {
            return context.MedicalReports
            .Where(r => r.patientId == _patientId && r.Patient.Name.Contains(searchTerm)) // أضفنا التصفية هنا قبل الـ Include
            .Include(r => r.Doctor)
            .ThenInclude(d => d.Department)
            .Include(r => r.Patient)
            .OrderBy(r => r.Id) // يمكنك ترتيب البيانات حسب الحاجة (مثلاً حسب ID التقرير)
            .Skip((page - 1) * pageSize) // التخطي حسب الـ pagination
            .Take(pageSize) // تحديد عدد السجلات المعروضة في الصفحة
            .ToList();
        }
        public int GetReportsCountByDoctorId(int doctorId, string searchTerm)
        {
            return context.MedicalReports
                .Where(r => r.DoctorId == doctorId && r.Patient.Name.Contains(searchTerm))
                .Count();
        }

        public int GetReportsCountByPatientId(int patientId, string searchTerm)
        {
            return context.MedicalReports
                .Where(r => r.patientId == patientId && r.Patient.Name.Contains(searchTerm))
                .Count();
        }

        public int GetAllReportsCount(string searchTerm)
        {
            return context.MedicalReports
                .Where(r => r.Patient.Name.Contains(searchTerm))
                .Count();
        }

    }
}
