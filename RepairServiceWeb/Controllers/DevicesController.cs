using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RepairServiceWeb.DAL;
using RepairServiceWeb.Domain.ViewModels;
using RepairServiceWeb.Service.Interfaces;

namespace RepairServiceWeb.Controllers
{
    public class DevicesController : Controller
    {
        private readonly IDevicesService _deviceService;
        private readonly IRoleCheckerService _roleCheckerService;
        private readonly ApplicationDbContext _context;

        public DevicesController(IDevicesService deviceService, ApplicationDbContext context, IRoleCheckerService roleCheckerService)
        {
            _deviceService = deviceService;
            _context = context;
            _roleCheckerService = roleCheckerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDevices()
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");
            var resultStaff = await _roleCheckerService.Check(Request, "staff", "сотрудник");

            if (resultAdmin is UnauthorizedResult)
                if (resultReception is UnauthorizedResult)
                    if (resultStaff is UnauthorizedResult)
                        return Redirect("/");

            var response = await _deviceService.GetAll();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return View(response.Data.ToList());

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetDevices(int id)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");

            if (resultAdmin is UnauthorizedResult)
                return Redirect("/");

            var response = await _deviceService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetDevicesByName(string name)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");
            var resultStaff = await _roleCheckerService.Check(Request, "staff", "сотрудник");

            if (resultAdmin is UnauthorizedResult)
                if (resultReception is UnauthorizedResult)
                    if (resultStaff is UnauthorizedResult)
                        return Redirect("/");

            var response = await _deviceService.GetByName(name);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, highlightedName = name });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        [HttpGet]
        public async Task<IActionResult> GetFilteredDevices(string manufacturer = "", string type = "", string clientFullName = "")
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");
            var resultStaff = await _roleCheckerService.Check(Request, "staff", "сотрудник");

            if (resultAdmin is UnauthorizedResult)
                if (resultReception is UnauthorizedResult)
                    if (resultStaff is UnauthorizedResult)
                        return Redirect("/");

            var response = await _deviceService.GetFiltered(manufacturer, type, clientFullName);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        public async Task<IActionResult> DeleteDevices(int id)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");

            if (resultAdmin is UnauthorizedResult)
                return Redirect("/");

            var response = await _deviceService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return RedirectToAction("GetAllDevices");

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditDevices(int id)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");

            ViewBag.ClientsList = new SelectList(_context.Clients.ToList(), "Id", "FullName");

            if (id == 0)
            {
                if (resultAdmin is UnauthorizedResult)
                    if (resultReception is UnauthorizedResult)
                        return Redirect("/");

                return PartialView();
            }
            else
                if (resultAdmin is UnauthorizedResult)
                    return Redirect("/");

            var response = await _deviceService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditDevices(DevicesViewModel model, IFormFile? file = null)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");

            if (resultAdmin is UnauthorizedResult)
                if (resultReception is UnauthorizedResult)
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
