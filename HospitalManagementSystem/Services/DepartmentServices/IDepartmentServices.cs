using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Services.DepartmentServices
{
    public interface IDepartmentServices
    {
        public List<DepartmentModel> Display();
        public void Add(DepartmentModel model);

        public void Delete(int id);
    }
}
