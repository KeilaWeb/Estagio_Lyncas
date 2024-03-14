using Dominio.Models.DTO;
using Dominio.Models.Entities;
using Repository.Generics;

namespace Repository.ClienteRepositorys
{
    public interface IClienteRepository : IGenericRepository<Cliente>
    {
        Task<(List<Cliente>, int)> PaginarClientesRepo(Paginacao paginacao);
        Task<Cliente> BuscarPorIdRepo(int id);
    }
}
