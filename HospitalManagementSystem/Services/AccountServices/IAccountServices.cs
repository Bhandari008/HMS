using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace HospitalManagementSystem.Services.AccountServices
{
    public interface IAccountServices
    {
        public List<DepartmentModel> GetDepartment(string doctorId);


        public List<AppointmentModel> GetAppointmentDetails(string doctorId);

        public List<AppointmentModel> GetAppointmentForUser(string userId);

        public int GetRoleID(ApplicationUser user);

    }
}
