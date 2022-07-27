using System.ComponentModel.DataAnnotations;

namespace IntegracionWebAPI.DTOs
{
    public class CredencialesUsuario
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public string NuevaPass { get; set; }
    }
}
