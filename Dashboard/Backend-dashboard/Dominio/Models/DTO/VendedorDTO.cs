namespace Dominio.Models.DTO
{
    public class VendedorDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public VendaDTO VendaId { get; set; }
        public ClienteDTO ClienteId { get; set; }
    }
}
