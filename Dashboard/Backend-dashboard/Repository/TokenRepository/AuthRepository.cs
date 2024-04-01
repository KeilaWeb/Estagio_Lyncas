using Dominio.Models.ApplicationUser;
using Microsoft.AspNetCore.Identity;

namespace Repository.TokenRepository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> Login(string email, string senha)
        {
            // Usamos ApplicationUser como o tipo genérico para UserManager
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && await _userManager.CheckPasswordAsync(user, senha))
            {
                return user;
            }
            return null;
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
