using IntegracionWebAPI.Entidades;

namespace IntegracionWebAPI.Utiles
{
    public class ResultadoReserva:Resultado
    {
        public List<Reserva> reserva { get; set; }
        public List<int> cuartosdisponibles { get; set; }
    }
}
