using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RepairServiceWeb.DAL;
using RepairServiceWeb.Domain.Entity;
using RepairServiceWeb.Domain.ViewModels;
using RepairServiceWeb.Service.Interfaces;

namespace RepairServiceWeb.Controllers
{
    public class RepairsController : Controller
    {
        private readonly IRepairsService _repairsService;
        private readonly IRoleCheckerService _roleCheckerService;
        private readonly ApplicationDbContext _context;

        List<string> statusList = new List<string>()
        {
            "Принят в работу",
            "Выполняется",
            "Ожидание запчастей",
            "Выполнен"
        }; // коллекция статусов ремонта

        public RepairsController(IRepairsService repairsService, ApplicationDbContext context, IRoleCheckerService roleCheckerService)
        {
            _repairsService = repairsService;
            _context = context;
           _roleCheckerService = roleCheckerService;
        }

        /// <summary>
        /// Метод для получения списка ремонтов
        /// </summary>
        /// <returns>Представление с инструментами взаимодействия со списком</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllRepairs()
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");
            var resultManager = await _roleCheckerService.Check(Request, "manager", "менеджер");

            // Если пользователь не админ, не ресепшен и не менеджер, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultReception is UnauthorizedResult && resultManager is UnauthorizedResult)
                return Redirect("/");

            var response = await _repairsService.GetAll();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                if (response.Data != null)
                    return View(response.Data.ToList());
                else
                    return View(new List<Repair>());

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для получения информации о ремонте по его id
        /// </summary>
        /// <param name="id" - код ремонта></param>
        /// <returns>Частичное представление с инструментами взаимодействия с информацией</returns>
        [HttpGet]
        public async Task<IActionResult> GetRepairs(int id)
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultManager = await _roleCheckerService.Check(Request, "manager", "менеджер");

            // Если пользователь не админ и не менеджер, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultManager is UnauthorizedResult)
                return Redirect("/");

            var response = await _repairsService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для получения информации о ремонте по модели устройства
        /// </summary>
        /// <param name="name" - модель устройства></param>
        /// <returns>Ответ в формате Json, содержащий модель найденного устройства</returns>
        [HttpGet]
        public async Task<IActionResult> GetRepairsByName(string name)
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");
            var resultManager = await _roleCheckerService.Check(Request, "manager", "менеджер");

            // Если пользователь не админ, не ресепшен и не менеджер, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultReception is UnauthorizedResult && resultManager is UnauthorizedResult)
                return Redirect("/");

            var response = await _repairsService.GetByName(name);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, highlightedName = name });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        /// <summary>
        /// Метод для получения отфильтрованного списка ремонтов
        /// </summary>
        /// <param name="clientFullName" - фио клиента></param>
        /// <param name="staffFullName" - фио сотрудника></param>
        /// <returns>Ответ в формате Json, содержащий список найденных ремонтов</returns>
        [HttpGet]
        public async Task<IActionResult> GetFilteredRepairs(string clientFullName = "", string staffFullName = "")
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");
            var resultManager = await _roleCheckerService.Check(Request, "manager", "менеджер");

            // Если пользователь не админ, не ресепшен и не менеджер, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultReception is UnauthorizedResult && resultManager is UnauthorizedResult)
                return Redirect("/");

            var response = await _repairsService.GetFiltered(clientFullName, staffFullName);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        /// <summary>
        /// Метод для удаления ремонта
        /// </summary>
        /// <param name="id" - код ремонта></param>
        /// <returns>Перенаправление на представление со списком всех ремонтов</returns>
        public async Task<IActionResult> DeleteRepairs(int id)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ"); // Проверка роли пользователя

            // Если пользователь не админ, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult)
                return Redirect("/");

            var response = await _repairsService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return RedirectToAction("GetAllRepairs");

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для проверки наличия ремонта для дальнейшего редактирования существующего или добавления нового
        /// </summary>
        /// <param name="id" - код ремонта></param>
        /// <returns>Если код = 0, то выводит частичное представление добавления ремонта, если нет, то редактирования</returns>
        [HttpGet]
        public async Task<IActionResult> AddOrEditRepairs(int id)
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultManager = await _roleCheckerService.Check(Request, "manager", "менеджер");

            // Если пользователь не админ и не менеджер, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultManager is UnauthorizedResult)
                return Redirect("/");

            FillViewBag();

            if (id == 0)
                return PartialView();

            var response = await _repairsService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                var repair = response.Data;

                var devices = _context.Devices.Where(d => d.ClientId == repair.Device.ClientId).ToList(); // Получение списка устройств по коду клиента

                ViewBag.DevicesList = new SelectList(devices, "Id", "Model"); // Заполнение ViewBag списком устройств

                return PartialView(response.Data);
            }

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для добавления или редактирования ремонта
        /// </summary>
        /// <param name="model" - ViewModel></param>
        /// <returns>Если Get-версия метода вернула код = 0, то добавляет новый ремонт, иначе редактирует существующую</returns>
        [HttpPost]
        public async Task<IActionResult> AddOrEditRepairs(RepairsViewModel model)
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultManager = await _roleCheckerService.Check(Request, "manager", "менеджер");

            // Если пользователь не админ и не менеджер, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultManager is UnauthorizedResult)
                return Redirect("/");

            FillViewBag();

            if (!ModelState.IsValid)
            {
                ModelState.Remove("DeviceId");
                model.DeviceId = 0;

                ModelState.AddModelError("Error", "Выберите клиента ещё раз.");

                return View(model);
            }

            if (model.Id == 0)
                await _repairsService.Create(model);
            else
                await _repairsService.Edit(model.Id, model);

            TempData["Successfully"] = "Успешно";

            return RedirectToAction("GetAllRepairs");
        }

        /// <summary>
        /// Метод для получения устройств конкретного пользователя
        /// </summary>
        /// <param name="clientId" - код клиента></param>
        /// <returns>Список устройств в формате Json</returns>
        public async Task<IActionResult> GetDevices(int clientId)
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultManager = await _roleCheckerService.Check(Request, "manager", "менеджер");

            // Если пользователь не админ и не менеджер, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultManager is UnauthorizedResult)
                return Redirect("/");

            var devices = _context.Devices.Where(d => d.ClientId == clientId).ToList(); // Получение списка устройств по коду клиента

            return Json(devices);
        }

        /// <summary>
        /// Метод для заполнения ViewBags
        /// </summary>
        private void FillViewBag()
        {
            ViewBag.ClientsList = new SelectList(_context.Clients.ToList(), "Id", "FullName"); // Заполнение ViewBag списком клиентов

            var workers = _context.Staff.Where(x => x.Role.Role1.ToLower() == "сотрудник" || x.Role.Role1.ToLower() == "employee"); // Получение списка сотрудников
            ViewBag.StaffList = new SelectList(workers.ToList(), "Id", "FullName"); // Заполнение ViewBag списком сотрудников

            ViewBag.StatusList = new SelectList(statusList); // Заполнение ViewBag списком возможных статусов
        }
    }
}
