using ApiCatalogo.Models;
using ApiCatalogo.Repository;


namespace ApiCatalogo.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;

        }

        public async Task<IEnumerable<Categoria>> BuscarTodos()
        {
            var categorias = await _categoriaRepository.BuscarCategoria();

            if (categorias is null)
            {
                throw new Exception("Não foi possível encontrar categorias");
            }

            return categorias;
        }
        public async Task<IEnumerable<Categoria>> BuscarCategoriaPorProduto()
        {
            var fitrarCategoriaPorProduto = _categoriaRepository.BuscarCategoriaPorProduto();

            if (fitrarCategoriaPorProduto == null)
            {
                throw new Exception("Não foi possível encontrar produto dentro de categorias");

            }
            return await fitrarCategoriaPorProduto;
        }

        public async Task<Categoria> BuscarCategoriaPorId(int id)
        {
            var filtrarCategoriaPorId = _categoriaRepository.BuscarCategoriaPorId(id);
            if (filtrarCategoriaPorId is null)
            {
                throw new Exception("Não foi encontrado o id de categoria");
            }
            return await filtrarCategoriaPorId;
        }

        public async Task<Categoria> AdicionarCategoria(Categoria categoria)
        {
            var adicionarCategoria = _categoriaRepository.AdicionarCategoria(categoria);
            if (adicionarCategoria is null)
            {
                throw new Exception("Não foi possível criar uma categoria");
            }
            return await adicionarCategoria;
        }

        public async Task<Categoria> AtualizarCategoria(int id, Categoria categoria)
        {
            if (categoria is null)
            {
                throw new Exception("Não foi possível atualizar a categoria");

            }
            var atualizarCategoria = await _categoriaRepository.AtualizarCategoria(id, categoria);
            return atualizarCategoria;

        }

        public async Task<Categoria> DeletarCategoria(int id)
        {
            if (id != 0)
            {
                var deletarCategoria =  _categoriaRepository.DeletarCategoria(id);
                return deletarCategoria;
            }
            throw new Exception("Não foi possível deletar uma categoria");

        }
    }
}