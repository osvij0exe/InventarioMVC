namespace InventarioMVC.Models.Models.Request
{
    public class ProductoDTORequest
    {
        public string NombreProducto { get; set; } = default!;
        public string Clave { get; set; } = default!;
        public int TipoId { get; set; }

    }
}
