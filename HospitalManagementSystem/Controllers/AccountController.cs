using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services.AccountServices;
using HospitalManagementSystem.Services.AppointmentServices;
using HospitalManagementSystem.Services.BillServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HospitalManagementSystem.Controllers
{

    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly HMSDbContext _hmsDbContext;
        private readonly IAccountServices _accountServices;
        private readonly IAppointmentServices _appointmentServices;
        private readonly IBillServices _billServices;


        public AccountController(
            IAppointmentServices appointmentServices,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, HMSDbContext hMSDbContext, IAccountServices accountServices,IBillServices billServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _hmsDbContext = hMSDbContext;
            _accountServices = accountServices;
            _appointmentServices = appointmentServices;
            _billServices = billServices;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser CurrentUser = await _userManager.GetUserAsync(HttpContext.User);

            string currentRole = _accountServices.GetRoleID(CurrentUser);
            if (currentRole == "Admin")

                return RedirectToAction("AdminProfile");

            else if (currentRole == "Doctor")

                return RedirectToAction("DoctorProfile");

            else if (currentRole == "Patient")

                return RedirectToAction("PatientProfile");

            else
                return RedirectToAction("/");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminProfile()
        {
            ApplicationUser CurrentUser = await _userManager.GetUserAsync(HttpContext.User);
            int appointmentLength = _appointmentServices.Display().Where(x => x.Condition == "Pending").Count();

            ViewBag.AppointmentLength = appointmentLength;
            return View(CurrentUser);
        }
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> DoctorProfile()
        {
            ApplicationUser CurrentUser = await _userManager.GetUserAsync(HttpContext.User);
            String did = CurrentUser.Id;
            List<DepartmentModel> department = _accountServices.GetDepartment(did);
            List<AppointmentModel> appointments = _accountServices.GetAppointmentDetails(did);
            ViewBag.Department = department;
            ViewBag.Appointment = appointments;
            return View(CurrentUser);
        }

        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> PatientProfile()
        {
            ApplicationUser CurrentUser = await _userManager.GetUserAsync(HttpContext.User);
            String uid = CurrentUser.Id;
            List<AppointmentModel> appointments = _accountServices.GetAppointmentForUser(uid);
            int billCount = _billServices.GetBillForUser(uid).Where(x => x.Condition == "Pending").Count();
            ViewBag.BillCount = billCount;
            ViewBag.Appointment = appointments;
            return View(CurrentUser);
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(UserModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    ApplicationUser User = new ApplicationUser
                    {
                        UserName = model.Username,
                        FullName = model.FullName,
                        PhoneNumber = model.PhoneNumber
                    };
                    IdentityResult result = await _userManager.CreateAsync(User, model.Password);

                    if (result.Succeeded)
                    {

                        return Redirect("Login");
                    }
                    TempData["ResponseMessage"] = "User Registered Successfully";
                    TempData["ResponseValue"] = "1";
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                TempData["ResponseMessage"] = "User Registered Failed!";
                TempData["ResponseValue"] = "0";
                
            }
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser user = await _userManager.FindByNameAsync(model.Username);
                    if (user != null)
                    {
                        bool result = await _userManager.CheckPasswordAsync(user, model.Password);
                        if (result)
                        {
                            await _signInManager.SignInAsync(user, result);
                            return Redirect("/Home");
                        }
                        else
                        {
                            TempData["ResponseMessage"] = "Password Wrong";
                            TempData["ResponseValue"] = "0";
                            return View(model);
                        }
                    }
                    else
                    {
                        TempData["ResponseMessage"] = "UserName Not Found";
                        TempData["ResponseValue"] = "0";
                        return View(model);
                    }
                }
                else
                {
                    TempData["ResponseMessage"] = "Please Enter the Correct Data";
                    TempData["ResponseValue"] = "0";    
                    return View(model);
                }
            }
            catch
            {
                TempData["ResponseMessage"] = "Something Went Wrong! Please try Agaian";
                TempData["ResponseValue"] = "0";
                return View(model);
            }
            

        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Home");
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AssignRole()
        {
            List<SelectListItem> userList = _userManager.Users.Select(x => new SelectListItem { Text = x.NormalizedUserName, Value = x.Id }).ToList();
            List<SelectListItem> roleList = _roleManager.Roles.Select(x => new SelectListItem { Text = x.Name, Value = x.Id }).ToList();
            ViewBag.Users = userList;
            ViewBag.Roles = roleList;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser User = _userManager.Users.FirstOrDefault(x => x.Id == model.UserId);
                IdentityRole Role = _roleManager.Roles.FirstOrDefault(x => x.Id == model.RoleId.ToString());
                if (User != null & Role != null)
                {
                    await _userManager.AddToRoleAsync(User, Role.Name);
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AssignDepartment()
        {
            List<ApplicationUser> Doctors = (from u in _userManager.Users
                                             join r in _hmsDbContext.UserRoles
                                             on u.Id equals r.UserId
                                             where r.RoleId == "2"
                                             select new ApplicationUser
                                             {
                                                 Id = u.Id,
                                                 FullName = u.FullName

                                             }).ToList();

            List<SelectListItem> doctorList = Doctors.Select(x => new SelectListItem { Text = x.FullName, Value = x.Id }).ToList();
            List<SelectListItem> departmentList = _hmsDbContext.Department.Select(x => new SelectListItem { Text = x.DName, Value = x.Id.ToString() }).ToList();
            ViewBag.Doctors = doctorList;
            ViewBag.Departments = departmentList;
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignDepartment(ApplicationUser user)
        {

            ApplicationUser iUser = await _userManager.FindByIdAsync(user.Id);
            iUser.DepartmentId = user.DepartmentId;
            IdentityResult result = await _userManager.UpdateAsync(iUser);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return View(user);

        }
    }
}
