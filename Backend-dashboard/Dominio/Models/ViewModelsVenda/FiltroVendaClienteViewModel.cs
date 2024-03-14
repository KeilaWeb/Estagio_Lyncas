using System.ComponentModel.DataAnnotations;

namespace Dominio.Models.ViewModels
{
    public class FiltroVendaClienteViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int ClienteId { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public int QuantidadeItens { get; set; }
        [Required]
        public DateTime DataVenda { get; set; }
        [Required]
        public DateTime DataFaturamento { get; set; }
        [Required]
        public double ValorTotalVenda { get; set; }
        public List<BuscarItemVendaViewModel> Itens { get; set; }
    }
}
