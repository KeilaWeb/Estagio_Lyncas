using Dominio.Models.DTO;

namespace Service.GenericService
{
    public interface IGenericService<T, DTO, Model> where T : class where DTO : class where Model : class
    {
        Task<List<Model>> BuscarTodosServiceT();
        Task<Model> BuscaPorIdServiceT(int id);
        Task<T> CriarServiceT(DTO entityDTO);
        Task<T> AtualizarServiceT(DTO entityDTO, int id);
    }
}
