using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using NerdStore.Catalogo.Application.ViewModels;
using NerdStore.Catalogo.Domain;

namespace NerdStore.Catalogo.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            //Onde eu tenho largura no ProdutoViewModel eu vou mapear de Produto.Dimenssoes.Largura assim para os demais campos
            CreateMap<Produto, ProdutoViewModel>()
                .ForMember(d => d.Largura, o => o.MapFrom(s => s.Dimensoes.Largura))
                .ForMember(d => d.Altura, o => o.MapFrom(s => s.Dimensoes.Altura))
                .ForMember(d => d.Profundidade, o => o.MapFrom(s => s.Dimensoes.Profundidade));

            //Todos os campos de categoria serão mapeados para categoria view model
            CreateMap<Categoria, CategoriaViewModel>();
        }
    }
}
