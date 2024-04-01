using Dominio.Models.AutenticacaoViewModel;
using Dominio.Models.LoginViewModel;
using Dominio.Models.ViewModelVendedor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.TokenService;

namespace ClienteVendaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        public AuthController(ITokenService tokenService, RoleManager<IdentityRole> roleManager,
                                IConfiguration configuration, ILogger<AuthController> logger)
        {
            _tokenService = tokenService;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var resultado = await _tokenService.Login(model);
                if (resultado != null)
                {
                    return Ok("Login bem-sucedido");
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
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
                var usuarioJaExiste = await _tokenService.CadastrarUsuario(model);

                if (usuarioJaExiste != null)
                {
                    return Ok(new Resposta { Status = "Sucesso", Message = "Usuário criado com sucesso" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new Resposta { Status = "Error", Message = "Usuário ja existe no sistema" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro durante o login: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("verificar-token")]
        [Authorize]
        public IActionResult VerificarToken()
        {
            return Ok();
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            try
            {
                var result = await _tokenService.RefreshToken(tokenModel);

                if (result != null)
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                       new Resposta { Status = "Error", Message = "RefreshToken inválido" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro ao atualizar o token: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            try
            {
                var result = await _tokenService.RevokeToken(username);
                if (result.Status == "Success")
                {
                    return NoContent();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro ao revogar o token: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("CriandoRole")]
        public async Task<IActionResult> CreateRole(string roleName)
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
                        return StatusCode(StatusCodes.Status200OK,
                            new Resposta { Status = "Sucesso", Message = $"Role {roleName} adicionado com sucesso" });
                    }
                    else
                    {
                        _logger.LogInformation(2, "Erro");
                        return StatusCode(StatusCodes.Status400BadRequest,
                            new Resposta { Status = "Erro", Message = "Role ja existe." });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Resposta { Status = "Erro", Message = "Role ja existe." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro durante a criação da role: {ex.Message}");
            }

        }

        [HttpPost]
        [Route("AdicionaUsuarioNaToRole")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            try
            {
                var result = await _tokenService.AddUserToRole(email, roleName);
                if (result.Status == "Success")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocorreu um erro durante a adesão da role: {ex.Message}");
            }
        }
    }
}
