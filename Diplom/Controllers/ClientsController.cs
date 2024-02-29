using Diplom.DAL;
using Diplom.Domain.ViewModels;
using Diplom.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Diplom.Controllers
{
    public class ClientsController : Controller
    {
        public readonly IClientsService _clientsService;
        private readonly ApplicationDbContext _context;

        public ClientsController(IClientsService clientsService, ApplicationDbContext context)
        {
            _clientsService = clientsService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClients()
        {
            var response = await _clientsService.GetAll();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return View(response.Data.ToList());

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetClients(int id)
        {
            var response = await _clientsService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<JsonResult> GetClientsByName(string name)
        {
            var response = await _clientsService.GetByName(name);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, highlightedName = name });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        [HttpGet]
        public async Task<JsonResult> GetFilteredClients(string fullName = "", string address = "")
        {
            var response = await _clientsService.GetFiltered(fullName, address);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        public async Task<IActionResult> DeleteClients(int id)
        {
            var response = await _clientsService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return RedirectToAction("GetAllClients");

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditClients(int id)
        {
            ViewBag.RoleList = new SelectList(_context.Roles.ToList(), "Id", "Role1");

            if (id == 0)
                return PartialView();

            var response = await _clientsService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditClients(ClientsViewModel model)
        {
            ViewBag.RoleList = new SelectList(_context.Roles.ToList(), "Id", "Role1");

            if (!ModelState.IsValid)
                return View(model);

            model.Role = _context.Roles.FirstOrDefault(x => x.Role1 == "Клиент");

            if (model.Id == 0)
                await _clientsService.Create(model);
            else
                await _clientsService.Edit(model.Id, model);

            TempData["Successfully"] = "Успешно";

            return RedirectToAction("GetAllClients");
        }
    }
}
