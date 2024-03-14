using System.ComponentModel.DataAnnotations;

namespace Dominio.Models.ViewModelCliente
{
    public class ClienteBuscaTodos
    {
        public int Id { get; set; }
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
