using Dominio.Models.AutenticacaoViewModel;
using Dominio.Models.LoginViewModel;
using Dominio.Models.ViewModelVendedor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.AuthService.AuthService;

namespace ClienteVendaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAplicationUserService _tokenService;
        public AuthController(IAplicationUserService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var tokens = await _tokenService.Login(model);
                if (tokens != null)
                {
                    return Ok(tokens);
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
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            try
            {
                var result = await _tokenService.RefreshToken(tokenModel);

                if (result != null)
                {
                    return Ok(result);
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

        
    }
}
