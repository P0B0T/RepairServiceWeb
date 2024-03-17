using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepairServiceWeb.DAL;
using RepairServiceWeb.Domain.Entity;

namespace RepairServiceWeb.Controllers
{
    public class PersonalCabinetController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonalCabinetController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> PersonalCabinet(int? userId, string login = "", string password = "")
        {
            if (userId == null && login == "" && password == "")
                return Redirect("/");

            var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == userId && x.Login == login && x.Password == password);
            var staff = new Staff();

            if (client == null)
                staff = await _context.Staff.Include(x => x.Role)
                                            .FirstOrDefaultAsync(x => x.Id == userId && x.Login == login && x.Password == password);

            return View((client != null) ? client : staff);
        }

        //public async Task<IActionResult> GetRepairs(int? userId)
        //{

        //}
    }
}
