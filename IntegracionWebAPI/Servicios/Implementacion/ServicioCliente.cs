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
        private readonly ResultadoCliente _resultado;

        public ServicioCliente(DapperContext db, ResultadoCliente resultado)
        {
            _db = db;
            _resultado = resultado;
        }

        public async Task<ResultadoCliente> ListarClientes()
        {
            var queryListaClientes = "SELECT * FROM Clientes";
            try
            {
                using (var conexion = _db.SuperConexionNando())
                {
                    var listaClientes = (await conexion.QueryAsync<Cliente>(queryListaClientes)).ToList();
                    _resultado.ok = true;
                    _resultado.clientes = listaClientes;
                    return _resultado;
                }
            }
            catch (Exception ex)
            {
                _resultado.ok = false;
                _resultado.mensaje = ex.Message;
                return _resultado;  
            }
        }

        public async Task<ResultadoCliente> ClientePorDNI(int DNI)
        {
            var clientedni = "SELECT * FROM Clientes WHERE DNI =" + DNI;

            using (var conexion = _db.SuperConexionNando())
            {
                try
                {
                    var cliente = await conexion.QuerySingleAsync<Cliente>(clientedni);

                    if (cliente != null)
                    {
                        _resultado.cliente = cliente;
                        _resultado.ok = true;
                        _resultado.mensaje = "";
                        return _resultado;
                    }                 
                }
                catch(Exception ex)
                {
                    _resultado.mensaje =ex.Message;
                }

                _resultado.ok = false;
                _resultado.cliente = null;
                return _resultado;
            }
        }

        public async Task<ResultadoCliente> AgregarCliente(int DNI, string nombre)
        {
            var insertcliente = "INSERT INTO Clientes (NomApe, DNI, IdEstado) VALUES (@nombreq, @dniq, @estadoq)";

            using (var conexion = _db.SuperConexionNando())
            {
                try
                {
                    await conexion.ExecuteAsync(insertcliente, new { nombreq = nombre, dniq = DNI, estadoq = 1 });
                    _resultado.ok = true;
                    _resultado.mensaje = "El cliente se agrego con exito";
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

        public async Task<ResultadoCliente> CambiarEstadoCliente(int estado, int id)
        {
            var updatecliente = "UPDATE Clientes SET IdEstado = @estadoq WHERE Id = @idq";

            using (var conexion = _db.SuperConexionNando())
            {
                try
                {
                    var r = await conexion.ExecuteAsync(updatecliente, new { estadoq = estado, idq = id });

                    if (r !=0)
                    {
                        _resultado.ok = true;
                        _resultado.mensaje = "El estado del cliente se cambio con exito";
                    }
                    else 
                    { 
                        _resultado.ok = false;
                        _resultado.mensaje = "No se pudo cambiar el estado del cliente, puede que la Id sea incorrecta";
                    }
                }
                catch(Exception ex)
                {
                    _resultado.ok = false;
                    _resultado.mensaje =ex.Message;
                }       
                return _resultado;
            }
        }
    }
}
