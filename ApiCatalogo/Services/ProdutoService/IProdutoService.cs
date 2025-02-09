using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Services.ProdutoService
{
    public interface IProdutoService
    {
        Task<IEnumerable<Produto>> BuscarProduto();
        Task<PagedModel<Produto>> BuscarProdutosPaginados(int pagina, int tamanhoPagina);
        Task<Produto> BuscarProdutoId(int id);
        Task<Produto> AdicionarProduto(Produto produto);
        Task<Produto> AtualizarProduto(Produto produto);
        Task<Produto> DeletarProduto(int id);


    }
}