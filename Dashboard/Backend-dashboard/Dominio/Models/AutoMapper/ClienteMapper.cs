using AutoMapper;
using Dominio.Models.DTO;
using Dominio.Models.Entities;
using Dominio.Models.ViewModelCliente;

namespace Dominio.Models.AutoMapper
{
    public class ClienteMapper : Profile
    {
        public ClienteMapper()
        {
            CreateMap<Cliente, ClienteDTO>();
            CreateMap<ClienteDTO, Cliente>();

            CreateMap<Cliente, ClienteBuscaTodos>();
            CreateMap<ClienteBuscaTodos, Cliente>();
            
            CreateMap<ClienteDTO, ClienteBuscaTodos>();
            CreateMap<ClienteBuscaTodos, ClienteDTO>();
        }
    }
}
