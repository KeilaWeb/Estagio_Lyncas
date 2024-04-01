using Dominio.Models.ApplicationUser;
using Dominio.Models.AutenticacaoViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Service.AuthService.RolesServices
{
    public class RolesService : IRolesService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RolesService> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<RolesService> logger)
        {
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        public async Task<Resposta> CriarRoles(string roleName)
        {
            try
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    if (roleResult.Succeeded)
                    {
                        _logger.LogInformation(1, "Roles adicionadas");
                        return new Resposta { Status = "Sucesso", Message = $"Role {roleName} adicionado com sucesso" };
                    }
                    else
                    {
                        _logger.LogInformation(2, "Erro");
                        return new Resposta { Status = "Erro", Message = "Role ja existe." };
                    }
                }
                else
                {
                    return new Resposta { Status = "Erro", Message = "Role ja existe." };
                }
            }
            catch (Exception ex)
            {
                return new Resposta { Status = "Error", Message = $"Ocorreu um erro durante a criação da role: {ex.Message}" };
            }

        }

        public async Task<Resposta> AddUserToRole(string email, string roleName)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var result = await _userManager.AddToRoleAsync(user, roleName);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation(1, $"User {user.Email} adicionado para {roleName} role");
                        return new Resposta { Status = "Success", Message = $"Usuário {user.Email} adicionado para a função {roleName}" };
                    }
                    else
                    {
                        _logger.LogInformation(1, $"Erro: não é possível adicionar usuário {user.Email} para a função {roleName}");
                        return new Resposta { Status = "Error", Message = $"Erro: não é possível adicionar usuário {user.Email} para a função {roleName}" };
                    }
                }
                else
                {
                    return new Resposta { Status = "Error", Message = "Não foi possível encontrar o usuário" };
                }
            }
            catch (Exception ex)
            {
                return new Resposta { Status = "Error", Message = $"Ocorreu um erro durante a adesão da role: {ex.Message}" };
            }
        }
    }
}
