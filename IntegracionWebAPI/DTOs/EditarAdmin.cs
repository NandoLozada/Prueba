using System.ComponentModel.DataAnnotations;

namespace IntegracionWebAPI.DTOs
{
    public class EditarAdmin
    {
        [Required]
        [EmailAddress]
       public string Email { get; set; }
    }
}
