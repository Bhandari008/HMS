using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Services.DepartmentServices
{
    public class DepartmentServices:IDepartmentServices
    {
        private readonly HMSDbContext _hmsDbContext;
        public DepartmentServices(HMSDbContext hmSDbContext)
        {
            _hmsDbContext = hmSDbContext;
        }

        public List<DepartmentModel> Display()
        {
            List<DepartmentModel> departments = _hmsDbContext.Department.ToList();
            return departments;
        }
        public void Add(DepartmentModel model)
        {
            _hmsDbContext.Department.Add(model);
            _hmsDbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            DepartmentModel model = _hmsDbContext.Department.FirstOrDefault(x => x.Id == id);
            _hmsDbContext.Department.Remove(model);
            _hmsDbContext.SaveChanges();

        }
    }
}
