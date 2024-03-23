using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<IActionResult> GetAllSuppliers()
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");

            if (resultAdmin is UnauthorizedResult)
                if (resultReception is UnauthorizedResult)
                    return Redirect("/");

            var responce = await _suppliersService.GetAll();

            if (responce.StatusCode == Domain.Enum.StatusCode.OK)
                return View(responce.Data.ToList());

            return View("~/Views/Shared/Error.cshtml", $"{responce.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetSuppliers(int id)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");

            if (resultAdmin is UnauthorizedResult)
                return Redirect("/");

            var response = await _suppliersService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetSuppliersByName(string name)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");

            if (resultAdmin is UnauthorizedResult)
                if (resultReception is UnauthorizedResult)
                    return Redirect("/");

            var response = await _suppliersService.GetByName(name);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, highlightedName = name });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        [HttpGet]
        public async Task<IActionResult> GetFilteredSuppliers(string address = "")
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultReception = await _roleCheckerService.Check(Request, "reception", "ресепшен");

            if (resultAdmin is UnauthorizedResult)
                if (resultReception is UnauthorizedResult)
                    return Redirect("/");

            var response = await _suppliersService.GetFiltered(address);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        public async Task<IActionResult> DeleteSuppliers(int id)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");

            if (resultAdmin is UnauthorizedResult)
                return Redirect("/");

            var response = await _suppliersService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return RedirectToAction("GetAllSuppliers");

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditSuppliers(int id)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");

            if (resultAdmin is UnauthorizedResult)
                return Redirect("/");

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
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");

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
