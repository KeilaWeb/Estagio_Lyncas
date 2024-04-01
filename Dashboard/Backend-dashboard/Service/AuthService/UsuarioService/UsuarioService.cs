using Dominio.Models.ApplicationUser;
using Dominio.Models.AutenticacaoViewModel;
using Dominio.Models.ViewModelVendedor;
using Repository.TokenRepository;

namespace Service.AuthService.Cadastro
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IAuthRepository _tokenRepository;
        public UsuarioService(IAuthRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public async Task<Resposta> CadastrarUsuario(RegistroModel model)
        {
            try
            {
                var usuarioJaExiste = await _tokenRepository.UsuarioExistente(model.Email);
                if (usuarioJaExiste)
                {
                    return new Resposta { Status = "Error", Message = "Usuário já existe" };
                }

                ApplicationUser user = new ApplicationUser
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.NomeUsuario
                };


                var resultadoNovoUsuario = await _tokenRepository.CadastrarUsuario(user, model.Senha!);

                if (!resultadoNovoUsuario)
                {
                    return new Resposta { Status = "Error", Message = "Falha ao criar usuário" };
                }
                return new Resposta { Status = "Sucesso", Message = "Usuário criado com sucesso" };

            }
            catch (Exception ex)
            {
                return new Resposta { Status = "Error", Message = $"Erro durante o cadastro: {ex.Message}" };
            }
        }
    }
}
