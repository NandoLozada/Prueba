using IntegracionWebAPI.Entidades;
using IntegracionWebAPI.Utiles;

namespace IntegracionWebAPI.Servicios.Interfaz
{
    public interface IServicioReserva
    {
        public Task<ResultadoReserva> ListaReservas();
        public Task<ResultadoReserva> ListaReservasPorCuarto(int idcuarto);
        public Task<ResultadoReserva> CuartosDisponibles(DateTime fechaini, DateTime fechafin);
        public Task<ResultadoReserva> TomarReserva(int idorden, int idcuarto, DateTime fecinicio, DateTime fecfin);
        public Task<ResultadoReserva> CambiarEstadoReserva(int estado, int id);
    }
}
