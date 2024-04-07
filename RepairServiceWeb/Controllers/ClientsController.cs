using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepairServiceWeb.DAL;
using RepairServiceWeb.Domain.ViewModels;
using RepairServiceWeb.Service.Interfaces;

namespace RepairServiceWeb.Controllers
{
    public class ClientsController : Controller
    {
        public readonly IClientsService _clientsService;
        private readonly IRoleCheckerService _roleCheckerService;
        private readonly ApplicationDbContext _context;

        public ClientsController(IClientsService clientsService, ApplicationDbContext context, IRoleCheckerService roleCheckerService)
        {
            _clientsService = clientsService;
            _context = context;
            _roleCheckerService = roleCheckerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClients()
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");

            if (resultAdmin is UnauthorizedResult)
                if (resultReception is UnauthorizedResult)
                    return Redirect("/");

            var response = await _clientsService.GetAll();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return View(response.Data.ToList());

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetClients(int id)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");

            if (resultAdmin is UnauthorizedResult)
                return Redirect("/");

            var response = await _clientsService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetClientsByName(string name)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");

            if (resultAdmin is UnauthorizedResult)
                if (resultReception is UnauthorizedResult)
                    return Redirect("/");

            var response = await _clientsService.GetByName(name);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, highlightedName = name });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        [HttpGet]
        public async Task<IActionResult> GetFilteredClients(string fullName = "", string address = "")
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");

            if (resultAdmin is UnauthorizedResult)
                if (resultReception is UnauthorizedResult)
                    return Redirect("/");

            var response = await _clientsService.GetFiltered(fullName, address);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        public async Task<IActionResult> DeleteClients(int id)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");

            if (resultAdmin is UnauthorizedResult)
                return Redirect("/");

            var response = await _clientsService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return RedirectToAction("GetAllClients");

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditClients(int id)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");
            var resultClient = await _roleCheckerService.Check(Request, "client", "клиент");

            if (id == 0)
            {
                if (resultAdmin is UnauthorizedResult)
                    if (resultReception is UnauthorizedResult)
                        return Redirect("/");

                return PartialView();
            }
            else
                if (resultAdmin is UnauthorizedResult)
                    if (resultClient is UnauthorizedResult)
                        return Redirect("/");

            var response = await _clientsService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditClients(ClientsViewModel model)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");
            var resultClient = await _roleCheckerService.Check(Request, "client", "клиент");

            if (resultAdmin is UnauthorizedResult)
                if (resultReception is UnauthorizedResult)
                    if (resultClient is UnauthorizedResult)
                        return Redirect("/");

            if (model.Id == 0)
            {
                var loginCheck = await _context.Clients.FirstOrDefaultAsync(x => x.Login == model.Login);

                if (loginCheck != null)
                    ModelState.AddModelError("Login", "Пользователь с таким логином уже существует.");
            }

            if (!ModelState.IsValid)
                return View(model);

            model.RoleId = _context.Roles.FirstOrDefault(x => x.Role1.ToLower() == "клиент" || x.Role1.ToLower() == "client").Id;

            if (model.Id == 0)
                await _clientsService.Create(model);
            else
            {
                await _clientsService.Edit(model.Id, model);

                if (resultClient is OkResult)
                {
                    TempData["Successfully"] = "Успешно";

                    return RedirectToAction("PersonalCabinet", "PersonalCabinet", new { userId = model.Id, login = model.Login, password = model.Password });
                }
            }

            TempData["Successfully"] = "Успешно";

            return RedirectToAction("GetAllClients");
        }
    }
}
