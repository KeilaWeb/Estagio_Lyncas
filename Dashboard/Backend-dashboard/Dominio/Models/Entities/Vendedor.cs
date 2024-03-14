using System.ComponentModel.DataAnnotations;

namespace Dominio.Models.Entities
{
    public class Vendedor : BaseEntity
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
