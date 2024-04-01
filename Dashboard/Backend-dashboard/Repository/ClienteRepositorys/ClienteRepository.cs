using Dominio.Data;
using Dominio.Models.DTO;
using Dominio.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Generics;

namespace Repository.ClienteRepositorys
{
    public class ClienteRepository : GenericRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(AplicacaoDbContext context) : base(context)
        {         }

        public async Task<(List<Cliente>, int)> PaginarClientesRepo(Paginacao paginacao)
        {
            var query = _context.Set<Cliente>().AsQueryable();
            if (!string.IsNullOrEmpty(paginacao.Busca))
            {
                query = query.Where(c => c.Nome.Contains(paginacao.Busca) || c.Email.Contains(paginacao.Busca));
            }
            var totalClientes = await query.CountAsync();

            var clientesPaginados = await query
                .Skip((paginacao.PaginaNumero - 1) * paginacao.PaginaQuantidade)
                .Take(paginacao.PaginaQuantidade)
                .ToListAsync();
            return (clientesPaginados, totalClientes);
        }

        public async Task<Cliente> BuscarPorIdRepo(int id)
        {
            return await _context.Set<Cliente>()
                        .Include(v => v.Vendas)
                        .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
