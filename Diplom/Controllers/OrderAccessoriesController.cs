using Azure;
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
        private readonly ApplicationDbContext _context;

        public OrderAccessoriesController(IOrderAccessoriesService orderAccessoriesService, ApplicationDbContext context)
        {
            _orderAccessoriesService = orderAccessoriesService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrderAccessories()
        {
            var response = await _orderAccessoriesService.GetAll();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return View(response.Data.ToList());

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderAccessories(int id)
        {
            var response = await _orderAccessoriesService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<JsonResult> GetOrderAccessoriesByName(string name)
        {
            var response = await _orderAccessoriesService.GetByName(name);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, highlightedName = name });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        [HttpGet]
        public async Task<JsonResult> GetFilteredOrderAccessories(string clientFullName = "", string accessoryName = "", int? count = null, DateOnly date = default, string status = "")
        {
            var response = await _orderAccessoriesService.GetFiltered(clientFullName, accessoryName, count, date, status);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        public async Task<IActionResult> DeleteOrderAccessories(int id)
        {
            var response = await _orderAccessoriesService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return RedirectToAction("GetAllOrderAccessories");

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditOrderAccessories(int id)
        {
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
        }
    }
}
