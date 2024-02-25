using Diplom.DAL;
using Diplom.Domain.Entity;
using Diplom.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Diplom.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly ISuppliersService _suppliersService;

        public SuppliersController(ISuppliersService suppliersService)
        {
            _suppliersService = suppliersService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSuppliers()
        {
            var responce = await _suppliersService.GetAll();

            if (responce.StatusCode == Domain.Enum.StatusCode.OK)
                return View(responce.Data.ToList());

            return View("Error", $"{responce.Description}");
        }
    }
}
