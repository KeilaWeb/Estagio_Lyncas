using System.ComponentModel.DataAnnotations;

namespace Dominio.Models.ViewModels
{
    public class BuscarItemVendaViewModel
    {
        [Required]
        public int Id { get; set; }
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
