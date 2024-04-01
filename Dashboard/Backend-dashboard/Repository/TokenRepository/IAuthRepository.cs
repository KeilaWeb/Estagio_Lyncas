using Dominio.Models.ApplicationUser;

namespace Repository.TokenRepository
{
    public interface IAuthRepository
    {
        Task<ApplicationUser> Login(string email, string senha);
        Task<bool> UsuarioExistente(string email);
        Task<bool> CadastrarUsuario(ApplicationUser user, string senha);
    }
}
