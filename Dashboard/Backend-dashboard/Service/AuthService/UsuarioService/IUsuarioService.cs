using Dominio.Models.AutenticacaoViewModel;
using Dominio.Models.ViewModelVendedor;

namespace Service.AuthService.Cadastro
{
    public interface IUsuarioService
    {
        Task<Resposta> CadastrarUsuario(RegistroModel model);
    }
}
