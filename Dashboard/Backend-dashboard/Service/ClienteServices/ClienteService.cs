using Dominio.Models.DTO;
using Dominio.Models.Entities;
using AutoMapper;
using Service.GenericService;
using Repository.ClienteRepositorys;
using Dominio.Models.ViewModelCliente;

namespace Service.ClienteServices
{
    public class ClienteService : GenericService<Cliente, ClienteDTO, Cliente>, IGenericService
    {
        private readonly IClienteRepository _repositoryCliente;
        private readonly IMapper _mapper;
        public ClienteService(IMapper mapperCliente, IClienteRepository clienteRepository) 
            : base(mapperCliente, clienteRepository)
        {
            _repositoryCliente = clienteRepository;
            _mapper = mapperCliente;
        }

        public async Task<(List<ClienteBuscaTodos>, int)> PaginarCliente(Paginacao paginacao)
        {
            try
            {
                var (clientesPaginados, totalClientes) = await _repositoryCliente.PaginarClientesRepo(paginacao);
                var clientes = _mapper.Map<List<ClienteBuscaTodos>>(clientesPaginados);
                return (clientes, totalClientes);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao paginar clientes", ex);
            }
        }

        public async Task DeletarClienteAsync(int id)
        {
            var cliente = await _repositoryCliente.BuscarPorIdRepo(id);
            if (cliente.Vendas == null)
            {
                throw new DirectoryNotFoundException("Cliente não encontrado.");
            }
            if (cliente.Vendas != null && cliente.Vendas.Any())
            {
                // Se o cliente tiver vendas associadas, marcar como excluído e manter um registro de auditoria
                cliente.Deletado = true;
                cliente.DataExclusao = DateTime.Now;
                await _repositoryCliente.AtualizaRepositoryT(cliente);
            }
            else
            {
                // Se o cliente não tiver vendas associadas, excluí-lo diretamente
                await _repositoryCliente.DeleteRepositoryT(cliente);
            }
        }

    }
}
