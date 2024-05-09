using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RepairServiceWeb.DAL;
using RepairServiceWeb.Domain.Entity;
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

        /// <summary>
        /// Метод для получения списка устройств
        /// </summary>
        /// <returns>Представление с инструментами взаимодействия со списком</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllDevices()
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");
            var resultStaff = await _roleCheckerService.Check(Request, "staff", "сотрудник");

            // Если пользователь не админ, не ресепшен и не сотрудник, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultReception is UnauthorizedResult && resultStaff is UnauthorizedResult)
                return Redirect("/");

            var response = await _deviceService.GetAll();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                if (response.Data != null)
                    return View(response.Data.ToList());
                else
                    return View(new List<Device>());

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для получения информации об устройстве по его id
        /// </summary>
        /// <param name="id" - код устройства></param>
        /// <returns>Частичное представление с инструментами взаимодействия с информацией</returns>
        [HttpGet]
        public async Task<IActionResult> GetDevices(int id)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ"); // Проверка роли пользователя

            // Если пользователь не админ, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult)
                return Redirect("/");

            var response = await _deviceService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для получения информации об устройстве по его названию
        /// </summary>
        /// <param name="name" - название устройства></param>
        /// <returns>Ответ в формате Json, содержащий имя найденного устройства</returns>
        [HttpGet]
        public async Task<IActionResult> GetDevicesByName(string name)
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");
            var resultStaff = await _roleCheckerService.Check(Request, "staff", "сотрудник");

            // Если пользователь не админ, не ресепшен и не сотрудник, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultReception is UnauthorizedResult && resultStaff is UnauthorizedResult)
                return Redirect("/");

            var response = await _deviceService.GetByName(name);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, highlightedName = name });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        /// <summary>
        /// Метод для получения отфильтрованного списка устройств
        /// </summary>
        /// <param name="manufacturer" - производитель устройств></param>
        /// <param name="type" - тип устройств></param>
        /// <param name="clientFullName" - хозяин устройств></param>
        /// <returns>Ответ в формате Json, содержащий список найденных устройств</returns>
        [HttpGet]
        public async Task<IActionResult> GetFilteredDevices(string manufacturer = "", string type = "", string clientFullName = "")
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");
            var resultStaff = await _roleCheckerService.Check(Request, "staff", "сотрудник");

            // Если пользователь не админ, не ресепшен и не сотрудник, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultReception is UnauthorizedResult && resultStaff is UnauthorizedResult)
                return Redirect("/");

            var response = await _deviceService.GetFiltered(manufacturer, type, clientFullName);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        /// <summary>
        /// Метод для удаления устройства
        /// </summary>
        /// <param name="id" - код устройства></param>
        /// <returns>Перенаправление на представление со списком всех устройств</returns>
        public async Task<IActionResult> DeleteDevices(int id)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ"); // Проверка роли пользователя

            // Если пользователь не админ, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult)
                return Redirect("/");

            var response = await _deviceService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return RedirectToAction("GetAllDevices");

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для проверки наличия устройства для дальнейшего редактирования существующего или добавления нового
        /// </summary>
        /// <param name="id" - код устройства></param>
        /// <returns>Если код = 0, то выводит частичное представление добавления устройства, если нет, то редактирования</returns>
        [HttpGet]
        public async Task<IActionResult> AddOrEditDevices(int id)
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");

            ViewBag.ClientsList = new SelectList(_context.Clients.ToList(), "Id", "FullName"); // Заполнение ViewBag списком клиентов

            if (id == 0)
            {
                // Если пользователь не админ и не ресепшен, то происходит перенаправление на стартовую страницу
                if (resultAdmin is UnauthorizedResult && resultReception is UnauthorizedResult)
                    return Redirect("/");

                return PartialView();
            }
            else
                // Если пользователь не админ, то происходит перенаправление на стартовую страницу
                if (resultAdmin is UnauthorizedResult)
                    return Redirect("/");

            var response = await _deviceService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для добавления или редактирвоания устройства
        /// </summary>
        /// <param name="model" - ViewModel></param>
        /// <returns>Если Get-версия метода вернула код = 0, то добавляет новое устройство, иначе редактирует существующее</returns>
        [HttpPost]
        public async Task<IActionResult> AddOrEditDevices(DevicesViewModel model, IFormFile? file = null)
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");

            // Если пользователь не админ и не ресепшен, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultReception is UnauthorizedResult)
                return Redirect("/");

            ViewBag.ClientsList = new SelectList(_context.Clients.ToList(), "Id", "FullName"); // Заполнение ViewBag списком клиентов

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
