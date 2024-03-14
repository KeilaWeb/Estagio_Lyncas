using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Dominio.Models.Entities
{
    public class Cliente : BaseEntity
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string CPF { get; set; }
        public bool Deletado { get; set; }
        public DateTime? DataExclusao { get; set; } = DateTime.Now;
        [JsonIgnore]
        public virtual List<Venda> Vendas { get; set; }
    }
}
