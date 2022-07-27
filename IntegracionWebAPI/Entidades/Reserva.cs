using System.ComponentModel.DataAnnotations;

namespace IntegracionWebAPI.Entidades
{
    public class Reserva
    {
        public int Id { get; set; }
        [Required]
        public int IdOrden { get; set; }
        [Required]
        public int IdCuarto { get; set; }

        public DateTime FechaInicio{ get; set; }
        public DateTime FechaFin { get; set; }

        public int IdEstado { get; set; }
    }
}
