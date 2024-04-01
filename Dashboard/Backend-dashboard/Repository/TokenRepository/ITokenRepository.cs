using Dominio.Models.ApplicationUser;
using Dominio.Models.ViewModelVendedor;

namespace Repository.TokenRepository
{
    public interface ITokenRepository
    {
        Task<ApplicationUser> TokenLogin(string email);
        Task<bool> UsuarioExistente(string email);
        Task<bool> CadastrarUsuario(ApplicationUser user, string senha);
    }
}
