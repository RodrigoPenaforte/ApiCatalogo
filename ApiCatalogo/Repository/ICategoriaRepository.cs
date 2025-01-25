using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.Models;

namespace ApiCatalogo.Repository
{
    public interface ICategoriaRepository
    {
        Task<IEnumerable<Categoria>> BuscarCategoria();
        Task<IEnumerable<Categoria>> BuscarCategoriaPorProduto();
        Task<Categoria> BuscarCategoriaPorId(int id);
        Task<Categoria> AdicionarCategoria(Categoria categoria);
        Task<Categoria> AtualizarCategoria( int id, Categoria categoria);
        Categoria DeletarCategoria(int id);

    }
}