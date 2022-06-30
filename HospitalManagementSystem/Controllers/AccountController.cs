using HospitalManagementSystem.Models;
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
        

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,RoleManager<IdentityRole> roleManager,HMSDbContext hMSDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _hmsDbContext = hMSDbContext;
        }             
        //[Authorize (Roles="Admin")]
        public async Task<IActionResult> Index()
        {
            ApplicationUser CurrentUser = await _userManager.GetUserAsync(HttpContext.User);
            int currentRole = _userManager.GetRolesAsync(CurrentUser).Id;
            if (currentRole == 1)
            {
                return View(CurrentUser);
            }
            return RedirectToAction("Register");
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
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(model.Username);
                if(user!=null)
                {
                    bool result = await _userManager.CheckPasswordAsync(user, model.Password);
                    await _signInManager.SignInAsync(user, true);
                    return RedirectToAction("Index");
                }
            }
            return View(model);
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
        [Authorize (Roles ="Admin")]
        public async Task<IActionResult> AssignRole(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser User = _userManager.Users.FirstOrDefault(x => x.Id == model.UserId);
                IdentityRole Role = _roleManager.Roles.FirstOrDefault(x => x.Id == model.RoleId.ToString());
                if(User!=null & Role!= null)
                {
                    await _userManager.AddToRoleAsync(User, Role.Name);
                    return RedirectToAction("Index");
                }    
            }
            return View(model);
        }

        [HttpGet]
        [Authorize (Roles ="Admin")]
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
            List<SelectListItem> departmentList = _hmsDbContext.Department.Select(x => new SelectListItem {Text=x.DepartmentName,Value=x.Id.ToString() }).ToList();
            ViewBag.Doctors = doctorList;
            ViewBag.Departments = departmentList;
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignDepartment(ApplicationUser user)
        {
            
            ApplicationUser iUser= await _userManager.FindByIdAsync(user.Id);
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
