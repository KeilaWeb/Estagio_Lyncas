using Dominio.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.ClienteServices;

namespace ClienteVendaApi.Controllers
{
    [Authorize]
    [Route("api/clientes")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        public readonly IGenericService _serviceCliente;
        private readonly ILogger<ClienteController> _logger;

        public ClienteController(IGenericService serviceCliente, ILogger<ClienteController> logger)
        {
            _serviceCliente = serviceCliente;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<ClienteDTO>>> BuscarClientesController()
        {
            var clientes = await _serviceCliente.BuscarTodosServiceT();
            return Ok(clientes);
        }

        [HttpPost]
        [Route("busca")]
        public async Task<ActionResult> PaginarClientesController([FromBody]Paginacao paginacao)
        {
            try
            {
                var (clientes, totalClientes) = await _serviceCliente.PaginarCliente(paginacao);
                var data = new
                {
                    clientes,
                    totalClientes
                };

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao paginar clientes: {ex.Message}");
            }
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDTO>> BuscaClienteIdController(int id)
        {
            _logger.Log(LogLevel.Error, "Teve um erro");
            var cliente = await _serviceCliente.BuscaPorIdServiceT(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<ClienteDTO>> CriarClienteController(ClienteDTO entityDTO)
        {
            var clienteCriado = await _serviceCliente.CriarServiceT(entityDTO);
            return Created($"/api/clientes/{clienteCriado.Id}", clienteCriado);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ClienteDTO>> AtualizarClienteController(ClienteDTO clienteDTO, int id)
        {
            return Ok(await _serviceCliente.AtualizarServiceT(clienteDTO, id));//200
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarCliente(int id)
        {
            await _serviceCliente.DeletarClienteAsync(id);
            return NoContent(); //204
        }


    }
}
