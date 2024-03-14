using Dominio.Data;
using Dominio.Models.DTO;
using Microsoft.EntityFrameworkCore;
namespace Repository.Generics
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public readonly AplicacaoDbContext _context;
        public GenericRepository(AplicacaoDbContext context)
        {
            _context = context;
        }

        public async Task<List<T>> BuscarTodosRepositoryT()
        {
            return await _context.Set<T>().ToListAsync();
        }


        public async Task<T> BuscaPorIdRepositoryT(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> CriarRepositoryT(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task AtualizaRepositoryT(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRepositoryT(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
