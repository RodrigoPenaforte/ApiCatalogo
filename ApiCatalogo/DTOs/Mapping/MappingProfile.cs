using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.DTOs.Categorias;
using ApiCatalogo.DTOs.Produtos;
using ApiCatalogo.Models;
using AutoMapper;

namespace ApiCatalogo.DTOs.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            // Mapeamentos para DTOs de Produto
            CreateMap<Produto, ProdutoDTO>().ReverseMap();
            CreateMap<Produto, ProdutoInputDTO>().ReverseMap();
            CreateMap<Produto, ProdutoOutputDTO>().ReverseMap();

            // Mapeamentos para DTOs de Categoria
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            CreateMap<Categoria, CategoriaInputDTO>().ReverseMap();
            CreateMap<Categoria, CategoriaOutputDTO>().ReverseMap();
        }
    }
}