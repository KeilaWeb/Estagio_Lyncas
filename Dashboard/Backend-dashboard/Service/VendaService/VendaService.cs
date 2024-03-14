using Dominio.Models.DTO;
using Dominio.Models.Entities;
using Service.GenericService;
using AutoMapper;
using Repository.VendasRepositorys;
using Dominio.Models.ViewModels;

namespace Service.VendaService
{
    public class VendaService : GenericService<Venda, VendaDTO, Venda>, IVendaService
        //importou o service generico aqui para implementar os métodos
    {
        private readonly IVendaRepository _repositoryVenda;
        private readonly IMapper _mapper;
        public VendaService(IMapper mapperVenda, IVendaRepository repositoryVenda) 
            :base(mapperVenda, repositoryVenda)
        {
            _mapper = mapperVenda;
            _repositoryVenda = repositoryVenda;
        }

        public async Task<List<ListaDeVendas>> BuscarVendas()
        {
            var venda = await _repositoryVenda.BuscarVendasRepo();
            if (venda == null)
            {
                throw new NullReferenceException("Ops, não foi possível buscar todos!");
            }
            var modelos = _mapper.Map<List<ListaDeVendas>>(venda);
            return modelos;
        }

        public async Task<FiltroVendaClienteViewModel> BuscaVendaPorId(int id)
        {
            var entidade = await _repositoryVenda.BuscaVendaByIdRepo(id);
            if (entidade == null)
            {
                throw new NullReferenceException("Esta entidade não foi encontrada.");
            }
            var modelo = _mapper.Map<FiltroVendaClienteViewModel>(entidade);
            return modelo;
        }

        public async Task DeletarVendaAsync(int id)
        {
            var venda = await _repositoryVenda.BuscaPorIdRepositoryT(id);
            if (venda == null)
            {
                throw new NullReferenceException("A venda a ser deletada não foi encontrada.");
            }
            await _repositoryVenda.DeleteRepositoryT(venda);
        }
    }
}
