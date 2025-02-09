using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;

namespace ApiCatalogo.Services.CategoriaService
{
    public interface ICategoriaService
    {
        Task<IEnumerable<Categoria>> BuscarTodos();
        Task<PagedModel<Categoria>> BuscarCategoriasPaginados(int pagina, int tamanhoPagina);
        Task<IEnumerable<Categoria>> BuscarCategoriaPorProduto(int id);
        Task<Categoria> BuscarCategoriaPorId(int id);
        Task<Categoria> AdicionarCategoria(Categoria categoria);
        Task<Categoria> AtualizarCategoria(int id, Categoria categoria);
        Task<Categoria> DeletarCategoria(int id);

    }
}