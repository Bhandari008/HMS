using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace HospitalManagementSystem.Services.AccountServices
{
    public class AccountServices:IAccountServices
    {
        private readonly HMSDbContext _hmsDBContext;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public AccountServices(HMSDbContext hmSDbContext, UserManager<ApplicationUser> userManager)
        {
            _hmsDBContext = hmSDbContext;
            _userManager = userManager;
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



    }
}
