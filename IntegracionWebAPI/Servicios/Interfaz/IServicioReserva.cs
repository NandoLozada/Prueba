using IntegracionWebAPI.Entidades;

namespace IntegracionWebAPI.Servicios.Interfaz
{
    public interface IServicioReserva
    {
        public Task<List<Reserva>> ListaReservas();
        public Task<List<Reserva>> ListaReservasPorCuarto(int idcuarto);
    }
}
