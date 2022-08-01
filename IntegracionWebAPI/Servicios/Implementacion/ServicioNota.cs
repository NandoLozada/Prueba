using Dapper;
using IntegracionWebAPI.Data;
using IntegracionWebAPI.Entidades;
using IntegracionWebAPI.Servicios.Interfaz;

namespace IntegracionWebAPI.Servicios.Implementacion
{
    public class ServicioNota : IServicioNota
    {
        private readonly DapperContext _db;

        public ServicioNota(DapperContext db)
        {
            _db = db;
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

        public async Task<List<Nota>> NotasPorCuarto(int idcuarto)
        {
            var notascuarto = "SELECT * FROM Notas WHERE IdCuarto =" + idcuarto;

            using (var conexion = _db.SuperConexionNando())

            {
                var notas = (await conexion.QueryAsync<Nota>(notascuarto)).ToList();

                return notas;
            }
        }

        public void AgregarNota(int idcuarto, string descripcion)
        {
            var insertnota = "INSERT INTO Notas (IdCuarto, Descripcion) VALUES (@idcuarto, @descripcion)";

            using (var conexion = _db.SuperConexionNando())

            {
                conexion.Execute(insertnota, new { idcuarto = idcuarto, descripcion = descripcion });
            }
        }

        public void ActualizarNota(int id, string descripcion)
        {
            var updatecuarto = "UPDATE Notas SET Descripcion = @descripcionq WHERE Id = @id";

            using (var conexion = _db.SuperConexionNando())
            {
                conexion.Execute(updatecuarto, new { descripcionq = descripcion, id = id }); ;
            }
        }
    }
}
