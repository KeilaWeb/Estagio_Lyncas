using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Dominio.Models.Entities
{
    public class Venda : BaseEntity
    {
        public int ClienteId { get; set; }
        public virtual Cliente Cliente { get; set; }
        public DateTime DataFaturamento { get; set; }
        public DateTime DataVenda { get; set; }
        public float ValorTotalVenda { get; set; }
        public int QuantidadeItens { get; set; }
        [JsonIgnore]
        public virtual List<ItemVenda> Itens { get; set; }
 
    }
}
