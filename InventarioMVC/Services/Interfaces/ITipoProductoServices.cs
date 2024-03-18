using InventarioMVC.Models.Models.Response;
using System.ComponentModel;

namespace InventarioMVC.Services.Interfaces
{
    public interface ITipoProductoServices
    {

        Task<BaseResponseGeneric<List<TipoProductoDTOResponse>>> ListTipoProductosAsync();

    }
}
