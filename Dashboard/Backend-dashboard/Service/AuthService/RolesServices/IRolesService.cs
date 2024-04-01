using Dominio.Models.AutenticacaoViewModel;
namespace Service.AuthService.RolesServices
{
    public interface IRolesService
    {

        Task<Resposta> AddUserToRole(string email, string roleName);
        Task<Resposta> CriarRoles(string roleName);
    }
}
