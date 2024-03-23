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

        public async Task<IActionResult> PersonalCabinet(int? userId, string login = "", string password = "")
        {
            if (userId == null && login == "" && password == "")
                return Redirect("/");

            var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == userId && x.Login == login && x.Password == password);
            var staff = new Staff();

            if (client == null)
                staff = await _context.Staff.Include(x => x.Role)
                                            .FirstOrDefaultAsync(x => x.Id == userId && x.Login == login && x.Password == password);

            return View((client != null) ? client : staff);
        }

        public async Task<IActionResult> GetRepairs(int? userId, string login = "", string password = "")
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultStaff = await _roleCheckerService.Check(Request, "staff", "сотрудник");
            var resultClient = await _roleCheckerService.Check(Request, "client", "клиент");

            if (resultAdmin is UnauthorizedResult)
                if (resultStaff is UnauthorizedResult)
                    if (resultClient is UnauthorizedResult)
                        return Redirect("/");

            var response = await _repairsService.GetFilteredByUser(userId, login, password);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        public async Task<IActionResult> GetDevices(int? userId, string login = "", string password = "")
        {
            var resultClient = await _roleCheckerService.Check(Request, "client", "клиент");

            if (resultClient is UnauthorizedResult)
                return Redirect("/");

            var response = await _devicesService.GetFilteredByUser(userId, login, password);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        public async Task<IActionResult> GetOrderAccessories(int? userId, string login = "", string password = "")
        {
            var resultClient = await _roleCheckerService.Check(Request, "client", "клиент");

            if (resultClient is UnauthorizedResult)
                return Redirect("/");

            var response = await _orderAccessoriesService.GetFilteredByUser(userId, login, password);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }
    }
}
