using Dapper;
using IntegracionWebAPI.Data;
using IntegracionWebAPI.Entidades;
using IntegracionWebAPI.Servicios.Interfaz;
using IntegracionWebAPI.Utiles;

namespace IntegracionWebAPI.Servicios.Implementacion
{
    public class ServicioNota : IServicioNota
    {
        private readonly DapperContext _db;
        private readonly ResultadoNota _resultado;

        public ServicioNota(DapperContext db, ResultadoNota resultado)
        {
            _db = db;
            _resultado = resultado;
        }

        public async Task<ResultadoNota> ListaNotas()
        {
            var queryListaNotas = "SELECT * FROM Notas";
            try
            {
                using (var conexion = _db.SuperConexionNando())
                {
                    var listaNotas = (await conexion.QueryAsync<Nota>(queryListaNotas)).ToList();

                    _resultado.ok = true;
                    _resultado.notas = listaNotas;
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

        public async Task<ResultadoNota> NotasPorCuarto(int idcuarto)
        {
            var notascuarto = "SELECT * FROM Notas WHERE IdCuarto =" + idcuarto;

            using (var conexion = _db.SuperConexionNando())
            {
                try
                {
                    var notas = (await conexion.QueryAsync<Nota>(notascuarto)).ToList();

                    if(notas != null)
                    {
                        _resultado.notas = notas;
                        _resultado.ok = true;
                        _resultado.mensaje = "";
                        return _resultado;
                    }
                }
                catch (Exception ex)
                {
                    _resultado.mensaje = ex.Message;
                }

                _resultado.ok = false;
                _resultado.notas = null;
                return _resultado;
            }
        }

        public async Task<ResultadoNota> AgregarNota(int idcuarto, string descripcion)
        {
            var cuartodisponible = "SELECT COUNT(*) FROM Cuartos WHERE Id = @idcuarto AND IdEstado = 1";
            var insertnota = "INSERT INTO Notas (IdCuarto, Descripcion) VALUES (@idcuarto, @descripcion)";

            using (var conexion = _db.SuperConexionNando())
            {
                try
                {
                    var r = await conexion.QuerySingleAsync<int>(cuartodisponible, new {idcuarto = idcuarto});

                    if (r != 0)
                    {
                        await conexion.ExecuteAsync(insertnota, new { idcuarto = idcuarto, descripcion = descripcion });
                        _resultado.ok = true;
                        _resultado.mensaje = "La nota se agrego con exito";
                    }
                    else
                    {
                        _resultado.ok = true;
                        _resultado.mensaje = "La nota no se pudo agregar, el Id de cuarto es incorrecto";
                    }
                }
                catch (Exception ex)
                {
                    _resultado.ok = false;
                    _resultado.mensaje = ex.Message;
                }
                return _resultado;

            }
        }

        public async Task<ResultadoNota> ActualizarNota(int id, string descripcion)
        {
            var updatecuarto = "UPDATE Notas SET Descripcion = @descripcionq WHERE Id = @id";

            using (var conexion = _db.SuperConexionNando())
            {
                try
                {
                    var r = await conexion.ExecuteAsync(updatecuarto, new { descripcionq = descripcion, id = id });
                    
                    if (r!=0)
                    {
                        _resultado.ok = true;
                        _resultado.mensaje = "La nota se actualizo con exito";
                    }
                    else
                    {
                        _resultado.ok = false;
                        _resultado.mensaje = "No se pudo cambiar el estado de la nota, puede que la Id sea incorrecta";
                    }
                }
                catch (Exception ex)
                {
                    _resultado.ok = false;
                    _resultado.mensaje = ex.Message;
                }
                return _resultado;
            }
        }
    }
}
