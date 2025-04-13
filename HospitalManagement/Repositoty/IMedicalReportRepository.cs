using HospitalManagement.Models;

namespace HospitalManagement.Repositoty
{
    public interface IMedicalReportRepository
    {
        public void Add(MedicalReport medicalReport);
        public void Update(MedicalReport medicalReport);
        public void Delete(int id);
        public List<MedicalReport> GetAll();
        public MedicalReport GetById(int id);

        public List<MedicalReport> GetAllPagination(int page, int pageSize,string searchTerm);
        public List<MedicalReport> GetReportsByDoctorIdPagination(int doctorId, int page, int pageSize, string searchTerm);
        public List<MedicalReport> GetReportsByPatientIdPagination(int _patientId, int page, int pageSize, string searchTerm);
        public int GetReportsCountByDoctorId(int doctorId, string searchTerm);

        public int GetReportsCountByPatientId(int patientId, string searchTerm);
        public int GetAllReportsCount(string searchTerm);
        public void Save();
    }
}
