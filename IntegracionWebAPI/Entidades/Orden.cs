using System.ComponentModel.DataAnnotations;

namespace IntegracionWebAPI.Entidades
{
    public class Orden
    {
        public int Id { get; set; }
        [Required]
        public int IdCliente { get; set; }
    }
}
