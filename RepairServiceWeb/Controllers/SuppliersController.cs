using Microsoft.AspNetCore.Mvc;
using RepairServiceWeb.Domain.Entity;
using RepairServiceWeb.Domain.ViewModels;
using RepairServiceWeb.Service.Interfaces;

namespace RepairServiceWeb.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly ISuppliersService _suppliersService;
        private readonly IRoleCheckerService _roleCheckerService;

        public SuppliersController(ISuppliersService suppliersService, IRoleCheckerService roleCheckerService)
        {
            _suppliersService = suppliersService;
            _roleCheckerService = roleCheckerService;
        }

        /// <summary>
        /// Метод для получения списка поставщиков
        /// </summary>
        /// <returns>Представление с инструментами взаимодействия со списком</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllSuppliers()
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");

            // Если пользователь не админ и не ресепшен, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultReception is UnauthorizedResult)
                return Redirect("/");

            var response = await _suppliersService.GetAll();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                if (response.Data != null)
                    return View(response.Data.ToList());
                else
                    return View(new List<Supplier>());

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для получения информации о поставщике по её id
        /// </summary>
        /// <param name="id" - код поставщика></param>
        /// <returns>Частичное представление с инструментами взаимодействия с информацией</returns>
        [HttpGet]
        public async Task<IActionResult> GetSuppliers(int id)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ"); // Проверка роли пользователя

            // Если пользователь не админ, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult)
                return Redirect("/");

            var response = await _suppliersService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для получения информации о поставщике по её названию
        /// </summary>
        /// <param name="name" - название компании поставщика></param>
        /// <returns>Ответ в формате Json, содержащий название компании найденного поставщика</returns>
        [HttpGet]
        public async Task<IActionResult> GetSuppliersByName(string name)
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");

            // Если пользователь не админ и не ресепшен, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultReception is UnauthorizedResult)
                return Redirect("/");

            var response = await _suppliersService.GetByName(name);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, highlightedName = name });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        /// <summary>
        /// Метод для получения отфильтрованного списка поставщиков
        /// </summary>
        /// <param name="address" - адрес поставщика></param>
        /// <returns>Ответ в формате Json, содержащий список найденных поставщиков</returns>
        [HttpGet]
        public async Task<IActionResult> GetFilteredSuppliers(string address = "")
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");

            // Если пользователь не админ и не ресепшен, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultReception is UnauthorizedResult)
                return Redirect("/");

            var response = await _suppliersService.GetFiltered(address);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        /// <summary>
        /// Метод для удаления поставщика
        /// </summary>
        /// <param name="id" - код поставщика></param>
        /// <returns>Перенаправление на представление со списком всех поставщиков</returns>
        public async Task<IActionResult> DeleteSuppliers(int id)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ"); // Проверка роли пользователя

            // Если пользователь не админ, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult)
                return Redirect("/");

            var response = await _suppliersService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return RedirectToAction("GetAllSuppliers");

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для проверки наличия поставщика для дальнейшего редактирования существующего или добавления нового
        /// </summary>
        /// <param name="id" - код запчасти></param>
        /// <returns>Если код = 0, то выводит частичное представление добавления запчасти, если нет, то редактирования</returns>
        [HttpGet]
        public async Task<IActionResult> AddOrEditSuppliers(int id)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ"); // Проверка роли пользователя

            // Если пользователь не админ, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult)
                return Redirect("/");

            if (id == 0)
                return PartialView();

            var response = await _suppliersService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для добавления или редактирования поставщика
        /// </summary>
        /// <param name="model" - ViewModel></param>
        /// <returns>Если Get-версия метода вернула код = 0, то добавляет нового поставщика, иначе редактирует существующего</returns>
        [HttpPost]
        public async Task<IActionResult> AddOrEditSuppliers(SuppliersViewModel model)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ"); // Проверка роли пользователя

            // Если пользователь не админ, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult)
                return Redirect("/");

            if (!ModelState.IsValid)
                return View(model);

            if (model.Id == 0)
                await _suppliersService.Create(model);
            else
                await _suppliersService.Edit(model.Id, model);

            TempData["Successfully"] = "Успешно";

            return RedirectToAction("GetAllSuppliers");
        }
    }
}
