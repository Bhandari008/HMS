using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Services.AccountServices
{
    public interface IAccountServices
    {
        public List<DepartmentModel> GetDepartment(string doctorId);


        public List<AppointmentModel> GetAppointmentDetails(string doctorId);

    }
}
