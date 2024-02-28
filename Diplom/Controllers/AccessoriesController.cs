using Diplom.DAL;
using Diplom.Domain.ViewModels;
using Diplom.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Diplom.Controllers
{
    public class AccessoriesController : Controller
    {
        private readonly IAccessoriesService _accessoriesService;
        private readonly ApplicationDbContext _context;

        public AccessoriesController(IAccessoriesService accessoriesService, ApplicationDbContext context)
        {
            _accessoriesService = accessoriesService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccessories()
        {
            var response = await _accessoriesService.GetAll();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return View(response.Data.ToList());

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetAccessories(int id)
        {
            var response = await _accessoriesService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<JsonResult> GetAccessoriesByName(string name)
        {
            var response = await _accessoriesService.GetByName(name);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, highlightedName = name });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        [HttpGet]
        public async Task<JsonResult> GetFilteredAccessories(string Name = "", string manufacturer = "", string supplier = "")
        {
            var response = await _accessoriesService.GetFiltered(Name, manufacturer, supplier);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        public async Task<IActionResult> DeleteAccessories(int id)
        {
            var response = await _accessoriesService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return RedirectToAction("GetAllAccessories");

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }


        [HttpGet]
        public async Task<IActionResult> AddOrEditAccessories(int id)
        {
            ViewBag.SuppliersList = new SelectList(_context.Suppliers.ToList(), "Id", "CompanyName");

            if (id == 0)
                return PartialView();

            var response = await _accessoriesService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditAccessories(AccessoriesViewModel model)
        {
            ViewBag.SuppliersList = new SelectList(_context.Suppliers.ToList(), "Id", "CompanyName");

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