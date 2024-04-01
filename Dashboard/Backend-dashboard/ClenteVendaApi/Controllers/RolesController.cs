using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.AuthService.RolesServices;

namespace ClienteVendaApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _rolesService;
        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        [HttpPost]
        [Route("CriandoRole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            try
            {
                var roleExist = await _rolesService.CriarRoles(roleName);
                return Ok(roleExist);
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
                var result = await _rolesService.AddUserToRole(email, roleName);
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
