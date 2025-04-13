using HospitalManagement.Models;

namespace HospitalManagement.Repositoty
{
    public class BillingRepository: IBillingRepository
    {
        APPContext context;
        public BillingRepository(APPContext _context)
        {
            context = _context;
        }

        public void Add(Billing billing)
        {
            context.Add(billing);
        }

        public void Delete(int id)
        {
            Billing billing = GetById(id);
            context.Remove(billing);
        }

        public List<Billing> GetAll()
        {
            return context.Billings.ToList();
        }

        public Billing GetById(int id)
        {
            return context.Billings.FirstOrDefault(b => b.Id == id);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Billing billing)
        {
            context.Update(billing);
        }
    }
}
