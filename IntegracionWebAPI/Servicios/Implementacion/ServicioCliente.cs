using Dapper;
using IntegracionWebAPI.Data;
using IntegracionWebAPI.Entidades;
using IntegracionWebAPI.Servicios.Interfaz;
using IntegracionWebAPI.Utiles;
using Microsoft.AspNetCore.Mvc;

namespace IntegracionWebAPI.Servicios.Implementacion
{
    public class ServicioCliente : IServicioCliente
    {
        private readonly DapperContext _db;
        private readonly Resultado _resultado;

        public ServicioCliente(DapperContext db, Resultado resultado)
        {
            _db = db;
            _resultado = resultado;
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
                try
                {
                    var cliente = await conexion.QueryFirstAsync<Cliente>(clientedni);

                    return cliente;
                }
                catch(Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task<Resultado> AgregarCliente(int DNI, string nombre)
        {
            var insertcliente = "INSERT INTO Clientes (NomApe, DNI, IdEstado) VALUES (@nombreq, @dniq, @estadoq)";

            using (var conexion = _db.SuperConexionNando())
            {
                try
                {
                    await conexion.ExecuteAsync(insertcliente, new { nombreq = nombre, dniq = DNI, estadoq = 1 });
                    _resultado.ok = true;
                    return _resultado ;
                }
                catch (Exception ex)
                {
                    _resultado.ok= false;
                    _resultado.mensaje = ex.Message;
                    return _resultado;
                }          
            }
        }

        public async Task<Resultado> CambiarEstadoCliente(int estado, int id)
        {
            var updatecliente = "UPDATE Clientes SET IdEstado = @estadoq WHERE Id = @idq";

            using (var conexion = _db.SuperConexionNando())
            {
                try
                {
                    await conexion.ExecuteAsync(updatecliente, new { estadoq = estado, idq = id });
                    _resultado.ok = true;
                    return _resultado;
                }

                catch(Exception ex)
                {
                    _resultado.ok = false;
                    _resultado.mensaje =ex.Message;
                    return _resultado;
                }
               
            }
        }
    }
}
