using Dominio.Models.DTO;
using Dominio.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.VendaService;

namespace ClienteVendaApi.Controllers
{
    [Authorize]
    [Route("api/vendas")]
    [ApiController]
    public class VendasController : ControllerBase
    {
        public readonly IVendaService _serviceVenda;
        public VendasController(IVendaService serviceVenda)
        {
            _serviceVenda = serviceVenda;
        }

        [HttpGet]
        public async Task<ActionResult<List<ListaDeVendas>>> BuscarVendasController()
        {
            var vendas = await _serviceVenda.BuscarVendas();
            return Ok(vendas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FiltroVendaClienteViewModel>> BuscarVendaIdController(int id)
        {
            var venda = await _serviceVenda.BuscaVendaPorId(id);
            if (venda == null)
            {
                return NotFound();
            }
            return Ok(venda);
        }

        [HttpPost]
        public async Task<ActionResult<VendaDTO>> CriarVendaController(VendaDTO vendaDTO)
        {
            await _serviceVenda.CriarServiceT(vendaDTO);
            return Created("{model.Id}", vendaDTO); //201
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VendaDTO>> AtualizarVendaController(VendaDTO vendaDTO, int id)
        {
            return Ok(await _serviceVenda.AtualizarServiceT(vendaDTO, id)); //200
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarCliente(int id)
        {
            await _serviceVenda.DeletarVendaAsync(id);
            return NoContent(); //204
        }

    }
}
