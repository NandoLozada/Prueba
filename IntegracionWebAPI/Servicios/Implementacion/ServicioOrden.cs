using Dapper;
using IntegracionWebAPI.Data;

namespace IntegracionWebAPI.Servicios.Implementacion
{
    public class ServicioOrden
    {
        private readonly DapperContext _db;

        public ServicioOrden(DapperContext db)
        {
            _db = db;
        }

        public int AgregarOrden(int idcliente)
        {
            var insertorden = "INSERT INTO Ordenes (IdCliente) VALUES (@idcliente)";
            var ultid = "SELECT MAX(Id) FROM ORDENES";
            var orden = 0;

            using (var conexion = _db.SuperConexionNando())

            {
                conexion.Execute(insertorden, new { idcliente = idcliente });
                orden = conexion.QuerySingle<int>(ultid);
            }

            return orden;
        }
    }
}
