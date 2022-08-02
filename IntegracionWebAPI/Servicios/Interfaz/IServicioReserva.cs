using IntegracionWebAPI.Entidades;

namespace IntegracionWebAPI.Servicios.Interfaz
{
    public interface IServicioReserva
    {
        public Task<List<Reserva>> ListaReservas();
        public Task<List<Reserva>> ListaReservasPorCuarto(int idcuarto);
        public Task<List<int>> CuartosDisponibles(DateTime fechaini, DateTime fechafin);
        public Task<bool> TomarReserva(int idorden, int idcuarto, DateTime fecinicio, DateTime fecfin);
        public void CambiarEstadoReserva(int estado, int id);
    }
}
