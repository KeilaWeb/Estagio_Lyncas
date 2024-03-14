
using Dominio.Models.DTO;

namespace Repository.Generics
{
    public interface IGenericRepository<T> where T : class 
    {
        Task<List<T>> BuscarTodosRepositoryT();
        Task<T> BuscaPorIdRepositoryT(int id);
        Task<T> CriarRepositoryT(T entity);
        Task AtualizaRepositoryT(T entity);
        Task DeleteRepositoryT(T entity);
    }
}
