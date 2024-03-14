using AutoMapper;
using Dominio.Models.DTO;
using Repository.Generics;

namespace Service.GenericService
{
    public class GenericService<T, DTO, Model> : IGenericService<T, DTO, Model> where T : class where DTO : class where Model : class, T
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<T> _repository;
        public GenericService(IMapper mapperCliente, IGenericRepository<T> repository) : base()
        {
            _mapper = mapperCliente;
            _repository = repository;
        }

        public async Task<List<Model>> BuscarTodosServiceT()
        {
            var entidades = await _repository.BuscarTodosRepositoryT();
            if (entidades == null)
            {
                throw new NullReferenceException("Ops, não foi possível buscar todos!");
            }
            var modelos = _mapper.Map<List<Model>>(entidades);
            return modelos;
        }

        public async Task<Model> BuscaPorIdServiceT(int id)
        {
            var entidade = await _repository.BuscaPorIdRepositoryT(id);
            if (entidade == null)
            {
                throw new NullReferenceException("Esta entidade não foi encontrada.");
            }
            var modelo = _mapper.Map<Model>(entidade);
            return modelo;
        }

        public async Task<T> CriarServiceT(DTO entityDTO)
        {
            var entidade = _mapper.Map<T>(entityDTO);
            return await _repository.CriarRepositoryT(entidade);    
        }
        

        public async Task<T> AtualizarServiceT(DTO entityDTO, int id)
        {
            var entidade = await _repository.BuscaPorIdRepositoryT(id);
            if (entidade == null)
            {
                throw new NullReferenceException("A entidade a ser atualizada não foi encontrada.");
            }
            _mapper.Map(entityDTO, entidade);
            await _repository.AtualizaRepositoryT(entidade);
            return entidade;
        }        
    }
}
