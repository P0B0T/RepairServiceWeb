using Diplom.DAL;
using Diplom.Domain.ViewModels;
using Diplom.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Diplom.Controllers
{
    public class DevicesController : Controller
    {
        private readonly IDevicesService _deviceService;
        private readonly IRolesService _rolesService;
        private readonly ApplicationDbContext _context;

        public DevicesController(IDevicesService deviceService, ApplicationDbContext context, IRolesService rolesService)
        {
            _deviceService = deviceService;
            _context = context;
            _rolesService = rolesService;
        }

        private async Task<StatusCodeResult> CheckRole()
        {
            var permissionId = int.Parse(Request.Cookies["permissions"]);

            var responce = await _rolesService.GetRoleName(permissionId);

            string data = responce.Data.ToLower();

            if (responce.StatusCode == Domain.Enum.StatusCode.OK)
                if (!data.Contains("admin") && !data.Contains("админ") && !data.Contains("ресепшен") && !data.Contains("reception"))
                    return Unauthorized();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDevices()
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _deviceService.GetAll();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return View(response.Data.ToList());

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetDevices(int id)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _deviceService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetDevicesByName(string name)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _deviceService.GetByName(name);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, highlightedName = name });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        [HttpGet]
        public async Task<IActionResult> GetFilteredDevices(string manufacturer = "", string type = "", string clientFullName = "")
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _deviceService.GetFiltered(manufacturer, type, clientFullName);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        public async Task<IActionResult> DeleteDevices(int id)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _deviceService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return RedirectToAction("GetAllDevices");

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditDevices(int id)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            ViewBag.ClientsList = new SelectList(_context.Clients.ToList(), "Id", "FullName");

            if (id == 0)
                return PartialView();

            var response = await _deviceService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditDevices(DevicesViewModel model, IFormFile? file = null)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            ViewBag.ClientsList = new SelectList(_context.Clients.ToList(), "Id", "FullName");

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Error", "Выберите фото повторно (если оно нужно).");

                return View(model);
            }

            if (model.Id == 0)
                await _deviceService.Create(model, file);
            else
                await _deviceService.Edit(model.Id, model, file);

            TempData["Successfully"] = "Успешно";

            return RedirectToAction("GetAllDevices");
        }
    }
}
