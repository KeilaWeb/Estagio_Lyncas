using Dominio.Data;
using Dominio.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Generics;

namespace Repository.VendasRepositorys
{
    public class VendaRepository : GenericRepository<Venda>, IVendaRepository
    {
        public VendaRepository(AplicacaoDbContext context) : base(context) { }


        public async Task<IEnumerable<Venda>> BuscarVendasRepo()
        {
            return await _context.Vendas
                        .Include(x => x.Cliente)
                        .ToListAsync();
        }

        public async Task<Venda> BuscaVendaByIdRepo(int id)
        {
            return await _context.Vendas
                        .Include(x => x.Cliente)
                        .Include(x => x.Itens)
                        .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Venda> DeleteVendaRepo(int id)
        {
            var venda = await BuscaVendaByIdRepo(id);
            if (venda != null)
            {
                _context.Vendas.Remove(venda);
                await _context.SaveChangesAsync();
            }
            return venda;
        }
    }
}

