using System.ComponentModel.DataAnnotations;

namespace Dominio.Models.DTO
{
    public class VendaDTO
    {
        [Required]
        public int ClienteId { get; set; }
        [Required]
        public DateTime DataVenda { get; set; }
        [Required]
        public DateTime DataFaturamento { get; set; }
        [Required]
        public float ValorTotalVenda { get; set; }
        [Required]
        public int QuantidadeItens { get; set; }
        public virtual List<ItemVendaDTO> Itens { get; set; }
    }
}
