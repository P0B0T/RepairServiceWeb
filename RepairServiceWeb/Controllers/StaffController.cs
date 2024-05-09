using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RepairServiceWeb.DAL;
using RepairServiceWeb.Domain.ViewModels;
using RepairServiceWeb.Service.Interfaces;

namespace RepairServiceWeb.Controllers
{
    public class StaffController : Controller
    {
        private readonly IStaffService _staffService;
        private readonly IRoleCheckerService _roleCheckerService;
        private readonly ApplicationDbContext _context;

        public StaffController(IStaffService staffService, ApplicationDbContext context, IRoleCheckerService roleCheckerService)
        {
            _staffService = staffService;
            _context = context;
            _roleCheckerService = roleCheckerService;
        }

        /// <summary>
        /// Метод для получения списка сотрудников
        /// </summary>
        /// <returns>Представление с инструментами взаимодействия со списком</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllStaff()
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultHumanResourceDepartment = await _roleCheckerService.Check(Request, "human resources", "отдел кадров");

            // Если пользователь не админ и не отдел кадров, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultHumanResourceDepartment is UnauthorizedResult)
                return Redirect("/");

            var response = await _staffService.GetAll();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return View(response.Data.ToList());

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для получения информации о сотруднике по его id
        /// </summary>
        /// <param name="id" - код сотрудника></param>
        /// <returns>Частичное представление с инструментами взаимодействия с информацией</returns>
        [HttpGet]
        public async Task<IActionResult> GetStaff(int id)
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultHumanResourceDepartment = await _roleCheckerService.Check(Request, "human resources", "отдел кадров");

            // Если пользователь не админ и не отдел кадров, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultHumanResourceDepartment is UnauthorizedResult)
                return Redirect("/");

            var response = await _staffService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для получения информации о сотруднике по его фио
        /// </summary>
        /// <param name="name" - фио сотрудника></param>
        /// <returns>Ответ в формате Json, содержащий фио найденного сотрудника</returns>
        [HttpGet]
        public async Task<IActionResult> GetStaffByName(string name)
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultHumanResourceDepartment = await _roleCheckerService.Check(Request, "human resources", "отдел кадров");

            // Если пользователь не админ и не отдел кадров, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultHumanResourceDepartment is UnauthorizedResult)
                return Redirect("/");

            var response = await _staffService.GetByName(name);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, highlightedName = name });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        /// <summary>
        /// Метод для получения отфильтрованного списка сотрудников
        /// </summary>
        /// <param name="fullName" - фио сотрудника></param>
        /// <param name="experiance" - стаж сотрудника></param>
        /// <param name="post" - должность сотрудника></param>
        /// <param name="role" - роль сотрудника></param>
        /// <returns>Ответ в формате Json, содержащий список найденных сотрудников</returns>
        [HttpGet]
        public async Task<IActionResult> GetFilteredStaff(string fullName = "", int? experiance = null, string post = "", string role = null)
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultHumanResourceDepartment = await _roleCheckerService.Check(Request, "human resources", "отдел кадров");

            // Если пользователь не админ и не отдел кадров, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultHumanResourceDepartment is UnauthorizedResult)
                return Redirect("/");

            var response = await _staffService.GetFiltered(fullName, experiance, post, role);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        /// <summary>
        /// Метод для удаления сотрудника
        /// </summary>
        /// <param name="id" - код сотрудника></param>
        /// <returns>Перенаправление на представление со списком всех сотрудников</returns>
        public async Task<IActionResult> DeleteStaff(int id)
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultHumanResourceDepartment = await _roleCheckerService.Check(Request, "human resources", "отдел кадров");

            // Если пользователь не админ и не отдел кадров, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultHumanResourceDepartment is UnauthorizedResult)
                return Redirect("/");

            var response = await _staffService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return RedirectToAction("GetAllStaff");

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для проверки наличия сотрудника для дальнейшего редактирования существующего или добавления нового
        /// </summary>
        /// <param name="id" - код сотрудника></param>
        /// <returns>Если код = 0, то выводит частичное представление добавления сотрудника, если нет, то редактирования</returns>
        [HttpGet]
        public async Task<IActionResult> AddOrEditStaff(int id)
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultHumanResourceDepartment = await _roleCheckerService.Check(Request, "human resources", "отдел кадров");

            // Если пользователь не админ и не отдел кадров, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultHumanResourceDepartment is UnauthorizedResult)
                return Redirect("/");

            GetRolesNoClient();

            if (id == 0)
                return PartialView();

            var response = await _staffService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        /// <summary>
        /// Метод для добавления или редактирования сотрудника
        /// </summary>
        /// <param name="model" - ViewModel></param>
        /// <returns>Если Get-версия метода вернула код = 0, то добавляет нового сотрудника, иначе редактирует существующую</returns>
        [HttpPost]
        public async Task<IActionResult> AddOrEditStaff(StaffViewModel model, IFormFile? file = null)
        {
            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultHumanResourceDepartment = await _roleCheckerService.Check(Request, "human resources", "отдел кадров");

            // Если пользователь не админ и не отдел кадров, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultHumanResourceDepartment is UnauthorizedResult)
                return Redirect("/");

            GetRolesNoClient();

            if (model.Id == 0) 
            {
                var loginCheck = await _context.Staff.FirstOrDefaultAsync(x => x.Login == model.Login);

                if (loginCheck != null)
                    ModelState.AddModelError("Login", "Пользователь с таким логином уже существует.");
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Error", "Выберите фото повторно (если оно нужно).");

                return View(model);
            }

            if (model.Id == 0)
                await _staffService.Create(model, file);
            else
                await _staffService.Edit(model.Id, model, file);

            TempData["Successfully"] = "Успешно";

            return RedirectToAction("GetAllStaff");
        }

        /// <summary>
        /// Метод для получения списка ролей без роли "клиент"
        /// </summary>
        private void GetRolesNoClient()
        {
            var roles = _context.Roles.Where(x => x.Role1.ToLower() != "клиент" && x.Role1.ToLower() != "client"); // Получение списка ролей

            ViewBag.RolesList = new SelectList(roles.ToList(), "Id", "Role1"); // Заполнение ViewBag списком ролей
        }
    }
}
