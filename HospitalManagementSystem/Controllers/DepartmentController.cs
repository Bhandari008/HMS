using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services.DepartmentServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {

        private readonly IDepartmentServices _departmentServices;
        public DepartmentController(IDepartmentServices departmentServices)
        {
            _departmentServices = departmentServices;
        }
        public IActionResult Index()
        {
            List<DepartmentModel> departments=_departmentServices.Display();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(DepartmentModel model)
        {
            if (ModelState.IsValid)
            {
                _departmentServices.Add(model);
                return RedirectToAction("Index");
            }
            return View(model);

        }
        public IActionResult Delete(int id)
        {
            try
            {
                _departmentServices.Delete(id);
                TempData["ResponseMessage"] = "Successfully Removed Department";
                TempData["ResponseValue"] = "1";
                return RedirectToAction("Index");

            }
            catch
            {
                TempData["ResponseMessage"] = "Patient has appointment associated with this department";
                TempData["ResponseValue"] = "0";
                return RedirectToAction("Index");
            }

        }


    }
}
