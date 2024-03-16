using Diplom.DAL;
using Diplom.Domain.ViewModels;
using Diplom.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Diplom.Controllers
{
    public class StaffController : Controller
    {
        private readonly IStaffService _staffService;
        private readonly ApplicationDbContext _context;

        public StaffController(IStaffService staffService, ApplicationDbContext context)
        {
            _staffService = staffService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStaff()
        {
            var response = await _staffService.GetAll();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return View(response.Data.ToList());

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetStaff(int id)
        {
            var response = await _staffService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<JsonResult> GetStaffByName(string name)
        {
            var response = await _staffService.GetByName(name);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, highlightedName = name });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        [HttpGet]
        public async Task<JsonResult> GetFilteredStaff(string fullName = "", int? experiance = null, string post = "", string role = null)
        {
            var response = await _staffService.GetFiltered(fullName, experiance, post, role);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        public async Task<IActionResult> DeleteStaff(int id)
        {
            var response = await _staffService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return RedirectToAction("GetAllStaff");

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditStaff(int id)
        {
            GetRolesNoClient();

            if (id == 0)
                return PartialView();

            var response = await _staffService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditStaff(StaffViewModel model, IFormFile? file = null)
        {
            GetRolesNoClient();

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Error", "Выберите фото повторно (если оно нужно).");

                return View(model);
            }

            if (model.Id == 0)
                await _staffService.Create(model, file);
            else
                await _staffService.Edit(model.Id, model, file);

            TempData["Successfully"] = "Успешно";

            return RedirectToAction("GetAllStaff");
        }

        private void GetRolesNoClient()
        {
            var roles = _context.Roles.Where(x => x.Role1.ToLower() != "клиент" && x.Role1.ToLower() != "client");

            ViewBag.RolesList = new SelectList(roles.ToList(), "Id", "Role1");
        }
    }
}
