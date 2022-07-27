using System.ComponentModel.DataAnnotations;


namespace IntegracionWebAPI.Entidades
{
    public class Nota
    {
        public int Id { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public int IdCuarto { get; set; }
    }
}
