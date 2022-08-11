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

        public async Task<List<Nota>> ListaNotas()
        {
            var queryListaNotas = "SELECT * FROM Notas";

            using (var conexion = _db.SuperConexionNando())

            {
                var listaNotas = (await conexion.QueryAsync<Nota>(queryListaNotas)).ToList();
                return listaNotas;
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
                        _resultado.nota = notas;
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
                _resultado.nota = null;
                return _resultado;
            }
        }

        public async Task<ResultadoNota> AgregarNota(int idcuarto, string descripcion)
        {
            var insertnota = "INSERT INTO Notas (IdCuarto, Descripcion) VALUES (@idcuarto, @descripcion)";

            using (var conexion = _db.SuperConexionNando())
            {
                try
                {
                    await conexion.ExecuteAsync(insertnota, new { idcuarto = idcuarto, descripcion = descripcion });
                    _resultado.ok = true;
                    _resultado.mensaje = "La nota se agrego con exito";
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

        public async Task<ResultadoNota> ActualizarNota(int id, string descripcion)
        {
            var updatecuarto = "UPDATE Notas SET Descripcion = @descripcionq WHERE Id = @id";

            using (var conexion = _db.SuperConexionNando())
            {
                try
                {
                    await conexion.ExecuteAsync(updatecuarto, new { descripcionq = descripcion, id = id });
                    _resultado.ok = true;
                    _resultado.mensaje = "La nota se actualizo con exito";
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
