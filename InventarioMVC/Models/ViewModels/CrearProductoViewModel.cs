using InventarioMVC.Models.Models.Response;

namespace InventarioMVC.Models.ViewModels
{
    public class CrearProductoViewModel
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = default!;
        public string Clave { get; set; } = default!;
        public ICollection<TipoProductoDTOResponse> TipoProductos { get; set; } = default!;
        public int TipoProductoSelecionado { get; set; }
    }
}
