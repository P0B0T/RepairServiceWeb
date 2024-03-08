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
        private readonly ApplicationDbContext _context;

        public DevicesController(IDevicesService deviceService, ApplicationDbContext context)
        {
            _deviceService = deviceService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDevices()
        {
            var response = await _deviceService.GetAll();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return View(response.Data.ToList());

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetDevices(int id)
        {
            var response = await _deviceService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<JsonResult> GetDevicesByName(string name)
        {
            var response = await _deviceService.GetByName(name);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, highlightedName = name });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        [HttpGet]
        public async Task<JsonResult> GetFilteredDevices(string manufacturer = "", string type = "", string clientFullName = "")
        {
            var response = await _deviceService.GetFiltered(manufacturer, type, clientFullName);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        public async Task<IActionResult> DeleteDevices(int id)
        {
            var response = await _deviceService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return RedirectToAction("GetAllDevices");

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditDevices(int id)
        {
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
            ViewBag.ClientsList = new SelectList(_context.Clients.ToList(), "Id", "FullName");

            if (!ModelState.IsValid)
                return View(model);

            if (model.Id == 0)
                await _deviceService.Create(model, file);
            else
                await _deviceService.Edit(model.Id, model, file);

            TempData["Successfully"] = "Успешно";

            return RedirectToAction("GetAllDevices");
        }
    }
}
