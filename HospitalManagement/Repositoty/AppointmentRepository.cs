using HospitalManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Repositoty
{
    public class AppointmentRepository: IAppointmentRepository
    {
        APPContext context;
        public AppointmentRepository(APPContext _context)
        {
            context = _context;
        }

        public void Add(Appointment appointment)
        {
            context.Add(appointment);
        }

        public int GetAppointmentCountByDoctorId(int doctorId, string searchTerm)
        {
            return context.Appointments
                .Where(r => r.DoctorId == doctorId && r.Patient.Name.Contains(searchTerm))
                .Count();
        }

        public int GetAppointmentCountByPatientId(int patientId, string searchTerm)
        {
            return context.Appointments
                .Where(r => r.PatientId == patientId && r.Patient.Name.Contains(searchTerm))
                .Count();
        }

        public int GetAllAppointmentCount(string searchTerm)
        {
            return context.Appointments
                .Where(r => r.Patient.Name.Contains(searchTerm))
                .Count();
        }
        public void Delete(int id)
        {
            Appointment appointment = GetById(id);
            context.Remove(appointment);
        }

        public List<Appointment> GetAll()
        {
            return context.Appointments.ToList();
        }

        public List<Appointment> GetAppointmentByDoctorIdPagination(int _doctorId, int page, int pageSize, string searchTerm)
        {
            return context.Appointments
              .Where(r => r.DoctorId == _doctorId && r.Patient.Name.Contains(searchTerm)).Include(a => a.Doctor)
              .Include(a => a.Doctor)  // تحميل البيانات الخاصة بالطبيب
             .Include(p => p.Patient) // تحميل البيانات الخاصة بالمريض
             .Include(a => a.Doctor.Department) // تحميل قسم الطبيب
             .OrderBy(r => r.AppointmentDate)  // ترتيب المواعيد حسب تاريخ الموعد
             .Skip((page - 1) * pageSize)  // تطبيق التصفح (pagination)
             .Take(pageSize)  // تحديد عدد النتائج المعروضة
             .ToList();
        }
        public List<Appointment> GetAllPagination(int page, int pageSize, string searchTerm)
        {
                return context.Appointments
             .Where(r => r.Patient.Name.Contains(searchTerm))  // البحث باستخدام اسم المريض
             .Include(a => a.Doctor)  // تحميل البيانات الخاصة بالطبيب
             .Include(p => p.Patient) // تحميل البيانات الخاصة بالمريض
             .Include(a => a.Doctor.Department) // تحميل قسم الطبيب
             .OrderBy(r => r.AppointmentDate)  // ترتيب المواعيد حسب تاريخ الموعد
             .Skip((page - 1) * pageSize)  // تطبيق التصفح (pagination)
             .Take(pageSize)  // تحديد عدد النتائج المعروضة
             .ToList();
        }
        public List<Appointment> GetAppointmentByPatientIdPagination(int _patientId, int page, int pageSize, string searchTerm)
        {
            return context.Appointments
                .Where(r => r.PatientId == _patientId && r.Patient.Name.Contains(searchTerm)).Include(a => a.Doctor)
                .Include(a => a.Doctor)  // تحميل البيانات الخاصة بالطبيب
             .Include(p => p.Patient) // تحميل البيانات الخاصة بالمريض
             .Include(a => a.Doctor.Department) // تحميل قسم الطبيب
             .OrderBy(r => r.AppointmentDate)  // ترتيب المواعيد حسب تاريخ الموعد
             .Skip((page - 1) * pageSize)  // تطبيق التصفح (pagination)
             .Take(pageSize)  // تحديد عدد النتائج المعروضة
             .ToList();
        }
        public List<Appointment> GetByPatientId(int patientId)
        {
            return context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Doctor.Department)
                .Where(a => a.PatientId == patientId)
                .ToList();
        }
        public Appointment GetById(int id)
        {
            return context.Appointments
                .Include(p=>p.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.Doctor.Department).
                FirstOrDefault(a => a.Id == id);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Appointment appointment)
        {
            context.Update(appointment);
        }
        
    }
}
