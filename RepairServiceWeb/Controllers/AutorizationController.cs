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

        public async Task<IActionResult> Enter(string login, string password)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => EF.Functions.Collate(x.Login, "SQL_Latin1_General_CP1_CS_AS") == login && EF.Functions.Collate(x.Password, "SQL_Latin1_General_CP1_CS_AS") == password);

            var staff = new Staff();

            if (client == null)
            {
                staff = await _context.Staff.FirstOrDefaultAsync(x => EF.Functions.Collate(x.Login, "SQL_Latin1_General_CP1_CS_AS") == login && EF.Functions.Collate(x.Password, "SQL_Latin1_General_CP1_CS_AS") == password);

                if (staff == null)
                    return BadRequest("Неверный логин или пароль");
            }

            return Ok(new
            {
                auth_key = JWTCreate(staff, client),

                permissions = client != null ? client.RoleId : staff.RoleId,

                userId = client != null ? client.Id : staff.Id,

                login = client != null ? client.Login : staff.Login,

                password = client != null ? client.Password : staff.Password
            });
        }

        private static string JWTCreate(Staff staffs, Client? clients = null)
        {
            List<Claim>? claims;

            if (clients != null)
                claims = [new Claim(ClaimTypes.Name, clients.Name)];
            else
                claims = [new Claim(ClaimTypes.Name, staffs.Name)];

            var jwt = new JwtSecurityToken(
                    issuer: AutorizationOptions.ISSUER,
                    audience: AutorizationOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromHours(10)),
                    signingCredentials: new SigningCredentials(AutorizationOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public async Task<JsonResult> GetRoleName(int? permissionId)
        {
            var response = await _rolesService.GetRoleName(permissionId);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
                return Json(new { success = true, data = response.Data });

            return Json(new { success = false });
        }
    }
}
