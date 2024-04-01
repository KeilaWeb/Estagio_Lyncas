using Dominio.Models.AutenticacaoViewModel;
using Dominio.Models.ViewModelVendedor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.AuthService.Cadastro;

namespace ClienteVendaApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _cadastroService;
        public UsuarioController(IUsuarioService cadastroService)
        {
            _cadastroService = cadastroService;
        }

        [HttpPost]
        [Route("cadastro")]
        public async Task<IActionResult> Cadastro([FromBody] RegistroModel model)
        {
            try
            {
                var usuarioJaExiste = await _cadastroService.CadastrarUsuario(model);

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
    }
}
