using Dominio.Models.ApplicationUser;
using Dominio.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dominio.Data
{
    public class AplicacaoDbContext : IdentityDbContext<ApplicationUser>
    {
        public AplicacaoDbContext(DbContextOptions<AplicacaoDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ItemVenda> Itens{ get; set; }
        public DbSet<Venda> Vendas{ get; set; }
        public DbSet<Vendedor> Vendedores { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
