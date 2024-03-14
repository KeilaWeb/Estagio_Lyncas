using Dominio.Models.DTO;
using Dominio.Models.Entities;
using Dominio.Models.ViewModels;
using Service.GenericService;

namespace Service.VendaService
{
    public interface IVendaService : IGenericService<Venda, VendaDTO, Venda>
    {
        Task<List<ListaDeVendas>> BuscarVendas();
        Task<FiltroVendaClienteViewModel> BuscaVendaPorId(int id);
        Task DeletarVendaAsync(int id);
    }
}
