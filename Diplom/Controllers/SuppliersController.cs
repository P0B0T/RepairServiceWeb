using Diplom.DAL;
using Diplom.Domain.Entity;
using Diplom.Domain.ViewModels;
using Diplom.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Diplom.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly ISuppliersService _suppliersService;

        public SuppliersController(ISuppliersService suppliersService)
        {
            _suppliersService = suppliersService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSuppliers()
        {
            var responce = await _suppliersService.GetAll();

            if (responce.StatusCode == Domain.Enum.StatusCode.OK)
                return View(responce.Data.ToList());

            return View("~/Views/Shared/Error.cshtml", $"{responce.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetSuppliers(int id)
        {
            var response = await _suppliersService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<JsonResult> GetSuppliersByName(string name)
        {
            var response = await _suppliersService.GetByName(name);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, highlightedName = name });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        [HttpGet]
        public async Task<JsonResult> GetFilteredSuppliers(string address = "")
        {
            var response = await _suppliersService.GetFiltered(address);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        public async Task<IActionResult> DeleteSuppliers(int id)
        {
            var response = await _suppliersService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return RedirectToAction("GetAllSuppliers");

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditSuppliers(int id)
        {
            if (id == 0)
                return PartialView();

            var response = await _suppliersService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditSuppliers(SuppliersViewModel model)
        {
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
