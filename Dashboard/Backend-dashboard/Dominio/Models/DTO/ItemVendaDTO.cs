

using System.ComponentModel.DataAnnotations;

namespace Dominio.Models.DTO
{
    public class ItemVendaDTO
    {
        [Required]
        public string DescricaoItem { get; set; }
        [Required]
        public float PrecoUnitario { get; set; }
        [Required]
        public int Quantidade { get; set; }
        [Required]
        public float ValorTotal { get; set; }
    }
}
