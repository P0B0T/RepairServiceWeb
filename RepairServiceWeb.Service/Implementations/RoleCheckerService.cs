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

        /// <summary>
        /// Метод для определения роли пользователя
        /// </summary>
        /// <param name="request"> - запрос</param>
        /// <param name="role"> - название роли</param>
        /// <param name="roleAlt"> - альтернативное название роли</param>
        /// <returns>Авторизован или неавторизован</returns>
        public async Task<StatusCodeResult> Check(HttpRequest request, string role, string roleAlt)
        {
            try
            {
                var permissionId = int.Parse(request.Cookies["permissions"]);

                var response = await _rolesService.GetRoleName(permissionId);

                string data = response.Data.ToLower();

                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                    if (!data.Contains(role) && !data.Contains(roleAlt))
                        return new UnauthorizedResult();

                return new OkResult();
            }
            catch
            {
                return new UnauthorizedResult();
            }
        }
    }
}
