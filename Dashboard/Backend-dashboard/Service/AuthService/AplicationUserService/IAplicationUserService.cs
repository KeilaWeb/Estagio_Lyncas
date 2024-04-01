using Dominio.Models.AutenticacaoViewModel;
using Dominio.Models.LoginViewModel;

namespace Service.AuthService.AuthService
{
    public interface IAplicationUserService
    {
        Task<object> Login(LoginModel model);
        Task<Resposta> RefreshToken(TokenModel tokenModel);
        Task<Resposta> RevokeToken(string username);
    }
}
