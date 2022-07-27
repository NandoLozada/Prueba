namespace IntegracionWebAPI.Models
{
    public class ReservasReporte
    {
        public int Id { get; set; }
        public int IdOrden { get; set; }
        public int IdCuarto { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int IdEstado { get; set; }
    }
}
