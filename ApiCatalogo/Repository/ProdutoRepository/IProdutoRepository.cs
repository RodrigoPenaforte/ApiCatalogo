using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Repository.ProdutoRepository
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produto>> BuscarProduto();
        Task<Produto> BuscarProdutoId(int id);
        Task<Produto> AdicionarProduto(Produto produto);
        Task<Produto> AtualizarProduto(Produto produto);
        Produto DeletarProduto(int id);
        Task<PagedModel<Produto>> BuscarProdutosPaginados(int pagina, int tamanhoPagina);


    }
}