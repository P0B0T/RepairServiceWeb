using Diplom.DAL;
using Diplom.Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;

namespace Diplom.Controllers
{
    public class AutorizationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AutorizationController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Enter(string login, string password)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.Login == login && x.Password == password);

            var staff = new Staff();

            if(client == null)
            {
                staff = await _context.Staff.FirstOrDefaultAsync(x => x.Login == login && x.Password == password);

                if (staff == null)
                    return BadRequest("�������� ����� ��� ������");
            }

            return Ok(new
            {
                auth_key = JWTCreate(staff, client),

                permissions = (client != null) ? client.RoleId : staff.RoleId,

                userId = (client != null) ? client.Id : staff.Id
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

        public async Task<IActionResult> PersonalCabinet(int userId)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == userId);

            var staff = new Staff();

            if (client == null)
                staff = await _context.Staff.Include(x => x.Role)
                                            .FirstOrDefaultAsync(x => x.Id == userId);

            return View((client != null) ? client : staff);
        }
    }
}