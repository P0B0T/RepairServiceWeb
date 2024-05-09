using Microsoft.AspNetCore.Mvc;
using RepairServiceWeb.Domain.ViewModels;
using RepairServiceWeb.Service.Interfaces;

namespace RepairServiceWeb.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRolesService _rolesService;
        private readonly IRoleCheckerService _roleCheckerService;

        public RolesController(IRolesService rolesService, IRoleCheckerService roleCheckerService)
        {
            _rolesService = rolesService;
            _roleCheckerService = roleCheckerService;
        }

        /// <summary>
        /// Метод для получения списка ролей
        /// </summary>
        /// <returns>Представление с инструментами взаимодействия со списком</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultHumanResourceDepartment = await _roleCheckerService.Check(Request, "human resources", "отдел кадров");

            // Если пользователь не админ и не отдел кадров, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultHumanResourceDepartment is UnauthorizedResult)
                return Redirect("/");

            var response = await _rolesService.GetAll();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return View(response.Data.ToList());

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для получения информации о роли по её id
        /// </summary>
        /// <param name="id" - код роли></param>
        /// <returns>Частичное представление с инструментами взаимодействия с информацией</returns>
        [HttpGet]
        public async Task<IActionResult> GetRoles(int id)
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultHumanResourceDepartment = await _roleCheckerService.Check(Request, "human resources", "отдел кадров");

            // Если пользователь не админ и не отдел кадров, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultHumanResourceDepartment is UnauthorizedResult)
                return Redirect("/");

            var response = await _rolesService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для удаления роли
        /// </summary>
        /// <param name="id" - код роли></param>
        /// <returns>Перенаправление на представление со списком всех ролей</returns>
        public async Task<IActionResult> DeleteRoles(int id)
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultHumanResourceDepartment = await _roleCheckerService.Check(Request, "human resources", "отдел кадров");

            // Если пользователь не админ и не отдел кадров, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultHumanResourceDepartment is UnauthorizedResult)
                return Redirect("/");

            var response = await _rolesService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return RedirectToAction("GetAllRoles");

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для проверки наличия роли для дальнейшего редактирования существующего или добавления новой
        /// </summary>
        /// <param name="id" - код роли></param>
        /// <returns>Если код = 0, то выводит частичное представление добавления роли, если нет, то редактирования</returns>
        [HttpGet]
        public async Task<IActionResult> AddOrEditRoles(int id)
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultHumanResourceDepartment = await _roleCheckerService.Check(Request, "human resources", "отдел кадров");

            // Если пользователь не админ и не отдел кадров, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultHumanResourceDepartment is UnauthorizedResult)
                return Redirect("/");

            if (id == 0)
                return PartialView();

            var response = await _rolesService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для добавления или редактирования роли
        /// </summary>
        /// <param name="model" - ViewModel></param>
        /// <returns>Если Get-версия метода вернула код = 0, то добавляет новую роль, иначе редактирует существующую</returns>
        [HttpPost]
        public async Task<IActionResult> AddOrEditRoles(RolesViewModel model)
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultHumanResourceDepartment = await _roleCheckerService.Check(Request, "human resources", "отдел кадров");

            // Если пользователь не админ и не отдел кадров, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultHumanResourceDepartment is UnauthorizedResult)
                return Redirect("/");

            if (!ModelState.IsValid)
                return View(model);

            if (model.Id == 0)
                await _rolesService.Create(model);
            else
                await _rolesService.Edit(model.Id, model);

            TempData["Successfully"] = "Успешно";

            return RedirectToAction("GetAllRoles");
        }
    }
}
