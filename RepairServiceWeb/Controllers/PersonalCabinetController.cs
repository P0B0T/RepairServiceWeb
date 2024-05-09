using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepairServiceWeb.DAL;
using RepairServiceWeb.Domain.Entity;
using RepairServiceWeb.Service.Interfaces;

namespace RepairServiceWeb.Controllers
{
    public class PersonalCabinetController : Controller
    {
        private readonly IRepairsService _repairsService;
        private readonly IDevicesService _devicesService;
        private readonly IOrderAccessoriesService _orderAccessoriesService;
        private readonly IRoleCheckerService _roleCheckerService;
        private readonly ApplicationDbContext _context;

        public PersonalCabinetController(ApplicationDbContext context, IRepairsService repairsService, IDevicesService devicesService, IOrderAccessoriesService orderAccessoriesService, IRoleCheckerService roleCheckerService)
        {
            _context = context;
            _repairsService = repairsService;
            _devicesService = devicesService;
            _orderAccessoriesService = orderAccessoriesService;
            _roleCheckerService = roleCheckerService;
        }

        /// <summary>
        /// Метод для отображения персонального кабинета
        /// </summary>
        /// <returns>Личный кабинет пользователя</returns>
        public async Task<IActionResult> PersonalCabinet()
        {
            var (userId, login, password) = Cookies();

            // Если куки пустые, то перенаправляет на начальную страницу
            if (userId == null && string.IsNullOrEmpty(login) && string.IsNullOrEmpty(password))
                return Redirect("/");

            var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == userId && x.Login == login && x.Password == password); // Поиск клиента по данным из куки
            var staff = new Staff();

            // Если клиент не найден, то ищем сотрудника
            if (client == null)
                staff = await _context.Staff.Include(x => x.Role)
                                            .FirstOrDefaultAsync(x => x.Id == userId && x.Login == login && x.Password == password);

            return View((client != null) ? client : staff);
        }

        /// <summary>
        /// Метод отображения ремонтов пользователя
        /// </summary>
        /// <returns>Список ремонтов конкретного пользователя</returns>
        public async Task<IActionResult> GetRepairs()
        {
            var (userId, login, password) = Cookies();

            // Проверка роли пользователя
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultStaff = await _roleCheckerService.Check(Request, "staff", "сотрудник");
            var resultClient = await _roleCheckerService.Check(Request, "client", "клиент");

            // Если пользователь не админ, не сотрудник и не клиент, то происходит перенаправление на стартовую страницу
            if (resultAdmin is UnauthorizedResult && resultStaff is UnauthorizedResult && resultClient is UnauthorizedResult)
                return Redirect("/");

            var response = await _repairsService.GetFilteredByUser(userId, login, password);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        /// <summary>
        /// Метод для получения устройств, привязанных к пользователю
        /// </summary>
        /// <returns>Список устройств конкретного пользователя</returns>
        public async Task<IActionResult> GetDevices()
        {
            var (userId, login, password) = Cookies();

            var resultClient = await _roleCheckerService.Check(Request, "client", "клиент"); // Проверка роли пользователя

            // Если пользователь не клиент, то происходит перенаправление на стартовую страницу
            if (resultClient is UnauthorizedResult)
                return Redirect("/");

            var response = await _devicesService.GetFilteredByUser(userId, login, password);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        /// <summary>
        /// Метод для получения заказов запчастей конкретного пользователя
        /// </summary>
        /// <returns>Список заказов запчастей конкретного пользователя</returns>
        public async Task<IActionResult> GetOrderAccessories()
        {
            var (userId, login, password) = Cookies();

            var resultClient = await _roleCheckerService.Check(Request, "client", "клиент"); // Проверка роли пользователя

            // Если пользователь не клиент, то происходит перенаправление на стартовую страницу
            if (resultClient is UnauthorizedResult)
                return Redirect("/");

            var response = await _orderAccessoriesService.GetFilteredByUser(userId, login, password);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        /// <summary>
        /// Метод для получения данных из куки
        /// </summary>
        /// <returns>Код пользователя, логин и пароль</returns>
        private (int?, string?, string?) Cookies()
        {
            var userId = Request.Cookies["userId"];
            var login = Request.Cookies["login"];
            var password = Request.Cookies["password"];

            int? userIdInt = null;

            if (userId != null)
                userIdInt = int.Parse(userId);

            return (userIdInt, login, password);
        }
    }
}
