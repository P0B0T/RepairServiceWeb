using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepairServiceWeb.DAL;
using RepairServiceWeb.Domain.Entity;
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

        /// <summary>
        /// Метод для получения списка клиентов
        /// </summary>
        /// <returns>Представление с инструментами взаимодействия со списком</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllClients()
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");

            // Если пользователь не админ и не ресепшен, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultReception is UnauthorizedResult)
                return Redirect("/");

            var response = await _clientsService.GetAll();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                if (response.Data != null)
                    return View(response.Data.ToList());
                else
                    return View(new List<Client>());

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для получения информации о клиенте по его id
        /// </summary>
        /// <param name="id" - код клиента></param>
        /// <returns>Частичное представление с инструментами взаимодействия с информацией</returns>
        [HttpGet]
        public async Task<IActionResult> GetClients(int id)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ"); // Проверка роли пользователя

            // Если пользователь не админ, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult)
                return Redirect("/");

            var response = await _clientsService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для получения информации о клиенте по его имени
        /// </summary>
        /// <param name="name" - имя клиента></param>
        /// <returns>Ответ в формате Json, содержащий имя найденного клиента</returns>
        [HttpGet]
        public async Task<IActionResult> GetClientsByName(string name)
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");

            // Если пользователь не админ и не ресепшен, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultReception is UnauthorizedResult)
                return Redirect("/");

            var response = await _clientsService.GetByName(name);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, highlightedName = name });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        /// <summary>
        /// Метод для получения отфильтрованного списка клиентов
        /// </summary>
        /// <param name="fullName" - фио клиента></param>
        /// <param name="address" - адрес клиента></param>
        /// <returns>Ответ в формате Json, содержащий список найденных клиентов</returns>
        [HttpGet]
        public async Task<IActionResult> GetFilteredClients(string fullName = "", string address = "")
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");

            // Если пользователь не админ и не ресепшен, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultReception is UnauthorizedResult)
                return Redirect("/");

            var response = await _clientsService.GetFiltered(fullName, address);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        /// <summary>
        /// Метод для удаления клиента
        /// </summary>
        /// <param name="id" - код клиента></param>
        /// <returns>Перенаправление на представление со списком всех клиентов</returns>
        public async Task<IActionResult> DeleteClients(int id)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ"); // Проверка роли пользователя

            // Если пользователь не админ, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult)
                return Redirect("/");

            var response = await _clientsService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return RedirectToAction("GetAllClients");

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для проверки наличия клиента для дальнейшего редактирования существующего или добавления нового
        /// </summary>
        /// <param name="id" - код клиента></param>
        /// <returns>Если код = 0, то выводит частичное представление добавления клиента, если нет, то редактирования</returns>
        [HttpGet]
        public async Task<IActionResult> AddOrEditClients(int id)
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");
            var resultClient = await _roleCheckerService.Check(Request, "client", "клиент");

            if (id == 0)
            {
                // Если пользователь не админ и не ресепшен, то происходит перенаправление на стартовую страницу
                if (resultAdmin is UnauthorizedResult && resultReception is UnauthorizedResult)
                    return Redirect("/");

                return PartialView();
            }
            else
                // Если пользователь не админ и не клиент, то происходит перенаправление на стартовую страницу
                if (resultAdmin is UnauthorizedResult && resultClient is UnauthorizedResult)
                    return Redirect("/");

            var response = await _clientsService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для добавления или редактирования клиента
        /// </summary>
        /// <param name="model" - ViewModel></param>
        /// <returns>Если Get-версия метода вернула код = 0, то добавляет нового клиента, иначе редактирует существующего</returns>
        [HttpPost]
        public async Task<IActionResult> AddOrEditClients(ClientsViewModel model)
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");
            var resultClient = await _roleCheckerService.Check(Request, "client", "клиент");

            // Если пользователь не админ, не ресепшен и не клиент, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultReception is UnauthorizedResult && resultClient is UnauthorizedResult)
                return Redirect("/");

            // При добавлении нового клиента проверяется уникальность логина
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
