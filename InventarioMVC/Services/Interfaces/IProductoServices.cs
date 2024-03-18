using InventarioMVC.Models.Models.Request;
using InventarioMVC.Models.Models.Response;
using InventarioMVC.Models.Models.Response.InfoResponse;

namespace InventarioMVC.Services.Interfaces
{
    public interface IProductoServices
    {
        Task<BaseResponseGeneric<List<ProductoDTOResponse>>>ListarProductosAsync(string? nombrePrdocuto, string? clave,int? TipoId);
        Task<BaseResponse> InsertAsync(ProductoDTORequest request);
        Task<BaseResponse> UpdateAsync(int id, ProductoDTORequest request);
        Task<BaseResponseGeneric<ProductoDTOResponse>> FindProductoByIdAsync(int id);
        Task<BaseResponse> EliminarAsync(int id);
        Task<BaseResponseGeneric<List<ProductoDTOResponse>>> ProductosEliminados();
        Task<BaseResponse> ReactivarProductoAsync(int id);


    }
}
