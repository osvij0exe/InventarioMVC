using InventarioMVC.Models.Models.Response;
using InventarioMVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventarioMVC.Controllers
{
    public class TipoProductoController : Controller
    {
        private readonly ITipoProductoServices _Services;

        public TipoProductoController(ITipoProductoServices services)
        {
            _Services = services;
        }


        public async Task<IActionResult> Index()
        {
            var response = await _Services.ListTipoProductosAsync();


            return View(response.Data);
        }
    }
}
