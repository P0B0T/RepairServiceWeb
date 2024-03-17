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
        private readonly IRolesService _rolesService;

        public SuppliersController(ISuppliersService suppliersService, IRolesService rolesService)
        {
            _suppliersService = suppliersService;
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
        public async Task<IActionResult> GetAllSuppliers()
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var responce = await _suppliersService.GetAll();

            if (responce.StatusCode == Domain.Enum.StatusCode.OK)
                return View(responce.Data.ToList());

            return View("~/Views/Shared/Error.cshtml", $"{responce.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetSuppliers(int id)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _suppliersService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetSuppliersByName(string name)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _suppliersService.GetByName(name);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, highlightedName = name });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        [HttpGet]
        public async Task<IActionResult> GetFilteredSuppliers(string address = "")
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _suppliersService.GetFiltered(address);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        public async Task<IActionResult> DeleteSuppliers(int id)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _suppliersService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return RedirectToAction("GetAllSuppliers");

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditSuppliers(int id)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
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
            var result = await CheckRole();
            if (result is UnauthorizedResult)
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
