using HospitalManagement.Models;

namespace HospitalManagement.Repositoty
{
    public interface IBillingRepository
    {
        public void Add(Billing billing);
        public void Update(Billing billing);
        public void Delete(int id);
        public List<Billing> GetAll();
        public Billing GetById(int id);
        public void Save();
    }
}
