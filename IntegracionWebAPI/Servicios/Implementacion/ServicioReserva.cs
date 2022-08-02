using Dapper;
using IntegracionWebAPI.Data;
using IntegracionWebAPI.Entidades;
using IntegracionWebAPI.Servicios.Interfaz;

namespace IntegracionWebAPI.Servicios.Implementacion
{
    public class ServicioReserva:IServicioReserva
    {
        private readonly DapperContext _db;

        public ServicioReserva (DapperContext db)
        {
            _db = db;
        }

        public async Task<List<Reserva>> ListaReservas()
        {
            var queryListaReservas = "SELECT * FROM Reservas";

            using (var conexion = _db.SuperConexionNando())

            {
                var listaReservas = (await conexion.QueryAsync<Reserva>(queryListaReservas)).ToList();
                return listaReservas.ToList();
            }
        }
        public async Task<List<Reserva>> ListaReservasPorCuarto(int id)
        {
            var reservasquery = "SELECT * FROM Reservas WHERE IdCuarto = @IdCuartoq";

            using (var conexion = _db.SuperConexionNando())
            {
                var listareservas = (await conexion.QueryAsync<Reserva>(reservasquery, new { IdCuartoq = id })).ToList();

                return listareservas.ToList();
            }
        }
        public async Task<List<int>> CuartosDisponibles(DateTime fechaini, DateTime fechafin)
        {
            List<int> listacuartosdisponibles = new List<int>();

            var listaid = await ListaIdCuarto();

            foreach (int Id in listaid)
            {
                if (HayDisponibilidad(Id, fechaini, fechafin))
                {
                    listacuartosdisponibles.Add(Id);
                }
            }

            return listacuartosdisponibles;
        }
        public async Task<bool> TomarReserva(int idorden, int idcuarto, DateTime fecinicio, DateTime fecfin)
        {
            bool ok = false;
            if (HayDisponibilidad(idcuarto, fecinicio, fecfin))
            {
                ok = await AgregarReserva(idorden, idcuarto, fecinicio, fecfin);
            }

            if (ok)
            { return true; }
            else { return false; }

        }
        public void CambiarEstadoReserva(int estado, int id)
        {
            var updatereserva = "UPDATE Reservas SET IdEstado = @estadoq WHERE Id = @idq";

            using (var conexion = _db.SuperConexionNando())
            {
                conexion.Execute(updatereserva, new { estadoq = estado, idq = id });
            }
        }

        


        ///***************************************************************************


        public bool HayDisponibilidad(int idcuarto, DateTime fecinicio, DateTime fecfin)
        {
            List<Reserva> reservaList = new List<Reserva>();

            reservaList = ReservasPorCuarto(idcuarto, fecinicio, fecfin); //Lista de reservas para un cuarto

            foreach (Reserva reserva in reservaList)
            {
                if (fecinicio > reserva.FechaInicio & fecinicio < reserva.FechaFin)// esto en la query
                {
                    return false;
                }
                else
                {
                    if (fecfin > reserva.FechaInicio & fecfin < reserva.FechaFin)
                    {
                        return false;
                    }
                }

                if (fecinicio < reserva.FechaInicio & fecfin > reserva.FechaFin)
                {
                    return false;
                }
            }
            return true;
        }
        public List<Reserva> ReservasPorCuarto(int idcuarto, DateTime fecinicio, DateTime fecfin)
        {
            var queryListaReservas = "SELECT * FROM Reservas WHERE IdCuarto = @idcuartoq AND IdEstado = 1";

            using (var conexion = _db.SuperConexionNando())
            {
                var listaReservas = conexion.Query<Reserva>(queryListaReservas, new { idcuartoq = idcuarto }).ToList();

                return listaReservas;
            }
        }
        public async Task<List<int>> ListaIdCuarto()
        {
            var idcuartosquery = "SELECT Id FROM Cuartos WHERE IdEstado = @estado";
            List<int> listaidcuartos = new List<int>();

            using (var conexion = _db.SuperConexionNando())
            {
                listaidcuartos =(await conexion.QueryAsync<int>(idcuartosquery, new { estado = 1 })).ToList(); ;
            }

            return listaidcuartos.ToList();
        }
        public async Task<bool> AgregarReserva(int idorden, int idcuarto, DateTime fecinicio, DateTime fecfin)
        {
            var insertreserva = "INSERT INTO Reservas (IdOrden, IdCuarto, FechaInicio, FechaFin, IdEstado) VALUES (@idordenq, @idcuartoq, @fecinicioq, @fecfinq, @estadoq)";
            var estadocuarto = "UPDATE Cuartos SET Estado = 2 WHERE Id = @idcuartoq";

            using (var conexion = _db.SuperConexionNando())
            {
                try
                {
                    await conexion.ExecuteAsync(insertreserva, new { idordenq = idorden, idcuartoq = idcuarto, fecinicioq = fecinicio, fecfinq = fecfin, estadoq = 1 });
                    await conexion.ExecuteAsync(estadocuarto, new { idcuartoq = idcuarto });
                    return true;
                }

                catch
                {
                    return false;
                }
            }
        }
    }
}
