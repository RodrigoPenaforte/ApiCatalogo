using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.DTOs.Produtos;
using ApiCatalogo.Models;

namespace ApiCatalogo.DTOs.Categorias
{
    public class CategoriaOutputDTO
    {
        public int CategoriaId { get; set; }
        public string? Nome { get; set; }
        public string? ImagemUrl { get; set; }
        public ICollection<ProdutoOutputDTO>? ProdutoOutputDTO { get; set; }

    }
}