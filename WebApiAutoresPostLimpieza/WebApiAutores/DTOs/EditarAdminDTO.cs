using System.ComponentModel.DataAnnotations;

namespace WebApiAutores.DTOs
{
    public class EditarAdminDTO
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
