using HospitalManagement.Models;

namespace HospitalManagement.Repositoty
{
    public interface IAppointmentRepository
    {
        public void Add(Appointment appointment);
        public void Update(Appointment appointment);
        public void Delete(int id);
        public List<Appointment> GetAll();
        public int GetAppointmentCountByDoctorId(int doctorId, string searchTerm);

        public int GetAppointmentCountByPatientId(int patientId, string searchTerm);

        public int GetAllAppointmentCount(string searchTerm);
        public Appointment GetById(int id);
        public List<Appointment> GetAllPagination(int page, int pageSize, string searchTerm);

        public List<Appointment> GetAppointmentByDoctorIdPagination(int _doctorId, int page, int pageSize, string searchTerm);
        public List<Appointment> GetAppointmentByPatientIdPagination(int _patientId, int page, int pageSize, string searchTerm);
        public List<Appointment> GetByPatientId(int patientId);
        public void Save();

    }
}
