using InventarioMVC.Models.Models.Response;

namespace InventarioMVC.Models.ViewModels
{
    public class ProductoViewModel
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = default!;
        public string Clave { get; set; } = default!;
        public int TipoId { get; set; } = default!;
        public string Tipo { get; set; } = default!;
        public ICollection<ProductoDTOResponse> prdouctos { get; set; } = default!;


    }
}
