using Dominio.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dominio.Data
{
    public class AplicacaoDbContext : DbContext
    {
        public AplicacaoDbContext(DbContextOptions<AplicacaoDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ItemVenda> Itens{ get; set; }
        public DbSet<Venda> Vendas{ get; set; }
        public DbSet<Vendedor> Vendedores { get; set; }

    }
}
