using AutoMapper;
using Dominio.Models.DTO;
using Dominio.Models.Entities;
using Dominio.Models.ViewModels;

namespace Dominio.Models.AutoMapper
{
    public class VendaMapper : Profile
    {
        public VendaMapper()
        {

            CreateMap<VendaDTO, ListaDeVendas>();
            CreateMap<ListaDeVendas, VendaDTO>();
            CreateMap<ListaDeVendas, Venda>();
            CreateMap<Venda, ListaDeVendas>()
                    .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Cliente.Nome));

            CreateMap<VendaDTO, FiltroVendaClienteViewModel>();
            CreateMap<FiltroVendaClienteViewModel, VendaDTO>();
            CreateMap<Venda, FiltroVendaClienteViewModel>()
                    .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Cliente.Nome));
            CreateMap<FiltroVendaClienteViewModel, Venda>();

            CreateMap<BuscarItemVendaViewModel, ItemVendaDTO>();
            CreateMap<ItemVendaDTO, BuscarItemVendaViewModel>();
            CreateMap<ItemVenda, BuscarItemVendaViewModel>();
            CreateMap<BuscarItemVendaViewModel, ItemVenda>();
            CreateMap<BuscarItemVendaViewModel, ListaDeVendas>();
            CreateMap<ListaDeVendas, BuscarItemVendaViewModel>();

            CreateMap<ItemVenda, BuscarItemVendaViewModel>();
            CreateMap<BuscarItemVendaViewModel, ItemVenda>();
            CreateMap<BuscarItemVendaViewModel, ItemVendaDTO>();
            CreateMap<ItemVendaDTO, BuscarItemVendaViewModel>();

            CreateMap<ItemVendaDTO, ItemVenda>();
            CreateMap<ItemVenda, ItemVendaDTO>();

            CreateMap<VendaDTO, Venda>();
            CreateMap<Venda, VendaDTO>();
        }
    }
}
