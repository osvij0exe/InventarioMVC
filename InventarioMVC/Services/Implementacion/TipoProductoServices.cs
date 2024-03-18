using InventarioMVC.ConextionString;
using InventarioMVC.Models.Models.Response;
using InventarioMVC.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace InventarioMVC.Services.Implementacion
{
    public class TipoProductoServices : ITipoProductoServices
    {
        private readonly ConectionSettings _Conexion;
        private readonly ILogger<TipoProductoServices> _Logger;

        public TipoProductoServices(IOptions<ConectionSettings> conexion,
            ILogger<TipoProductoServices> logger)
        {
            _Conexion = conexion.Value;
            _Logger = logger;
        }

        public async Task<BaseResponseGeneric<List<TipoProductoDTOResponse>>> ListTipoProductosAsync()
        {
            var resposne = new BaseResponseGeneric<List<TipoProductoDTOResponse>>();

            var tipos = new List<TipoProductoDTOResponse>();

            using (var connection = new SqlConnection(_Conexion.InventarioDB))
            {
                await connection.OpenAsync();

                try
                {
                    SqlCommand cmd = new SqlCommand("ListarTipoProducto", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        int TipoIdPosition = reader.GetOrdinal("TipoId");
                        int TipoPosition = reader.GetOrdinal("Tipo");


                        while (await reader.ReadAsync())
                        {
                            tipos.Add(new TipoProductoDTOResponse()
                            {
                                TipoId = reader.IsDBNull(TipoIdPosition) ? 0 : reader.GetInt32(TipoIdPosition),
                                Tipo = reader.IsDBNull(TipoPosition) ? "" : reader.GetString(TipoPosition)
                            });
                        }

                        resposne.Data = tipos;
                        resposne.Success = true;
                        await reader.CloseAsync();
                    }
                }
                catch (Exception ex)
                {

                    resposne.ErrorMessage = "Error al listar los tipos de productos";
                    _Logger.LogCritical(ex, "{ErrorMessage}{Message}", resposne.ErrorMessage, ex.Message);
                    await connection.CloseAsync();
                }
                return resposne;

            }


        }
    }
}


