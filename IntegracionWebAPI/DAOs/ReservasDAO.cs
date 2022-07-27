using IntegracionWebAPI.Entidades;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace IntegracionWebAPI.DAOs
{
    public class ReservasDAO
    {
        private readonly ConexionDB conexionDB;

        public ReservasDAO(ConexionDB conexionDB)
        {
            this.conexionDB = conexionDB;
        }

        public List<Reserva> ReservasPorCuarto(int idcuarto, DateTime fecinicio, DateTime fecfin)
        {
            var queryListaReservas = "SELECT * FROM Reservas WHERE IdCuarto = @idcuartoq AND IdEstado = 1";

            using (IDbConnection conexion = new SqlConnection(conexionDB.StringConexion()))

            {
                var listaReservas = conexion.Query<Reserva>(queryListaReservas, new { idcuartoq = idcuarto }).ToList();

                return listaReservas;
            }
        }

        public List<int> ListaIdCuarto()
        {
            var idcuartosquery = "SELECT Id FROM Cuartos WHERE IdEstado = @estado";
            List<int> listaidcuartos = new List<int>();

            using (IDbConnection conexion = new SqlConnection(conexionDB.StringConexion()))
            {
                listaidcuartos = conexion.Query<int>(idcuartosquery, new { estado = 1 }).ToList(); ;
            }

            return listaidcuartos.ToList();
        }

        //public List<Cliente> ClientePorDNI(int DNI)
        //{
        //    var clientedni = "SELECT * FROM Clientes WHERE DNI =" + DNI ;

        //    using (IDbConnection conexion = new SqlConnection(conexionDB.StringConexion()))

        //    {
        //        var cliente = conexion.Query<Cliente>(clientedni).ToList();

        //        return cliente;
        //    }
        //}

        public bool AgregarReserva(int idorden, int idcuarto, DateTime fecinicio, DateTime fecfin)
        {
            var insertreserva = "INSERT INTO Reservas (IdOrden, IdCuarto, FechaInicio, FechaFin, IdEstado) VALUES (@idordenq, @idcuartoq, @fecinicioq, @fecfinq, @estadoq)";
            var estadocuarto = "UPDATE Cuartos SET Estado = 2 WHERE Id = @idcuartoq";

            using (IDbConnection conexion = new SqlConnection(conexionDB.StringConexion()))

            {
                try
                {
                    conexion.Execute(insertreserva, new { idordenq = idorden, idcuartoq = idcuarto, fecinicioq = fecinicio, fecfinq = fecfin, estadoq = 1 });
                    conexion.Execute(estadocuarto, new { idcuartoq = idcuarto });
                    return true;
                }

                catch
                {
                    return false;
                }
            }

        }

        public List<Reserva> ListaReservasDAO()
        {
            var queryListaReservas = "SELECT * FROM Reservas";

            using (IDbConnection conexion = new SqlConnection(conexionDB.StringConexion()))

            {
                var listaReservas = conexion.Query<Reserva>(queryListaReservas).ToList();
                return listaReservas.ToList();
            }
        }

        public void EstadoReserva(int estado, int id)
        {
            var updatereserva = "UPDATE Reservas SET IdEstado = @estadoq WHERE Id = @idq";

            using (IDbConnection conexion = new SqlConnection(conexionDB.StringConexion()))

            {
                conexion.Execute(updatereserva, new { estadoq = estado, idq = id });
            }
        }

        public List<Reserva> ListaReservasPorCuarto(int id)
        {
            var reservasquery = "SELECT * FROM Reservas WHERE IdCuarto = @IdCuartoq";

            using (IDbConnection conexion = new SqlConnection(conexionDB.StringConexion()))
            {
                var listareservas = conexion.Query<Reserva>(reservasquery, new { IdCuartoq = id }).ToList();
                
                return listareservas.ToList();  
            }
        }
    }
}
