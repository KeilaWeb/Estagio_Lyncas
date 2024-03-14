using Dominio.Models.Entities;
using Repository.Generics;

namespace Repository.VendasRepositorys
{
    public interface IVendaRepository : IGenericRepository<Venda>
    {
        Task<IEnumerable<Venda>> BuscarVendasRepo();
        Task<Venda> BuscaVendaByIdRepo(int id);
    }
}
