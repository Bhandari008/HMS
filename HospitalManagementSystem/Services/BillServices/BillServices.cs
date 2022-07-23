using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace HospitalManagementSystem.Services.BillServices
{
    public class BillServices:IBillServices
    {
        private readonly HMSDbContext _hmsDBContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public BillServices(HMSDbContext hMSDbContext, UserManager<ApplicationUser> userManager)
        {
            _hmsDBContext = hMSDbContext;
            _userManager = userManager;
        }

        public List<BillViewModel> Display()
        {
            List<BillViewModel> bills = (from b in _hmsDBContext.Bill
                                         join a in _hmsDBContext.Appointment
                                         on b.AppointmentId equals a.Id
                                         select new BillViewModel
                                         {
                                             Id = b.Id,
                                             UserId = b.UserId,
                                              AppointmentId = b.AppointmentId,
                                             AppointmentDescription = a.Description,
                                             Amount = b.Amount,
                                             Condition = b.Condition
                                         }).ToList();
            return bills;
        }
        public void AddBill(BillModel model)
        {
            _hmsDBContext.Add(model);
            _hmsDBContext.SaveChanges();
        }

        public void UpdateBill(BillModel model)
        {
            _hmsDBContext.Update(model);
            _hmsDBContext.SaveChanges();
        }

        public List<BillModel> DisplayBillDetails(int bid)
        {
            List<BillModel> bills = _hmsDBContext.Bill.Where(x=>x.Id==bid).ToList();
            return bills;
        }

        public List<BillViewModel> GetBillForUser(string uid)
        {
            List<BillModel> billList = (from b in _hmsDBContext.Bill
                                        join u in _userManager.Users
                                        on b.UserId equals u.Id
                                        where b.UserId == uid
                                        select new BillModel
                                        {
                                            Id = b.Id,
                                            Amount = b.Amount,
                                            Condition = b.Condition, 
                                            AppointmentId = b.AppointmentId,
                                            
                                        }).ToList();

            List <BillViewModel> billDetails  = (from b in billList
                                       join a in _hmsDBContext.Appointment
                                       on b.AppointmentId equals a.Id
                                       select new BillViewModel
                                       {
                                            Id = b.Id,
                                            AppointmentDescription = a.Description,
                                            Amount = b.Amount,
                                            Condition = b.Condition
                                       }).ToList();
            return billDetails;
        }
        public void DeleteBill(int id)
        {
            BillModel model = _hmsDBContext.Bill.FirstOrDefault(x => x.Id == id);
            _hmsDBContext.Bill.Remove(model);
            _hmsDBContext.SaveChanges();
        }
    }
}
