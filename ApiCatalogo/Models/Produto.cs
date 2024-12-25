using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Models
{
    public class Produto
    {
        public int CategoriaId { get; set;}
        public int ProdutoId { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public string? ImagemUrl { get; set; }
        public float Estoque { get; set;}
        public DateTime DataCadastro { get; set; }
        [JsonIgnore] 
        public Categoria? Categoria { get; set; }


    }
}