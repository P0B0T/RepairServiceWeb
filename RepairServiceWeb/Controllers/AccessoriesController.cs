using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RepairServiceWeb.DAL;
using RepairServiceWeb.Domain.Entity;
using RepairServiceWeb.Domain.ViewModels;
using RepairServiceWeb.Service.Interfaces;

namespace RepairServiceWeb.Controllers
{
    public class AccessoriesController : Controller
    {
        private readonly IAccessoriesService _accessoriesService;
        private readonly IRoleCheckerService _roleCheckerService;
        private readonly ApplicationDbContext _context;

        public AccessoriesController(IAccessoriesService accessoriesService, ApplicationDbContext context, IRoleCheckerService roleCheckerService)
        {
            _accessoriesService = accessoriesService;
            _context = context;
            _roleCheckerService = roleCheckerService;
        }

        /// <summary>
        /// Метод для получения списка запчастей
        /// </summary>
        /// <returns>Представление с инструментами взаимодействия со списком</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllAccessories()
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");
            var resultStaff = await _roleCheckerService.Check(Request, "staff", "сотрудник");

            // Если пользователь не админ, не ресепшен и не сотрудник, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultReception is UnauthorizedResult && resultStaff is UnauthorizedResult)
                return Redirect("/");

            var response = await _accessoriesService.GetAll();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                if (response.Data != null)
                    return View(response.Data.ToList());
                else
                    return View(new List<Accessory>());

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для получения информации о запчасти по её id
        /// </summary>
        /// <param name="id" - код устройства></param>
        /// <returns>Частичное представление с инструментами взаимодействия с информацией</returns>
        [HttpGet]
        public async Task<IActionResult> GetAccessories(int id)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ"); // Проверка роли пользователя

            // Если пользователь не админ, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult)
                return Redirect("/");

            var response = await _accessoriesService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для получения информации о запчасти по её названию
        /// </summary>
        /// <param name="name" - название устройства></param>
        /// <returns>Ответ в формате Json, содержащий название найденной запчасти</returns>
        [HttpGet]
        public async Task<IActionResult> GetAccessoriesByName(string name)
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");
            var resultStaff = await _roleCheckerService.Check(Request, "staff", "сотрудник");

            // Если пользователь не админ, не ресепшен и не сотрудник, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultReception is UnauthorizedResult && resultStaff is UnauthorizedResult)
                return Redirect("/");

            var response = await _accessoriesService.GetByName(name);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, highlightedName = name });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        /// <summary>
        /// Метод для получения отфильтрованного списка запчастей
        /// </summary>
        /// <param name="Name" - название запчасти></param>
        /// <param name="manufacturer" - производитель запчасти></param>
        /// <param name="supplier" - поставщик запчасти></param>
        /// <returns>Ответ в формате Json, содержащий список найденных запчастей</returns>
        [HttpGet]
        public async Task<IActionResult> GetFilteredAccessories(string Name = "", string manufacturer = "", string supplier = "")
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");
            var resultStaff = await _roleCheckerService.Check(Request, "staff", "сотрудник");

            // Если пользователь не админ, не ресепшен и не сотрудник, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultReception is UnauthorizedResult && resultStaff is UnauthorizedResult)
                return Redirect("/");

            var response = await _accessoriesService.GetFiltered(Name, manufacturer, supplier);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        /// <summary>
        /// Метод для удаления запчасти
        /// </summary>
        /// <param name="id" - код запчасти></param>
        /// <returns>Перенаправление на представление со списком всех запчастей</returns>
        public async Task<IActionResult> DeleteAccessories(int id)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ"); // Проверка роли пользователя

            // Если пользователь не админ, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult)
                return Redirect("/");

            var response = await _accessoriesService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return RedirectToAction("GetAllAccessories");

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для проверки наличия запчасти для дальнейшего редактирования существующей или добавления новой
        /// </summary>
        /// <param name="id" - код запчасти></param>
        /// <returns>Если код = 0, то выводит частичное представление добавления запчасти, если нет, то редактирования</returns>
        [HttpGet]
        public async Task<IActionResult> AddOrEditAccessories(int id)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ"); // Проверка роли пользователя

            // Если пользователь не админ, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult)
                return Redirect("/");

            ViewBag.SuppliersList = new SelectList(_context.Suppliers.ToList(), "Id", "CompanyName"); // Заполнение ViewBag списком поставщиков

            if (id == 0)
                return PartialView();

            var response = await _accessoriesService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для добавления или редактирования запчасти
        /// </summary>
        /// <param name="model" - ViewModel></param>
        /// <returns>Если Get-версия метода вернула код = 0, то добавляет новую запчасть, иначе редактирует существующую</returns>
        [HttpPost]
        public async Task<IActionResult> AddOrEditAccessories(AccessoriesViewModel model)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ"); // Проверка роли пользователя

            // Если пользователь не админ, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult)
                return Redirect("/");

            ViewBag.SuppliersList = new SelectList(_context.Suppliers.ToList(), "Id", "CompanyName"); // Заполнение ViewBag списком поставщиков

            if (!ModelState.IsValid)
                return View(model);

            if (model.Id == 0)
                await _accessoriesService.Create(model);
            else
                await _accessoriesService.Edit(model.Id, model);

            TempData["Successfully"] = "Успешно";

            return RedirectToAction("GetAllAccessories");
        }
    }
}