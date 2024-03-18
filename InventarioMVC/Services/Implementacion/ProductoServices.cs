using InventarioMVC.ConextionString;
using InventarioMVC.Models.Models.Request;
using InventarioMVC.Models.Models.Response;
using InventarioMVC.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace InventarioMVC.Services.Implementacion
{
    public class ProductoServices : IProductoServices
    {
        private readonly ConectionSettings _Conexion;
        private readonly ILogger<ProductoServices> _Logger;

        public ProductoServices(IOptions<ConectionSettings> conexion,
            ILogger<ProductoServices> logger)
        {
            _Conexion = conexion.Value;
            _Logger = logger;
        }

        public async Task<BaseResponse> EliminarAsync(int id)
        {
            var response = new BaseResponse();

            using (var conection = new SqlConnection(_Conexion.InventarioDB))
            {
                await conection.OpenAsync();

                try
                {

                    var SQLResponse = 0;
                    var existeProducto = await FindProductoByIdAsync(id);

                    if (existeProducto is not null)
                    {
                        SqlCommand cmd = new SqlCommand("upsDeleteProducto", conection);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProductoId", id);
                        SQLResponse = await cmd.ExecuteNonQueryAsync();

                        if (SQLResponse == 0)
                        {
                            throw new InvalidOperationException("no se encontro ningun registor");
                        }

                        response.Success = true;

                    }
                    else
                    {
                        throw new InvalidOperationException("No existe registro con ese id");
                    }
                }
                catch (Exception ex)
                {

                    response.ErrorMessage = "Error al eliminar producto";
                    _Logger.LogCritical(ex, "{ErrorMessage}{Message}", response.ErrorMessage, ex.Message);
                    await conection.CloseAsync();
                }
                return response;
            }
        }

        public async Task<BaseResponseGeneric<ProductoDTOResponse>> FindProductoByIdAsync(int id)
        {
            var response = new BaseResponseGeneric<ProductoDTOResponse>();
            var producto = new ProductoDTOResponse();

            using (var conection = new SqlConnection(_Conexion.InventarioDB))
            {
                await conection.OpenAsync();
                try
                {
                    SqlCommand cmd = new SqlCommand("uspFindById", conection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductoId", id);

                    using (var reader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult))
                    {
                        int ProductoIdPosicion = reader.GetOrdinal("ProductoId");
                        int NombreProductoPosicion = reader.GetOrdinal("NombreProducto");
                        int ClavePosicion = reader.GetOrdinal("Clave");
                        int TipoProductoIdPosicion = reader.GetOrdinal("TipoId");
                        int TipoPosicion = reader.GetOrdinal("Tipo");

                        while (await reader.ReadAsync())
                        {
                            producto = new ProductoDTOResponse()
                            {
                                ProductoId = reader.IsDBNull(ProductoIdPosicion) ? 0 : reader.GetInt32(ProductoIdPosicion),
                                NombreProducto = reader.IsDBNull(NombreProductoPosicion) ? "" : reader.GetString(NombreProductoPosicion),
                                Clave = reader.IsDBNull(ClavePosicion) ? "" : reader.GetString(ClavePosicion),
                                TipoProducto = new TipoProductoDTOResponse()
                                {
                                    TipoId = reader.IsDBNull(TipoProductoIdPosicion) ? 0 : reader.GetInt32(TipoProductoIdPosicion),
                                    Tipo = reader.IsDBNull(TipoPosicion) ? "" : reader.GetString(TipoPosicion),
                                }
                            };
                        }
                        if (producto is null || producto.ProductoId == 0)
                        {
                            throw new InvalidOperationException("No se encontro Producto");
                        }
                        response.Data = producto;
                        response.Success = true;
                        await reader.CloseAsync();
                    }
                }
                catch (Exception ex)
                {

                    response.ErrorMessage = "Error al buscar producto por Id";
                    _Logger.LogCritical(ex, "{ErrorMessage}{Message}", response.ErrorMessage, ex.Message);
                    await conection.CloseAsync();
                }
                return response;


            }
        }

        public async Task<BaseResponse> InsertAsync(ProductoDTORequest request)
        {
            var response = new BaseResponse();

            using(var conection = new SqlConnection(_Conexion.InventarioDB))
            {
                await conection.OpenAsync();

                var SQLResposne = 0;
                try
                {
                    SqlCommand cmd = new SqlCommand("uspInsertarProducto", conection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NombreProducto", request.NombreProducto);
                    cmd.Parameters.AddWithValue("@Clave", request.Clave);
                    cmd.Parameters.AddWithValue("@TipoId", request.TipoId);
                    SQLResposne = await cmd.ExecuteNonQueryAsync();

                    if(SQLResposne != 0)
                    {
                        response.Success = true;
                    }
                    else
                    {
                        throw new InvalidOperationException("error");
                    }


                }
                catch (Exception ex)
                {

                    response.ErrorMessage = "Erorr al iunsertar Porducto";
                    _Logger.LogCritical(ex, "{ErrorMessage}{Message}", response.ErrorMessage, ex.Message);
                    await conection.CloseAsync();
                }
                return response;
            }

        }

        public async Task<BaseResponseGeneric<List<ProductoDTOResponse>>> ListarProductosAsync(string? NombrePrdocuto, string? clave, int? TipoId)
        {
            var response = new BaseResponseGeneric<List<ProductoDTOResponse>>();

            var listaIProductos = new List<ProductoDTOResponse>();

            using(var coneccion = new SqlConnection(_Conexion.InventarioDB))
            {
                await coneccion.OpenAsync();


                try
                {
                    SqlCommand cmd = new SqlCommand("uspListarProductos", coneccion);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NombreProducto", NombrePrdocuto);
                    cmd.Parameters.AddWithValue("@Clave", clave);
                    cmd.Parameters.AddWithValue("@TipoId", TipoId);


                    using ( var reader = await cmd.ExecuteReaderAsync())
                    {
                        int ProductoIdPosicion = reader.GetOrdinal("ProductoId");
                        int NombreProductoPosicion = reader.GetOrdinal("NombreProducto");
                        int ClavePosicion = reader.GetOrdinal("Clave");
                        int TipoProductoIdPosicion = reader.GetOrdinal("TipoId");
                        int TipoPosicion = reader.GetOrdinal("Tipo");

                        while(await reader.ReadAsync())
                        {
                            listaIProductos.Add(new ProductoDTOResponse()
                            {
                                ProductoId = reader.IsDBNull(ProductoIdPosicion) ? 0 : reader.GetInt32(ProductoIdPosicion),
                                NombreProducto = reader.IsDBNull(NombreProductoPosicion) ? "" : reader.GetString(NombreProductoPosicion),
                                Clave = reader.IsDBNull(ClavePosicion) ? "" : reader.GetString(ClavePosicion),
                                TipoProducto = new TipoProductoDTOResponse()
                                {
                                    TipoId = reader.IsDBNull(TipoProductoIdPosicion) ? 0 : reader.GetInt32(TipoProductoIdPosicion),
                                    Tipo = reader.IsDBNull(TipoPosicion) ? "" : reader.GetString(TipoPosicion),
                                }
                            });
                        }
                        response.Data = listaIProductos;
                        response.Success = true;
                        await reader.CloseAsync();
                    }


                }
                catch (Exception ex)
                {

                    response.ErrorMessage = "Error al listar Productos";
                    _Logger.LogCritical(ex, "{ErrorMessage}{message}", response.ErrorMessage, ex.Message);
                    await coneccion.CloseAsync();

                }
                return response;
            }

        }

        public async Task<BaseResponseGeneric<List<ProductoDTOResponse>>> ProductosEliminados()
        {
            var response = new BaseResponseGeneric<List<ProductoDTOResponse>>();
            var Listaproductos = new List<ProductoDTOResponse>();

            using (var conection = new SqlConnection(_Conexion.InventarioDB))
            {
                await conection.OpenAsync();
                try
                {

                    SqlCommand cmd = new SqlCommand("upsListarEliminadosProducto", conection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = await cmd.ExecuteReaderAsync(System.Data.CommandBehavior.SingleResult))
                    {
                        int ProductoIdPosicion = reader.GetOrdinal("ProductoId");
                        int NombreProductoPosicion = reader.GetOrdinal("NombreProducto");
                        int ClavePosicion = reader.GetOrdinal("Clave");
                        int TipoProductoIdPosicion = reader.GetOrdinal("TipoId");
                        int TipoPosicion = reader.GetOrdinal("Tipo");

                        while (await reader.ReadAsync())
                        {
                            Listaproductos.Add(new ProductoDTOResponse()
                            {
                                ProductoId = reader.IsDBNull(ProductoIdPosicion) ? 0 : reader.GetInt32(ProductoIdPosicion),
                                NombreProducto = reader.IsDBNull(NombreProductoPosicion) ? "" : reader.GetString(NombreProductoPosicion),
                                Clave = reader.IsDBNull(ClavePosicion) ? "" : reader.GetString(ClavePosicion),
                                TipoProducto = new TipoProductoDTOResponse()
                                {
                                    TipoId = reader.IsDBNull(TipoProductoIdPosicion) ? 0 : reader.GetInt32(TipoProductoIdPosicion),
                                    Tipo = reader.IsDBNull(TipoPosicion) ? "" : reader.GetString(TipoPosicion),
                                }
                            });
                        }
                        response.Data = Listaproductos;
                        response.Success = true;
                        await reader.CloseAsync();


                    }

                }
                catch (Exception ex)
                {

                    response.ErrorMessage = "Error al listar productos eliminados";
                    _Logger.LogCritical(ex, "{ErrorMessge}{Message}", response.ErrorMessage, ex.Message);
                    await conection.CloseAsync();
                }
                return response;
            }

        }

        public async Task<BaseResponse> ReactivarProductoAsync(int id)
        {
            var response = new BaseResponse();

            var productoExiste = await FindProductoByIdAsync(id);

            using (var conection = new SqlConnection(_Conexion.InventarioDB))
            {
                await conection.OpenAsync();

                try
                {
                    var SQLResponse = 0;

                    if(productoExiste is not null)
                    {
                        SqlCommand cmd = new SqlCommand("upsReactivarProducto", conection);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProductoId", id);
                        SQLResponse = await cmd.ExecuteNonQueryAsync();

                        if(SQLResponse == 0)
                        {
                            throw new InvalidOperationException("No se encontroo ningun Producto con ese Id");
                        }
                         response.Success = true;

                    }
                    else
                    {
                        throw new InvalidOperationException("No existe ningun Producto con ese Id");
                    }


                }
                catch (Exception ex)
                {

                    response.ErrorMessage = "Error al reactivar Producto";
                    _Logger.LogCritical(ex, "{ErrorMessage}{Message}", response.ErrorMessage, ex.Message);
                    await conection.CloseAsync();
                }
                return response;
            }


        }

        public async Task<BaseResponse> UpdateAsync(int id, ProductoDTORequest request)
        {
            var response = new BaseResponse();

            using(var conection = new SqlConnection(_Conexion.InventarioDB))
            {
                await conection.OpenAsync();
                try
                {
                    var SQLResponse = 0;
                    var existeProducto = await FindProductoByIdAsync(id);

                    if (existeProducto != null)
                    {
                        SqlCommand cmd = new SqlCommand("uspUpdateProducto", conection);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProductoId", id);
                        cmd.Parameters.AddWithValue("@NombreProducto", request.NombreProducto);
                        cmd.Parameters.AddWithValue("@Clave", request.Clave);
                        cmd.Parameters.AddWithValue("@TipoId", request.TipoId);
                        SQLResponse = await cmd.ExecuteNonQueryAsync();
                        
                        if (SQLResponse != 0)
                        {
                            response.Success = existeProducto != null;
                        }
                        else
                        {
                            throw new InvalidOperationException("No se en otnro registro");
                        }
                    }
                }
                catch (Exception ex)
                {

                    response.ErrorMessage = "Errror al actualizar producto";
                    _Logger.LogCritical(ex, "{ErrorMessage}{Message}", response.ErrorMessage, ex.Message);
                    await conection.CloseAsync();
                }
                return response;

            }
        }
    }
}
