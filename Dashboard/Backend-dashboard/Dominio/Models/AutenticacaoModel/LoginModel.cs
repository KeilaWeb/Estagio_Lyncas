using System.ComponentModel.DataAnnotations;

namespace Dominio.Models.LoginViewModel
{
    public class LoginModel
    {
        [Required(ErrorMessage = "O e-email é obrigatório")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        public string? Senha { get; set; }

    }
}
