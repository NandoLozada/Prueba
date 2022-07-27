using System.ComponentModel.DataAnnotations;
using IntegracionWebAPI.Entidades;

namespace IntegracionWebAPI.Entidades
{
    public class Cuarto
    {

        public int Id { get; set; }
        [Required]
        public int Capacidad { get; set; }
        [Required]
        public string Foto { get; set; }

        public int IdEstado { get; set; }

        public List<Nota> Notas { get; set; }

        public List<Reserva> Reservas { get; set; }

    }
}
