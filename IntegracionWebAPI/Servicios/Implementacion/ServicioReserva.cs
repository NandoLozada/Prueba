using Dapper;
using IntegracionWebAPI.Data;
using IntegracionWebAPI.Entidades;
using IntegracionWebAPI.Servicios.Interfaz;
using IntegracionWebAPI.Utiles;

namespace IntegracionWebAPI.Servicios.Implementacion
{
    public class ServicioReserva:IServicioReserva
    {
        private readonly DapperContext _db;
        private  ResultadoReserva _resultado;

        public ServicioReserva (DapperContext db, ResultadoReserva resultado)
        {
            _db = db;
            _resultado = resultado;
        }

        public async Task<ResultadoReserva> ListaReservas()
        {
            var queryListaReservas = "SELECT * FROM Reservas";

            using (var conexion = _db.SuperConexionNando())
            {
                try
                {
                    var listaReservas = (await conexion.QueryAsync<Reserva>(queryListaReservas)).ToList();
                    _resultado.reserva = listaReservas;
                }
               catch(Exception ex)
                {
                    _resultado.mensaje = ex.Message;
                }

                return _resultado;
            }
        }
        public async Task<ResultadoReserva> ListaReservasPorCuarto(int id)
        {            
            var reservasquery = "SELECT * FROM Reservas WHERE IdCuarto = @IdCuartoq";
            try
            {
                using (var conexion = _db.SuperConexionNando())
                {
                    var listareservas = (await conexion.QueryAsync<Reserva>(reservasquery, new { IdCuartoq = id })).ToList();
                   
                    if(listareservas != null)
                    {
                        _resultado.reserva = listareservas;
                        _resultado.ok = true;
                        _resultado.mensaje = "";
                        return _resultado;
                    }                    
                }
            }
            catch (Exception ex)
            {
                _resultado.mensaje = ex.Message;
            }

            _resultado.ok = false;
            _resultado.reserva = null;
            return _resultado;
        }
        public async Task<ResultadoReserva> CuartosDisponibles(DateTime fechaini, DateTime fechafin)
        {
            try
            {
                var listaid = await ListaIdCuarto();

                foreach (int Id in listaid)
                {
                    if (await HayDisponibilidad(Id, fechaini, fechafin))
                    {
                        _resultado.cuartosdisponibles.Add(Id);
                    }
                }
                _resultado.ok = true;
                _resultado.mensaje = "";
            }
            catch(Exception ex)
            {
                _resultado.mensaje=ex.Message;
            }
            return _resultado;
        }
        public async Task<ResultadoReserva> TomarReserva(int idorden, int idcuarto, DateTime fecinicio, DateTime fecfin)
        {          
            
            if (await HayDisponibilidad(idcuarto, fecinicio, fecfin))
            {
                _resultado = await AgregarReserva(idorden, idcuarto, fecinicio, fecfin);
            }         
            else
            {
                _resultado.ok = false;
                _resultado.mensaje = "No hay disponibilidad";
            }

            return _resultado;   
        }
        public async Task<ResultadoReserva> CambiarEstadoReserva(int estado, int id)
        {
            var updatereserva = "UPDATE Reservas SET IdEstado = @estadoq WHERE Id = @idq";

            try
            {
                using (var conexion = _db.SuperConexionNando())
                {
                    await conexion.ExecuteAsync(updatereserva, new { estadoq = estado, idq = id });

                    _resultado.ok = true;
                    _resultado.mensaje = "El estado de la reserva se actualizo con exito";
                }
            }
            catch (Exception ex)
            {
                _resultado.ok = false;
                _resultado.mensaje = ex.Message;
            }

            return _resultado;
        }   

        //***************************************************************************

        private async Task<bool> HayDisponibilidad(int idcuarto, DateTime fecinicio, DateTime fecfin)
        {
            List<Reserva> reservaList = new List<Reserva>();

            reservaList = await ReservasPorCuarto(idcuarto, fecinicio, fecfin); //Lista de reservas para un cuarto

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
        private async Task<List<Reserva>> ReservasPorCuarto(int idcuarto, DateTime fecinicio, DateTime fecfin)
        {
            var queryListaReservas = "SELECT * FROM Reservas WHERE IdCuarto = @idcuartoq AND IdEstado = 1";

            using (var conexion = _db.SuperConexionNando())
            {
                var listaReservas = (await conexion.QueryAsync<Reserva>(queryListaReservas, new { idcuartoq = idcuarto })).ToList();

                return listaReservas;
            }
        }
        private async Task<List<int>> ListaIdCuarto()
        {
            var idcuartosquery = "SELECT Id FROM Cuartos WHERE IdEstado = @estado";
            List<int> listaidcuartos = new List<int>();

            using (var conexion = _db.SuperConexionNando())
            {
                listaidcuartos = (await conexion.QueryAsync<int>(idcuartosquery, new { estado = 1 })).ToList(); ;
            }

            return listaidcuartos.ToList();
        }
        private async Task<ResultadoReserva> AgregarReserva(int idorden, int idcuarto, DateTime fecinicio, DateTime fecfin)
        {
            var insertreserva = "INSERT INTO Reservas (IdOrden, IdCuarto, FechaInicio, FechaFin, IdEstado) VALUES (@idordenq, @idcuartoq, @fecinicioq, @fecfinq, @estadoq)";
            var estadocuarto = "UPDATE Cuartos SET Estado = 2 WHERE Id = @idcuartoq";

            try
            {
                using (var conexion = _db.SuperConexionNando())
                {
                    await conexion.ExecuteAsync(insertreserva, new { idordenq = idorden, idcuartoq = idcuarto, fecinicioq = fecinicio, fecfinq = fecfin, estadoq = 1 });
                    await conexion.ExecuteAsync(estadocuarto, new { idcuartoq = idcuarto });

                    _resultado.ok = true;
                    _resultado.mensaje = "La reserva se realizo con exito";
                }
            }
            catch (Exception ex)
            {
                _resultado.mensaje = ex.Message;
                _resultado.ok = false;
            }
            return _resultado;
        }
    }      
}
