using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RepairServiceWeb.DAL;
using RepairServiceWeb.Domain.ViewModels;
using RepairServiceWeb.Service.Interfaces;

namespace RepairServiceWeb.Controllers
{
    public class AccessoriesController : Controller
    {
        private readonly IAccessoriesService _accessoriesService;
        private readonly IRolesService _rolesService;
        private readonly ApplicationDbContext _context;

        public AccessoriesController(IAccessoriesService accessoriesService, ApplicationDbContext context, IRolesService rolesService)
        {
            _accessoriesService = accessoriesService;
            _context = context;
            _rolesService = rolesService;
        }

        private async Task<StatusCodeResult> CheckRole()
        {
            var permissionId = int.Parse(Request.Cookies["permissions"]);

            var responce = await _rolesService.GetRoleName(permissionId);

            string data = responce.Data.ToLower();

            if (responce.StatusCode == Domain.Enum.StatusCode.OK)
                if (!data.Contains("admin") && !data.Contains("админ") && !data.Contains("ресепшен") && !data.Contains("reception") && !data.Contains("сотрудник") && !data.Contains("staff"))
                    return Unauthorized();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccessories()
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _accessoriesService.GetAll();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return View(response.Data.ToList());

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetAccessories(int id)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _accessoriesService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetAccessoriesByName(string name)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _accessoriesService.GetByName(name);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, highlightedName = name });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        [HttpGet]
        public async Task<IActionResult> GetFilteredAccessories(string Name = "", string manufacturer = "", string supplier = "")
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _accessoriesService.GetFiltered(Name, manufacturer, supplier);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, filteredData = response.Data });

            return Json(new { success = false, error = $"{response.Description}" });
        }

        public async Task<IActionResult> DeleteAccessories(int id)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

            var response = await _accessoriesService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return RedirectToAction("GetAllAccessories");

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }


        [HttpGet]
        public async Task<IActionResult> AddOrEditAccessories(int id)
        {
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

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
            var result = await CheckRole();
            if (result is UnauthorizedResult)
                return Redirect("/");

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