using Dominio.Data;
using Dominio.Models.ApplicationUser;
using Dominio.Models.ViewModelVendedor;
using Microsoft.AspNetCore.Identity;

namespace Repository.TokenRepository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser?> TokenLogin(string email)
        {
            // Usamos ApplicationUser como o tipo genérico para UserManager
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> UsuarioExistente(string email)
        {
            var userExiste = await _userManager.FindByEmailAsync(email!);
            return userExiste != null;
        }

        public async Task<bool> CadastrarUsuario(ApplicationUser user, string senha)
        {
            var result = await _userManager.CreateAsync(user, senha);
            return result.Succeeded;
        }

    }
}
