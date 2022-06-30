using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Services.AppointmentServices
{
    public interface IAppointmentServices
    {
        public List<AppointmentViewModel> Display();
        public void Add(AppointmentModel model);

        public List<DoctorModel> GetDoctors(int id);

        public void Update(AppointmentModel model);

        public AppointmentModel GetAppointment(int id);
       
       
    }
}
