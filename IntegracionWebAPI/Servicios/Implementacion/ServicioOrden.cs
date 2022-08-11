using Dapper;
using IntegracionWebAPI.Data;
using IntegracionWebAPI.Servicios.Interfaz;
using IntegracionWebAPI.Utiles;
using Microsoft.AspNetCore.Mvc;

namespace IntegracionWebAPI.Servicios.Implementacion
{
    public class ServicioOrden: IServicioOrden
    {
        private readonly DapperContext _db;
        private readonly ResultadoOrden _resultado;

        public ServicioOrden(DapperContext db, ResultadoOrden resultado)
        {
            _db = db;
            _resultado = resultado;
        }

        public async Task<ResultadoOrden> AgregarOrden(int idcliente)
        {
            var insertorden = "INSERT INTO Ordenes (IdCliente) VALUES (@idcliente)";
            var ultid = "SELECT MAX(Id) FROM ORDENES";

            using (var conexion = _db.SuperConexionNando())
            {
                try
                {
                    await conexion.ExecuteAsync(insertorden, new { idcliente = idcliente });
                    _resultado.orden.Id = await conexion.QuerySingleAsync<int>(ultid);
                    _resultado.ok = true;
                    _resultado.mensaje = "";
                    return _resultado;
                }
                
                catch (Exception ex)
                {
                    _resultado.ok = false;
                    _resultado.mensaje = ex.Message;
                    return _resultado;
                }
            }
        }
    }
}
