using InventarioMVC.Models.Models.Response;
using System.ComponentModel.DataAnnotations;

namespace InventarioMVC.Models.ViewModels
{
    public class BuscarProductoViewModel
    {
        public int ProductoId { get; set; }
        [Display(Name = "Buscar por Nombre del Producto")]
        public string? NombreProducto { get; set; } = default!;
        [Display(Name = "Buscar por clave del producto")]
        public string? Clave { get; set; } = default!;
        public ICollection<TipoProductoDTOResponse> TipoProducto { get; set; } = default!;
        public int? TipoProductoSelecionado { get; set; }
        public ICollection<ProductoDTOResponse>? Productos { get; set; }
    }
}
