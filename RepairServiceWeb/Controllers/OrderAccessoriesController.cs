using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RepairServiceWeb.DAL;
using RepairServiceWeb.Domain.Entity;
using RepairServiceWeb.Domain.ViewModels;
using RepairServiceWeb.Service.Interfaces;

namespace RepairServiceWeb.Controllers
{
    public class OrderAccessoriesController : Controller
    {
        private readonly IOrderAccessoriesService _orderAccessoriesService;
        private readonly IRoleCheckerService _roleCheckerService;
        private readonly ApplicationDbContext _context;

        List<string> statusList = new List<string>()
        {
            "Создан",
            "В сборке",
            "В пути",
            "Ожидает получения",
            "Получен"
        }; // Коллекция статусов заказа

        public OrderAccessoriesController(IOrderAccessoriesService orderAccessoriesService, ApplicationDbContext context, IRoleCheckerService roleCheckerService)
        {
            _orderAccessoriesService = orderAccessoriesService;
            _context = context;
            _roleCheckerService = roleCheckerService;
        }

        /// <summary>
        /// Метод для получения списка заказов запчастей
        /// </summary>
        /// <returns>Представление с инструментами взаимодействия со списком</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllOrderAccessories()
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");

            // Если пользователь не админ, не ресепшен, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultReception is UnauthorizedResult)
                return Redirect("/");

            var response = await _orderAccessoriesService.GetAll();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                if (response.Data != null)
                    return View(response.Data.ToList());
                else
                    return View(new List<OrderAccessory>());

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для получения информации о заказе запчасти по его id
        /// </summary>
        /// <param name="id" - код устройства></param>
        /// <returns>Частичное представление с инструментами взаимодействия с информацией</returns>
        [HttpGet]
        public async Task<IActionResult> GetOrderAccessories(int id)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ"); // Проверка роли пользователя

            // Если пользователь не админ, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult)
                return Redirect("/");

            var response = await _orderAccessoriesService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для получения информации о заказе по названию запчасти
        /// </summary>
        /// <param name="name" - название устройства></param>
        /// <returns>Ответ в формате Json, содержащий название запчасти</returns>
        [HttpGet]
        public async Task<IActionResult> GetOrderAccessoriesByName(string name)
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");

            // Если пользователь не админ, не ресепшен, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultReception is UnauthorizedResult)
                return Redirect("/");

            var response = await _orderAccessoriesService.GetByName(name);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, highlightedName = name });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        /// <summary>
        /// Метод для получения отфильтрованного списка заказов запчастей
        /// </summary>
        /// <param name="clientFullName" - фио клиента></param>
        /// <param name="accessoryName" - название запчасти></param>
        /// <param name="count" - количество запчастей в заказе></param>
        /// <param name="date" - дата заказа></param>
        /// <param name="status" - статус заказа></param>
        /// <returns>Ответ в формате Json, содержащий список найденных заказов запчастей</returns>
        [HttpGet]
        public async Task<IActionResult> GetFilteredOrderAccessories(string clientFullName = "", string accessoryName = "", int? count = null, DateOnly date = default, string status = "")
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");

            // Если пользователь не админ, не ресепшен, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultReception is UnauthorizedResult)
                return Redirect("/");

            var response = await _orderAccessoriesService.GetFiltered(clientFullName, accessoryName, count, date, status);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        /// <summary>
        /// Метод для удаления заказа
        /// </summary>
        /// <param name="id" - код заказа></param>
        /// <returns>Перенаправление на представление со списком всех заказов запчастей</returns>
        public async Task<IActionResult> DeleteOrderAccessories(int id)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ"); // Проверка роли пользователя

            // Если пользователь не админ, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult)
                return Redirect("/");

            var response = await _orderAccessoriesService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return RedirectToAction("GetAllOrderAccessories");

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для проверки наличия заказа для дальнейшего редактирования существующего или добавления нового
        /// </summary>
        /// <param name="id" - код заказа></param>
        /// <returns>Если код = 0, то выводит частичное представление добавления заказа, если нет, то редактирования</returns>
        [HttpGet]
        public async Task<IActionResult> AddOrEditOrderAccessories(int id)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ"); // Проверка роли пользователя

            // Если пользователь не админ, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult)
                return Redirect("/");

            FillViewBag();

            if (id == 0)
                return PartialView();

            var response = await _orderAccessoriesService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для добавления или редактирования заказа
        /// </summary>
        /// <param name="model" - ViewModel></param>
        /// <returns>Если Get-версия метода вернула код = 0, то добавляет новый заказ запчасти, иначе редактирует существующую</returns>
        [HttpPost]
        public async Task<IActionResult> AddOrEditOrderAccessories(OrderAccessoriesViewModel model)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ"); // Проверка роли пользователя

            // Если пользователь не админ, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult)
                return Redirect("/");

            FillViewBag();

            if (!ModelState.IsValid)
                return View(model);

            if (model.Id == 0)
                await _orderAccessoriesService.Create(model);
            else
                await _orderAccessoriesService.Edit(model.Id, model);

            TempData["Successfully"] = "Успешно";

            return RedirectToAction("GetAllOrderAccessories");
        }

        /// <summary>
        /// Метод для заполнения ViewBags
        /// </summary>
        private void FillViewBag()
        {
            ViewBag.ClientsList = new SelectList(_context.Clients.ToList(), "Id", "FullName"); // Заполенение ViewBag списком клиентов
            ViewBag.AcessoriesList = new SelectList(_context.Accessories.ToList(), "Id", "Name"); // Заполенение ViewBag списком запчастей
            ViewBag.StatusList = new SelectList(statusList); // Заполенение ViewBag списком статусов
        }
    }
}
