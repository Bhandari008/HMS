using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HospitalManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly HMSDbContext _hMSDbContext;

        public HomeController(ILogger<HomeController> logger,UserManager<ApplicationUser> userManager,HMSDbContext hMSDbContext)
        {
            _logger = logger;
            _userManager = userManager;
            _hMSDbContext = hMSDbContext;

        }
        
        public IActionResult Index()
        {
            List<ApplicationUser> users = _userManager.Users.ToList();
            List<AppointmentModel> appointements = _hMSDbContext.Appointment.ToList();
            List<DepartmentModel> departments = _hMSDbContext.Department.ToList();
            List<DoctorModel> doctors = (from u in _userManager.Users
                                         join d in _hMSDbContext.Department
                                         on u.DepartmentId equals d.Id
                                         select new DoctorModel
                                         {
                                             DepartmentName = d.DName,
                                             Name = u.FullName
                                       }).ToList();
            int appointmentsCount = appointements.Count()-1;
            int usersCount = users.Count() - 1;
            int departmentCount = departments.Count() - 1;
            ViewBag.Users = usersCount;
            ViewBag.Appointments = appointmentsCount;
            ViewBag.Doctors = doctors;
            ViewBag.Departments = departmentCount;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}