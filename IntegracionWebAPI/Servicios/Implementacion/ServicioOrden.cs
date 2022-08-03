using Dapper;
using IntegracionWebAPI.Data;
using IntegracionWebAPI.Servicios.Interfaz;

namespace IntegracionWebAPI.Servicios.Implementacion
{
    public class ServicioOrden: IServicioOrden
    {
        private readonly DapperContext _db;

        public ServicioOrden(DapperContext db)
        {
            _db = db;
        }

        public async Task<int> AgregarOrden(int idcliente)
        {
            var insertorden = "INSERT INTO Ordenes (IdCliente) VALUES (@idcliente)";
            var ultid = "SELECT MAX(Id) FROM ORDENES";
            var orden = 0;

            using (var conexion = _db.SuperConexionNando())

            {
                await conexion.ExecuteAsync(insertorden, new { idcliente = idcliente });
                orden = await conexion.QuerySingleAsync<int>(ultid);
            }

            return orden;
        }
    }
}
