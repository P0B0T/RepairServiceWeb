using Diplom.DAL;
using Diplom.Domain.ViewModels;
using Diplom.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Diplom.Controllers
{
    public class OrderAccessoriesController : Controller
    {
        private readonly IOrderAccessoriesService _orderAccessoriesService;
        private readonly IRolesService _rolesService;
        private readonly ApplicationDbContext _context;

        List<string> statusList = new List<string>()
        {
            "Создан",
            "В сборке",
            "В пути",
            "Ожидает получения",
            "Получен"
        };

        public OrderAccessoriesController(IOrderAccessoriesService orderAccessoriesService, ApplicationDbContext context, IRolesService rolesService)
        {
            _orderAccessoriesService = orderAccessoriesService;
            _context = context;
            _rolesService = rolesService;
        }

        private async Task<StatusCodeResult> CheckRole()
        {
            var permissionId = int.Parse(Request.Cookies["permissions"]);

            var responce = await _rolesService.GetRoleName(permissionId);

            string data = responce.Data.ToLower();

            if (responce.StatusCode == Domain.Enum.StatusCode.OK)
                if (!data.Contains("admin") && !data.Contains("админ") && !data.Contains("ресепшен") && !data.Contains("reception"))
                    return Unauthorized();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrderAccessories()
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _orderAccessoriesService.GetAll();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return View(response.Data.ToList());

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderAccessories(int id)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _orderAccessoriesService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderAccessoriesByName(string name)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _orderAccessoriesService.GetByName(name);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, highlightedName = name });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        [HttpGet]
        public async Task<IActionResult> GetFilteredOrderAccessories(string clientFullName = "", string accessoryName = "", int? count = null, DateOnly date = default, string status = "")
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _orderAccessoriesService.GetFiltered(clientFullName, accessoryName, count, date, status);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        public async Task<IActionResult> DeleteOrderAccessories(int id)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _orderAccessoriesService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return RedirectToAction("GetAllOrderAccessories");

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditOrderAccessories(int id)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            FillViewBag();

            if (id == 0)
                return PartialView();

            var response = await _orderAccessoriesService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditOrderAccessories(OrderAccessoriesViewModel model)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
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

        private void FillViewBag()
        {
            ViewBag.ClientsList = new SelectList(_context.Clients.ToList(), "Id", "FullName");
            ViewBag.AcessoriesList = new SelectList(_context.Accessories.ToList(), "Id", "Name");
            ViewBag.StatusList = new SelectList(statusList);
        }
    }
}
