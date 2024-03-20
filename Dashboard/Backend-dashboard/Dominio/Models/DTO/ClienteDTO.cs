using System.ComponentModel.DataAnnotations;

namespace Dominio.Models.DTO
{
    public class ClienteDTO
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Telefone { get; set; }
        [Required]
        public string CPF { get; set; }

    }
}
