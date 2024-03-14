using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Dominio.Models.Entities
{
    public class ItemVenda : BaseEntity
    {
        public string DescricaoItem { get; set; }
        public float PrecoUnitario { get; set; }
        public int Quantidade { get; set; }
        [JsonIgnore]
        public virtual Venda Venda { get; set; }
        public int VendaId { get; set; }
        public float ValorTotal { get; set; }
    }
}