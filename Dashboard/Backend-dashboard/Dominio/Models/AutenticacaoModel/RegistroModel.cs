using System.ComponentModel.DataAnnotations;

namespace Dominio.Models.ViewModelVendedor
{
    public class RegistroModel
    {
        [Required(ErrorMessage = "Nome de usuário é obrigatorio")]
        public string? NomeUsuario { get; set; }

        [EmailAddress]
        [Required (ErrorMessage = "E-mail obrigatório")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Senha é um campo obrigatório")]
        public string? Senha { get; set; }
    }
}
