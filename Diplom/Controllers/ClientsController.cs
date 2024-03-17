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
        private readonly IRolesService _rolesService;
        private readonly ApplicationDbContext _context;

        public ClientsController(IClientsService clientsService, ApplicationDbContext context, IRolesService rolesService)
        {
            _clientsService = clientsService;
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
        public async Task<IActionResult> GetAllClients()
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _clientsService.GetAll();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return View(response.Data.ToList());

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetClients(int id)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _clientsService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetClientsByName(string name)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _clientsService.GetByName(name);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, highlightedName = name });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        [HttpGet]
        public async Task<IActionResult> GetFilteredClients(string fullName = "", string address = "")
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _clientsService.GetFiltered(fullName, address);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        public async Task<IActionResult> DeleteClients(int id)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _clientsService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return RedirectToAction("GetAllClients");

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditClients(int id)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

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
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            if (!ModelState.IsValid)
                return View(model);

            model.RoleId = _context.Roles.FirstOrDefault(x => x.Role1.ToLower() == "клиент" || x.Role1.ToLower() == "client").Id;

            if (model.Id == 0)
                await _clientsService.Create(model);
            else
                await _clientsService.Edit(model.Id, model);

            TempData["Successfully"] = "Успешно";

            return RedirectToAction("GetAllClients");
        }
    }
}
