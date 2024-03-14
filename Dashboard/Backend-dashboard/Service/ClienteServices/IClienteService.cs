using Dominio.Models.DTO;
using Dominio.Models.Entities;
using Dominio.Models.ViewModelCliente;
using Service.GenericService;

namespace Service.ClienteServices
{
    public interface IClienteService : IGenericService<Cliente, ClienteDTO, Cliente>
    {
        Task<(List<ClienteBuscaTodos>, int)> PaginarCliente(Paginacao paginacao);
        Task DeletarClienteAsync(int id);
    }
}
