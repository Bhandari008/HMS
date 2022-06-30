using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace HospitalManagementSystem.Services.AppointmentServices
{
    public class AppointmentServices:IAppointmentServices
    {
        private readonly HMSDbContext _hmsDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public AppointmentServices(HMSDbContext hSMDbContext, UserManager<ApplicationUser> userManager)
        {
            _hmsDbContext = hSMDbContext;
            _userManager = userManager;
        }

    
    public List<AppointmentViewModel> Display()
        {

            List<AppointmentViewModel> appointment = (from a in _hmsDbContext.Appointment
                                                      join d in _hmsDbContext.Department on
                                                      a.DepartmentId equals d.Id
                                                      where a.isActive == true
                                                      select new AppointmentViewModel
                                                      {
                                                          Id = a.Id,
                                                          Description = a.Description,
                                                          DepartmentName = d.DepartmentName,
                                                          Schedule = a.Schedule,
                                                          Condition = a.Condition,
                                                          DepartmentId = a.DepartmentId,
                                                          UserId = a.UserId,
                                                          DoctorId = a.DoctorId
                                                      }).ToList();

           
            return appointment;
        }

        public List<DoctorModel> GetDoctors(int id)
        {
            List<DoctorModel> doctors = (from u in _userManager.Users
                                         join d in _hmsDbContext.Department on
                                         u.DepartmentId equals d.Id
                                         where u.DepartmentId == id
                                         select new DoctorModel
                                         {
                                             Id = u.Id,
                                             Name = u.FullName,
                                             DepartmentId = u.DepartmentId
                                         }).ToList();
            return doctors;
        }
        public void Add(AppointmentModel model)
        {
            _hmsDbContext.Appointment.Add(model);
            _hmsDbContext.SaveChanges();
        }
        
        public void Update(AppointmentModel model)
        {
            _hmsDbContext.Appointment.Update(model);
            _hmsDbContext.SaveChanges();
        }

        public AppointmentModel GetAppointment(int id)
        {

            AppointmentModel model = _hmsDbContext.Appointment.FirstOrDefault(x => x.Id == id);
            if (model != null)
            {
                return model;
            }
            return (new AppointmentModel());
        }
    }
}
