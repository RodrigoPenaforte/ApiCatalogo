using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.Models;

namespace ApiCatalogo.Repository.ProdutoRepository
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produto>> BuscarProduto();
        Task<Produto> BuscarProdutoId(int id);
        Task<Produto> AdicionarProduto(Produto produto);
        Task<Produto> AtualizarProduto(Produto produto);
        Produto DeletarProduto(int id);

    }
}