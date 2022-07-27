using System.ComponentModel.DataAnnotations;

namespace IntegracionWebAPI.Entidades
{
    public class Cliente
    {
        public int Id { get; set; }
        [Required]
        public string NomApe { get; set; }
        [Required]
        public int DNI { get; set; }

        public int IdEstado { get; set; }
    }
}
