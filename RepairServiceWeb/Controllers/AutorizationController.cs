using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RepairServiceWeb.DAL;
using RepairServiceWeb.Domain.Entity;
using RepairServiceWeb.Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RepairServiceWeb.Controllers
{
    public class AutorizationController : Controller
    {
        private readonly IRolesService _rolesService;
        private readonly ApplicationDbContext _context;

        public AutorizationController(ApplicationDbContext context, IRolesService rolesService)
        {
            _context = context;
            _rolesService = rolesService;
        }

        public IActionResult Index() => View();

        /// <summary>
        /// Метод для авторизации
        /// </summary>
        /// <param name="login" - логин пользователя></param>
        /// <param name="password" - пароль></param>
        /// <returns>Данные для авторизации</returns>
        public async Task<IActionResult> Enter(string login, string password)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => EF.Functions.Collate(x.Login, "SQL_Latin1_General_CP1_CS_AS") == login && EF.Functions.Collate(x.Password, "SQL_Latin1_General_CP1_CS_AS") == password); // Поиск клиента по логину и паролю

            var staff = new Staff();

            // Если клмент не найден, ищет сотрудника
            if (client == null)
            {
                staff = await _context.Staff.FirstOrDefaultAsync(x => EF.Functions.Collate(x.Login, "SQL_Latin1_General_CP1_CS_AS") == login && EF.Functions.Collate(x.Password, "SQL_Latin1_General_CP1_CS_AS") == password);

                // Если сотрудник не найден, то возвращает BadRequest
                if (staff == null)
                    return BadRequest("Неверный логин или пароль");
            }

            // Возвращает данные для авторизации
            return Ok(new
            {
                auth_key = JWTCreate(staff, client), // JWT

                permissions = client != null ? client.RoleId : staff.RoleId, // Права

                userId = client != null ? client.Id : staff.Id, // Id пользователя

                login = client != null ? client.Login : staff.Login, // Логин

                password = client != null ? client.Password : staff.Password // Пароль
            });
        }

        /// <summary>
        /// Метод для создания JWT
        /// </summary>
        /// <param name="staff" - сотрудник></param>
        /// <param name="clients" - клиент></param>
        /// <returns>JWT</returns>
        private static string JWTCreate(Staff staff, Client? clients = null)
        {
            List<Claim>? claims;

            // Создание утверждения
            if (clients != null)
                claims = [new Claim(ClaimTypes.Name, clients.Name)];
            else
                claims = [new Claim(ClaimTypes.Name, staff.Name)];

            // Создание JWT
            var jwt = new JwtSecurityToken(
                    issuer: AutorizationOptions.ISSUER,
                    audience: AutorizationOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromHours(10)),
                    signingCredentials: new SigningCredentials(AutorizationOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        /// <summary>
        /// Метод для получения названия роли по коду
        /// </summary>
        /// <param name="permissionId" - код роли></param>
        /// <returns>Json ответ с названием роли</returns>
        public async Task<JsonResult> GetRoleName(int? permissionId)
        {
            var response = await _rolesService.GetRoleName(permissionId);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, data = response.Data });

            return Json(new { success = false });
        }
    }
}
