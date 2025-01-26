using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.Models;

namespace ApiCatalogo.Services.CategoriaService
{
    public interface ICategoriaService
    {
        Task<IEnumerable<Categoria>> BuscarTodos();
        Task<IEnumerable<Categoria>> BuscarCategoriaPorProduto();
        Task<Categoria> BuscarCategoriaPorId(int id);
        Task<Categoria> AdicionarCategoria(Categoria categoria);
        Task<Categoria> AtualizarCategoria(int id, Categoria categoria);
        Task<Categoria> DeletarCategoria(int id);

    }
}