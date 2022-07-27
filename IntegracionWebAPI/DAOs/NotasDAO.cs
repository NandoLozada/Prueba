using IntegracionWebAPI.Entidades;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace IntegracionWebAPI.DAOs
{
    public class NotasDAO
    {
        private readonly ConexionDB conexionDB;

        public NotasDAO(ConexionDB conexionDB)
        {
            this.conexionDB = conexionDB;
        }

        public List<Nota> ListaNotasDAO()
        {
            var queryListaNotas = "SELECT * FROM Notas";

            using (IDbConnection conexion = new SqlConnection(conexionDB.StringConexion()))
                
            {
               var listaNotas =  conexion.Query<Nota>(queryListaNotas).ToList();
               return listaNotas.ToList();
            }
        }

        public List<Nota> NotasPorCuarto(int idcuarto)
        {
            var notascuarto = "SELECT * FROM Notas WHERE IdCuarto =" + idcuarto ;

            using (IDbConnection conexion = new SqlConnection(conexionDB.StringConexion()))

            {
                var notas = conexion.Query<Nota>(notascuarto ).ToList();

                return notas.ToList();
            }
        }

        public void AgregarNota (int idcuarto, string descripcion)
        {
            var insertnota = "INSERT INTO Notas (IdCuarto, Descripcion) VALUES (@idcuarto, @descripcion)";

            using (IDbConnection conexion = new SqlConnection(conexionDB.StringConexion()))

            {
                conexion.Execute(insertnota, new { idcuarto = idcuarto, descripcion = descripcion });
            }
        }

        public void ActualizarNota(int id, string descripcion)
        {
            var updatecuarto = "UPDATE Notas SET Descripcion = @descripcionq WHERE Id = @id";

            using (IDbConnection conexion = new SqlConnection(conexionDB.StringConexion()))
            {
                conexion.Execute(updatecuarto, new { descripcionq = descripcion, id = id }); ;
            }

        }
    }
}
