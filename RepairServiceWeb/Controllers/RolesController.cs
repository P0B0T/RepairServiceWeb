using Microsoft.AspNetCore.Mvc;
using RepairServiceWeb.Domain.ViewModels;
using RepairServiceWeb.Service.Interfaces;

namespace RepairServiceWeb.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRolesService _rolesService;
        private readonly IRoleCheckerService _roleCheckerService;

        public RolesController(IRolesService rolesService, IRoleCheckerService roleCheckerService)
        {
            _rolesService = rolesService;
            _roleCheckerService = roleCheckerService;
        }

        private async Task<StatusCodeResult> CheckRole()
        {
            var permissionId = int.Parse(Request.Cookies["permissions"]);

            var responce = await _rolesService.GetRoleName(permissionId);

            string data = responce.Data.ToLower();

            if (responce.StatusCode == Domain.Enum.StatusCode.OK)
                if (!data.Contains("admin") && !data.Contains("админ") && !data.Contains("human resources") && !data.Contains("отдел кадров"))
                    return Unauthorized();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultHumanResourceDepartment = await _roleCheckerService.Check(Request, "human resources", "отдел кадров");

            if (resultAdmin is UnauthorizedResult)
                if (resultHumanResourceDepartment is UnauthorizedResult)
                    return Redirect("/");

            var response = await _rolesService.GetAll();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return View(response.Data.ToList());

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles(int id)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultHumanResourceDepartment = await _roleCheckerService.Check(Request, "human resources", "отдел кадров");

            if (resultAdmin is UnauthorizedResult)
                if (resultHumanResourceDepartment is UnauthorizedResult)
                    return Redirect("/");

            var response = await _rolesService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        public async Task<IActionResult> DeleteRoles(int id)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultHumanResourceDepartment = await _roleCheckerService.Check(Request, "human resources", "отдел кадров");

            if (resultAdmin is UnauthorizedResult)
                if (resultHumanResourceDepartment is UnauthorizedResult)
                    return Redirect("/");

            var response = await _rolesService.Delete(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return RedirectToAction("GetAllRoles");

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditRoles(int id)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultHumanResourceDepartment = await _roleCheckerService.Check(Request, "human resources", "отдел кадров");

            if (resultAdmin is UnauthorizedResult)
                if (resultHumanResourceDepartment is UnauthorizedResult)
                    return Redirect("/");

            if (id == 0)
                return PartialView();

            var response = await _rolesService.Get(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return PartialView(response.Data);

            return View("~/Views/Shared/Error.cshtml", $"{response.Description}");
        }

        [HttpPost]
        public async Task<IActionResult> AddOrEditRoles(RolesViewModel model)
        {
            var resultAdmin = await _roleCheckerService.Check(Request, "admin", "админ");
            var resultHumanResourceDepartment = await _roleCheckerService.Check(Request, "human resources", "отдел кадров");

            if (resultAdmin is UnauthorizedResult)
                if (resultHumanResourceDepartment is UnauthorizedResult)
                    return Redirect("/");

            if (!ModelState.IsValid)
                return View(model);

            if (model.Id == 0)
                await _rolesService.Create(model);
            else
                await _rolesService.Edit(model.Id, model);

            TempData["Successfully"] = "Успешно";

            return RedirectToAction("GetAllRoles");
        }
    }
}
