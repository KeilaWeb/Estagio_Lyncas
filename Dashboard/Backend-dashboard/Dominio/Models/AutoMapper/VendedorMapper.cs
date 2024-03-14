using AutoMapper;
using Dominio.Models.DTO;

namespace Dominio.Models.AutoMapper
{
    public class VendedorMapper : Profile
    {
        public VendedorMapper() 
        {
            CreateMap<VendedorDTO, VendedorDTO>().ReverseMap();
        }
    }
}
