using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace HospitalManagementSystem.Services.AccountServices
{
    public class AccountServices:IAccountServices
    {
        private readonly HMSDbContext _hmsDBContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        
        public AccountServices(HMSDbContext hmSDbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _hmsDBContext = hmSDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public List<DepartmentModel> GetDepartment(string doctorId)
        {
            List<DepartmentModel> departmentList = (from d in _hmsDBContext.Department
                                     join u in _userManager.Users
                                     on d.Id equals u.DepartmentId
                                     where u.Id == doctorId
                                     select new DepartmentModel
                                     {
                                        DepartmentName = d.DepartmentName,
                                        Id = d.Id
                                     }).ToList();
            return departmentList;
        }

        public List<AppointmentModel> GetAppointmentDetails(string doctorId)
        {
            List<AppointmentModel> appointmentList = (from a in _hmsDBContext.Appointment
                                                      join u in _userManager.Users
                                                      on a.DoctorId equals u.Id
                                                      where (u.Id == doctorId) 
                                                      select new AppointmentModel
                                                      {
                                                          Id = a.Id,
                                                          UserId = a.UserId,
                                                          Condition = a.Condition,
                                                          Description = a.Description,
                                                          Schedule = a.Schedule,  
                                                      }
                                                      ).ToList();
            return appointmentList;
        }
        public List<AppointmentModel> GetAppointmentForUser(string userId)
        {
            List<AppointmentModel> appointmentList = (from a in _hmsDBContext.Appointment
                                                      join u in _userManager.Users
                                                      on a.UserId equals u.Id
                                                      where (u.Id == userId)
                                                      select new AppointmentModel
                                                      {
                                                          Id = a.Id,
                                                          Condition = a.Condition,
                                                          Description = a.Description,
                                                          DoctorId = a.DoctorId,
                                                          Schedule = a.Schedule,
                                                      }
                                          ).ToList();
            return appointmentList;
        }

        public int GetRoleID(ApplicationUser user)
        {
            int roles = _userManager.GetRolesAsync(user).Id;
            return roles;
        }
    }
}
