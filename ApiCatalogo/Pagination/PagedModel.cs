using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogo.Pagination
{
    public class PagedModel<TModel>
    {
        private const int MaxPaginas = 5;
        private int _tamPaginas;

        public int PaginaTamanho
        {
            get => _tamPaginas;
            set => _tamPaginas = (value > MaxPaginas) ? MaxPaginas : value;
        }

        public int PaginaAtual { get; set; }
        public int TotalItens { get; set; }

        public int TotalPaginas => (int)Math.Ceiling((double)TotalItens / PaginaTamanho);

        public IList<TModel> Itens { get; set; }

        public PagedModel()
        {
            Itens = [];
        }
    }

}