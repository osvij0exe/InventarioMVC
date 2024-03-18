using InventarioMVC.Models.Models.Request;
using InventarioMVC.Models.Models.Response;
using InventarioMVC.Models.Models.Response.InfoResponse;
using InventarioMVC.Models.ViewModels;
using InventarioMVC.Services.Implementacion;
using InventarioMVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace InventarioMVC.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IProductoServices _ProductoServices;
        private readonly ITipoProductoServices _TipoProductoServices;

        public ProductoController(IProductoServices productoServices,
            ITipoProductoServices TipoProductoServices)
        {
            _ProductoServices = productoServices;
            _TipoProductoServices = TipoProductoServices;
        }

        public async Task<IActionResult> Index([FromQuery] string? nombreProducto, [FromQuery] string? clave,[FromQuery]int? TipoId)
        {
            var listaPorductos = await _TipoProductoServices.ListTipoProductosAsync();

            var response = await _ProductoServices.ListarProductosAsync(nombreProducto,clave,TipoId);
            ViewBag.Nombre = nombreProducto;
            ViewBag.Clave = clave;
            ViewBag.TipoId = TipoId;

            var model = new BuscarProductoViewModel()
            {
                NombreProducto = nombreProducto,
                Clave = clave,
                TipoProducto = listaPorductos.Data,
                TipoProductoSelecionado = TipoId,
                Productos = response.Data
            };


            //model.Productos = response.Data;

                return View(model);

        }

        public async Task<IActionResult> ProductoEliminados()
        {
            var response = await _ProductoServices.ProductosEliminados();

            if(response.Success)
            {
                return View(response.Data);
            }
            return View();
        }

        public async Task<IActionResult> Crear()
        {

            var model = new CrearProductoViewModel();

            var tipoPorductos = await _TipoProductoServices.ListTipoProductosAsync();

            model.TipoProductos = tipoPorductos.Data;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearProductoViewModel request)
        {
            await _ProductoServices.InsertAsync(new ProductoDTORequest()
            {
                NombreProducto = request.NombreProducto,
                Clave = request.Clave,
                TipoId = request.TipoProductoSelecionado,

            });
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editar(int id)
        {
            var entity = await _ProductoServices.FindProductoByIdAsync(id);
            var listaTipoProducto = await _TipoProductoServices.ListTipoProductosAsync();

            if(entity is null)
            {
                return RedirectToAction(nameof(Index));
            }
            var model = new CrearProductoViewModel()
            {
                ProductoId = entity.Data.ProductoId,
                NombreProducto = entity.Data.NombreProducto,
                Clave = entity.Data.Clave,
                TipoProductos = listaTipoProducto.Data,
                TipoProductoSelecionado = entity.Data.TipoProducto.TipoId
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(CrearProductoViewModel model)
        {
            await _ProductoServices.UpdateAsync(model.ProductoId,
                new ProductoDTORequest()
                {
                    NombreProducto = model.NombreProducto,
                    Clave = model.Clave,
                    TipoId = model.TipoProductoSelecionado
                });
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var entity = await _ProductoServices.FindProductoByIdAsync(id);
            if(entity is null)
            {
                return RedirectToAction(nameof(Index));
            }

            var model = new CrearProductoViewModel()
            {
                ProductoId = id,
            };
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(CrearProductoViewModel model)
        {
            await _ProductoServices.EliminarAsync(model.ProductoId);

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Reactivar(int id)
        {
            var entity = await _ProductoServices.FindProductoByIdAsync(id);
            if(entity is null)
            {
                return RedirectToAction(nameof(Index)); 
            }
            var model = new ProductoViewModel()
            {
                ProductoId = id
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Reactivar(ProductoViewModel model)
        {
            await _ProductoServices.ReactivarProductoAsync(model.ProductoId);

            return RedirectToAction(nameof(Index));
        }
    }
}
