using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services.AppointmentServices;
using HospitalManagementSystem.Services.BillServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HospitalManagementSystem.Controllers
{
    public class BillController : Controller
    {
        private readonly IBillServices _billServices;
        private readonly IAppointmentServices _appointmentServices;
        private readonly UserManager<ApplicationUser> _userManager;

        public BillController(IBillServices billServices, UserManager<ApplicationUser> userManager, IAppointmentServices appointmentServices)
        {
            _billServices = billServices;
            _userManager = userManager;
            _appointmentServices = appointmentServices;
        }
        [Authorize (Roles ="Admin")]
        public IActionResult BillDetails()
        {
            List<BillViewModel> billDetails = _billServices.Display();
            return View(billDetails);             
        }

        [Authorize (Roles="Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            List<SelectListItem> appointmentList = _appointmentServices.Display().Where(x=>x.Condition=="Approved").Select(x=>new SelectListItem { Text=x.Description,Value=x.Id.ToString()}).ToList();
            List<SelectListItem> usersList = _userManager.Users.Where(x => x.DepartmentId == 0).Select(x => new SelectListItem { Text = x.UserName, Value = x.Id }).ToList();
            ViewBag.Users = usersList;
            ViewBag.Appointment = appointmentList;
            return View();
        }
        [HttpPost]
        public IActionResult Create(BillModel model)
        {
            _billServices.AddBill(model);
            return RedirectToAction("BillDetails");
        }
        public async Task<IActionResult> BillUsers()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            List<BillViewModel> billDetails =_billServices.GetBillForUser(user.Id);
            return View(billDetails);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            BillViewModel bills = _billServices.Display().FirstOrDefault(x => x.Id == id);
            return View(bills);
        }
        [HttpPost]
        public IActionResult Edit(BillViewModel model)
        {
            BillModel bmodel = new BillModel
            {
                Id = model.Id,
                AppointmentId = model.AppointmentId,
                Amount = model.Amount,
                UserId = model.UserId,
                Condition = model.Condition
            };
            _billServices.UpdateBill(bmodel);
            return RedirectToAction("BillDetails");
        }

        public IActionResult Remove(int id)
        {
            _billServices.DeleteBill(id);
            return RedirectToAction("BillDetails");
        }

    }
}
