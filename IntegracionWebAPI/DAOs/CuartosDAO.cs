using Dapper;
using IntegracionWebAPI.Entidades;
using IntegracionWebAPI.Servicios;
using System.Data;
using System.Data.SqlClient;

namespace IntegracionWebAPI.DAOs
{
    public class CuartosDAO
    {
        private readonly ConexionDB conexionDB;

        public CuartosDAO(ConexionDB conexionDB)
        {
            this.conexionDB = conexionDB;
        }

        public List<Cuarto> ListaCuartosDAO()
        {
            var queryjoin = "SELECT * FROM Cuartos LEFT JOIN Notas ON Cuartos.Id = Notas.IdCuarto ";

            var diccuarto = new Dictionary<int, Cuarto>();

            using (IDbConnection conexion = new SqlConnection(conexionDB.StringConexion()))

            {
                var listado = conexion.Query<Cuarto, Nota, Cuarto>(queryjoin, (cuarto, nota) =>
                {
                    Cuarto cuartotemp;

                    if (!diccuarto.TryGetValue(cuarto.Id, out cuartotemp))
                    {
                        cuartotemp = cuarto;
                        cuartotemp.Notas = new List<Nota>();
                        diccuarto.Add(cuartotemp.Id, cuarto);
                    }

                    if (nota != null)
                    {
                        cuartotemp.Notas.Add(nota);
                    }

                    return cuartotemp;

                }).Distinct().ToList();

                return listado.ToList();
            }
        }

        public List<Cuarto> CuartoPorId(int idcuarto)
        {
            var queryjoin = "SELECT * FROM Cuartos LEFT JOIN Notas ON Cuartos.Id = Notas.IdCuarto WHERE Cuartos.Id = " + idcuarto;

            var diccuarto = new Dictionary<int, Cuarto>();

            using (IDbConnection conexion = new SqlConnection(conexionDB.StringConexion()))

            {
                var listado = conexion.Query<Cuarto, Nota, Cuarto>(queryjoin, (cuarto, nota) =>
                {
                    Cuarto cuartotemp;

                    if (!diccuarto.TryGetValue(cuarto.Id, out cuartotemp))
                    {
                        cuartotemp = cuarto;
                        cuartotemp.Notas = new List<Nota>();
                        diccuarto.Add(cuartotemp.Id, cuarto);
                    }

                    if (nota != null)
                    {
                        cuartotemp.Notas.Add(nota);
                    }

                    return cuartotemp;

                }).Distinct().ToList();

                return listado.ToList();
            }
        }

        public void AgregarCuarto(int capacidad, string foto)
        {
            var insertcuarto = "INSERT INTO Cuartos (Capacidad, Foto, IdEstado) VALUES (@capacidadq, @fotoq, @estadoq)";

            using (IDbConnection conexion = new SqlConnection(conexionDB.StringConexion()))

            {
                conexion.Execute(insertcuarto, new { capacidadq = capacidad, fotoq = foto, estadoq = 1 });
            }
        }

        public void EstadoCuarto(int estado, int id)
        {
            var updatecuarto = "UPDATE Cuartos SET IdEstado = @estadoq WHERE Id = @idq";

            using (IDbConnection conexion = new SqlConnection(conexionDB.StringConexion()))

            {
                conexion.Execute(updatecuarto, new { estadoq = estado, idq = id });
            }
        }

        public void ActualizarCuarto (int idcuarto, int capacidad, string foto)
        {
            var updatecuarto = "UPDATE Cuartos SET Capacidad = @capacidadq, Foto = @fotoq WHERE Id = @id";
                        
            using (IDbConnection conexion = new SqlConnection(conexionDB.StringConexion()))
            {
                conexion.Execute(updatecuarto, new { capacidadq = capacidad, fotoq = foto, id = idcuarto }); ;
            }

        }

        public List<int> ListaIdCuarto()
        {
            var idcuartosquery = "SELECT Id FROM Cuarto WHERE IdEstado = @estado";

            using (IDbConnection conexion = new SqlConnection(conexionDB.StringConexion()))
            {
                var listaidcuartos = conexion.Query(idcuartosquery, new {estado = 1}); ;
            }

            return ListaIdCuarto().ToList();
        }
        
    }
}

