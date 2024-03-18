namespace InventarioMVC.Models.Models.Response
{
    public class ProductoDTOResponse
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = default!;
        public string Clave { get; set; } = default!;
        public TipoProductoDTOResponse TipoProducto { get; set; } = default!;
    }
}
