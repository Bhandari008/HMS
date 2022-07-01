using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services.AppointmentServices;
using HospitalManagementSystem.Services.DepartmentServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HospitalManagementSystem.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentServices _appointmentServices;
        private readonly IDepartmentServices _departmentServices;
        private readonly UserManager<ApplicationUser> _userManager;
        public AppointmentController(IAppointmentServices appointmentServices, IDepartmentServices departmentServices, UserManager<ApplicationUser> userManager)
        {
            _appointmentServices = appointmentServices;
            _departmentServices = departmentServices;
            _userManager = userManager;
        }
        [Authorize(Roles="Admin")]
        public IActionResult Index()
        {
            List<AppointmentViewModel> Appointment = _appointmentServices.Display();
            return View(Appointment);
        }
        [Authorize(Roles = "Patient")]
        [HttpGet]
        public IActionResult Create()
        {
            List<SelectListItem> departments = _departmentServices.Display().Select(x=>new SelectListItem { Text=x.DepartmentName,Value=x.Id.ToString()}).ToList();
            ViewBag.Departments = departments;
            return View();
        }
        [Authorize(Roles = "Patient")]
        [HttpPost]
        public async Task<IActionResult> Create(AppointmentModel model)
        {
                ApplicationUser CurrentUser = await _userManager.GetUserAsync(HttpContext.User);
                string uid = CurrentUser.Id;
                model.UserId = uid;
                _appointmentServices.Add(model);
                return RedirectToAction("Create");

           
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult AssignAppointment(int did,int aid)
        {
            // TODO:
            // Show Appointment Lists with Related Department
            // Show Doctors according to the Department
            // Assign AppointmentId to a particular doctor

            AppointmentModel appointmentModel = _appointmentServices.GetAppointment(aid);
            List<SelectListItem> doctorsList = _appointmentServices.GetDoctors(did).Select(x => new SelectListItem { Text = x.Name, Value = x.Id }).ToList();
            ViewBag.Doctors = doctorsList;

            return View(appointmentModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AssignAppointment(AppointmentModel model)
        {
            _appointmentServices.Update(model);
            return RedirectToAction("Index");   
        }
    }
}
