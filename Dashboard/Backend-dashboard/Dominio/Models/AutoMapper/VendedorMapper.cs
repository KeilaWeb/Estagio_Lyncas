using AutoMapper;
using Dominio.Models.DTO;
using Dominio.Models.Entities;
using Dominio.Models.ViewModelVendedor;

namespace Dominio.Models.AutoMapper
{
    public class VendedorMapper : Profile
    {
        public VendedorMapper() 
        {
            CreateMap<Vendedor, ClienteDTO>().ReverseMap();
            CreateMap<VendedorBusca, Vendedor>().ReverseMap();
            CreateMap< ClienteDTO, VendedorBusca>().ReverseMap();
        }
    }
}
