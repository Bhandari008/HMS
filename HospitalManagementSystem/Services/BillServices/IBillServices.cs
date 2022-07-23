using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Services.BillServices
{
    public interface IBillServices
    {
        public void AddBill(BillModel model);

        public void UpdateBill(BillModel model);

        public List<BillModel> DisplayBillDetails(int bid);

        public List<BillViewModel> Display();

        public List<BillViewModel> GetBillForUser(string uid);

        public void DeleteBill(int id);
    }
}
