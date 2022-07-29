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

        public async Task<List<Cliente>> ListarClientes()
        {
            var queryListaClientes = "SELECT * FROM Clientes";

            using (var conexion = _db.SuperConexionNando())

            {
                var listaClientes = (await conexion.QueryAsync<Cliente>(queryListaClientes)).ToList();
                return listaClientes;
            }
        }

        public async Task<Cliente> ClientePorDNI(int DNI)
        {
            var clientedni = "SELECT * FROM Clientes WHERE DNI =" + DNI;

            using (var conexion = _db.SuperConexionNando())

            {
                var cliente = await conexion.QueryFirstAsync<Cliente>(clientedni);

                return cliente;
            }
        }

        public void AgregarCliente(int DNI, string nombre)
        {
            var insertcliente = "INSERT INTO Clientes (NomApe, DNI, IdEstado) VALUES (@nombreq, @dniq, @estadoq)";

            using (var conexion = _db.SuperConexionNando())

            {
                conexion.Execute(insertcliente, new { nombreq = nombre, dniq = DNI, estadoq = 1 });
            }
        }

        public void CambiarEstadoCliente(int estado, int id)
        {
            var updatecliente = "UPDATE Clientes SET IdEstado = @estadoq WHERE Id = @idq";

            using (var conexion = _db.SuperConexionNando())

            {
                conexion.Execute(updatecliente, new { estadoq = estado, idq = id });
            }
        }
    }
}
