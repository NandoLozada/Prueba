using Dapper;
using IntegracionWebAPI.Data;
using IntegracionWebAPI.Entidades;
using IntegracionWebAPI.Servicios.Interfaz;

namespace IntegracionWebAPI.Servicios.Implementacion
{
    public class ServicioCliente : IServicioCliente
    {
        private readonly DapperContext _db;
        public ServicioCliente(DapperContext db)
        {
            _db = db;
        }

        public async Task<List<Cliente>> ListarClients()
        {
            var queryListaClientes = "SELECT * FROM Clientes";

            using (var conexion = _db.SuperConexionNando())

            {
                var listaClientes = (await conexion.QueryAsync<Cliente>(queryListaClientes)).ToList();
                return listaClientes;
            }
        }
    }
}
