using Dominio.Models.AutenticacaoViewModel;
using Dominio.Models.LoginViewModel;
using Dominio.Models.ViewModelVendedor;

namespace Service.TokenService
{
    public interface ITokenService
    {
        Task<object> Login(LoginModel model);
        Task<Resposta> CadastrarUsuario(RegistroModel model);
        Task<Resposta> RefreshToken(TokenModel tokenModel);
        Task<Resposta> RevokeToken(string username);
        Task<Resposta> AddUserToRole(string email, string roleName);
    }
}
