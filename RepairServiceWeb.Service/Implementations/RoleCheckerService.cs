using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepairServiceWeb.Service.Interfaces;

namespace RepairServiceWeb.Service.Implementations
{
    public class RoleCheckerService : IRoleCheckerService
    {
        private readonly IRolesService _rolesService;

        public RoleCheckerService(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        public async Task<StatusCodeResult> Check(HttpRequest request, string role, string roleAlt)
        {
            var permissionId = int.Parse(request.Cookies["permissions"]);

            var response = await _rolesService.GetRoleName(permissionId);

            string data = response.Data.ToLower();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                if (!data.Contains(role) && !data.Contains(roleAlt))
                    return new UnauthorizedResult();

            return new OkResult();
        }
    }
}
