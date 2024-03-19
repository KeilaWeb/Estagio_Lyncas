using Dominio.Models.ApplicationUser;
using Dominio.Models.AutenticacaoViewModel;
using Dominio.Models.LoginViewModel;
using Dominio.Models.ViewModelVendedor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.TokenService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ClienteVendaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AuthController(ITokenService tokenService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email!); // ! <- indica que é certeza que não é nulo
                if(user is not null && await _userManager.CheckPasswordAsync(user, model.Senha!))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var authClaims = new List<Claim>  // criados para definir o token de autenticação
                    {
                        new Claim(ClaimTypes.Email, user.Email!),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };
                    foreach (var userRole in userRoles) // para cada perfil um user role
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }
                    var token = _tokenService.GenerateAccessToken(authClaims, _configuration);
                    var refreshToken = _tokenService.GenerateRefreshToken();
                    _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"],
                                    out int refreshTokenValidityInMinutes);
                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);
                    await _userManager.UpdateAsync(user);
                    return Ok(new
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        RefreshToken = refreshToken,
                        Expiration = token.ValidTo
                    });
                }else
                {
                    return Unauthorized();  
                }

            }catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro durante o login: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("cadastro")]
        public async Task<IActionResult> Cadastro([FromBody] RegistroModel model)
        {
            try
            {
                var userExists = await _userManager.FindByNameAsync(model.NomeUsuario!);
                if(userExists != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, 
                            new Response { Status = "Error", Message = "Usuario ja existe!" });
                }
                ApplicationUser user = new()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.NomeUsuario
                };
                var result = await _userManager.CreateAsync(user, model.Senha!);

                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                            new Response { Status = "Error", Message = "Falha ao criar usuario." });
                }else
            {
                return Ok(new Response { Status = "Sucesso", Message = "Usuario criado com sucesso" });
            }

            }catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro durante o login: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if(tokenModel is null)
            {
                return BadRequest("Solicitação de cliente inválida");
            }
            string? accessToken = tokenModel.AccessToken ?? throw new ArgumentNullException(nameof(tokenModel));
            string? refreshToken = tokenModel.RefreshToken ?? throw new ArgumentNullException(nameof (tokenModel));
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);
            if(principal == null)
            {
                return BadRequest("Token de acesso/token de atualização inválido");
            }
            string username = principal.Identity.Name;
            var user = await _userManager.FindByNameAsync(username!);
            if (user == null || user.RefreshToken != refreshToken 
                             || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Token de acesso/token de atualização inválido");
            }
            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);
            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });
        }

        [Authorize]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return BadRequest("Nome de usuario invalido");
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
            return NoContent();
        }
    }
}
