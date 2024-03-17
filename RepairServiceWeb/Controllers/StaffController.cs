using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RepairServiceWeb.DAL;
using RepairServiceWeb.Domain.ViewModels;
using RepairServiceWeb.Service.Interfaces;

namespace RepairServiceWeb.Controllers
{
    public class StaffController : Controller
    {
        private readonly IStaffService _staffService;
        private readonly IRolesService _rolesService;
        private readonly ApplicationDbContext _context;

        public StaffController(IStaffService staffService, ApplicationDbContext context, IRolesService rolesService)
        {
            _staffService = staffService;
            _context = context;
            _rolesService = rolesService;
        }

        private async Task<StatusCodeResult> CheckRole()
        {
            var permissionId = int.Parse(Request.Cookies["permissions"]);

            var responce = await _rolesService.GetRoleName(permissionId);

            string data = responce.Data.ToLower();

            if (responce.StatusCode == Domain.Enum.StatusCode.OK)
                if (!data.Contains("admin") && !data.Contains("админ") && !data.Contains("human resources department") && !data.Contains("отдел кадров"))
                    return Unauthorized();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStaff()
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _staffService.GetAll();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return View(response.Data.ToList());

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetStaff(int id)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _staffService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetStaffByName(string name)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _staffService.GetByName(name);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, highlightedName = name });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        [HttpGet]
        public async Task<IActionResult> GetFilteredStaff(string fullName = "", int? experiance = null, string post = "", string role = null)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _staffService.GetFiltered(fullName, experiance, post, role);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        public async Task<IActionResult> DeleteStaff(int id)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _staffService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return RedirectToAction("GetAllStaff");

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditStaff(int id)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

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
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

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
