using Diplom.DAL;
using Diplom.Domain.ViewModels;
using Diplom.Service.Implementations;
using Diplom.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Diplom.Controllers
{
    public class RepairsController : Controller
    {
        private readonly IRepairsService _repairsService;
        private readonly ApplicationDbContext _context;

        public RepairsController(IRepairsService repairsService, ApplicationDbContext context)
        {
            _repairsService = repairsService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRepairs()
        {
            var response = await _repairsService.GetAll();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return View(response.Data.ToList());

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetRepairs(int id)
        {
            var response = await _repairsService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<JsonResult> GetRepairsByName(string name)
        {
            var response = await _repairsService.GetByName(name);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, highlightedName = name });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        [HttpGet]
        public async Task<JsonResult> GetFilteredRepairs(string clientFullName = "", string staffFullName = "")
        {
            var response = await _repairsService.GetFiltered(clientFullName, staffFullName);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        public async Task<IActionResult> DeleteRepairs(int id)
        {
            var response = await _repairsService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return RedirectToAction("GetAllRepairs");

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditRepairs(int id)
        {
            FillViewBag();

            if (id == 0)
                return PartialView();

            var response = await _repairsService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                var repair = response.Data;

                var devices = _context.Devices.Where(d => d.ClientId == repair.Device.ClientId).ToList();

                ViewBag.DevicesList = new SelectList(devices, "Id", "Model");

                return PartialView(response.Data);
            }

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditRepairs(RepairsViewModel model)
        {
            FillViewBag();

            if (!ModelState.IsValid)
                return View(model);

            if (model.Id == 0)
                await _repairsService.Create(model);
            else
                await _repairsService.Edit(model.Id, model);

            TempData["Successfully"] = "Успешно";

            return RedirectToAction("GetAllRepairs");
        }

        public async Task<JsonResult> GetDevices(int clientId)
        {
            var devices = _context.Devices.Where(d => d.ClientId == clientId).ToList();

            return Json(devices);
        }

        private void FillViewBag()
        {
            ViewBag.ClientsList = new SelectList(_context.Clients.ToList(), "Id", "FullName");

            var workers = _context.Staff.Where(x => x.Role.Role1.ToLower() == "сотрудник" || x.Role.Role1.ToLower() == "employee");
            ViewBag.StaffList = new SelectList(workers.ToList(), "Id", "FullName");
        }
    }
}
