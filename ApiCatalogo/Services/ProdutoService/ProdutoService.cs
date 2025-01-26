using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.Models;
using ApiCatalogo.Repository.ProdutoRepository;

namespace ApiCatalogo.Services.ProdutoService
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;

        }

        public Task<IEnumerable<Produto>> BuscarProduto()
        {
            var buscaTodosProdutos = _produtoRepository.BuscarProduto();
            if (buscaTodosProdutos is null)
                throw new NotImplementedException("Produto não foi encontrado");

            return buscaTodosProdutos;
        }

        public async Task<Produto> BuscarProdutoId(int id)
        {
            var buscarProdutoPorId = _produtoRepository.BuscarProdutoId(id);
            if (id == 0)
            {
                throw new NotImplementedException("Id não encontrado do produto");
            }
            return await buscarProdutoPorId;
        }
        public Task<Produto> AdicionarProduto(Produto produto)
        {
            var AdicionarProduto = _produtoRepository.AdicionarProduto(produto);
            if (AdicionarProduto is null)
            {
                throw new NotImplementedException("Não foi possível adicionar produto");

            }
            return AdicionarProduto;
        }

        public async Task<Produto> AtualizarProduto( Produto produto)
        {
            if (produto is null)
            {
                throw new Exception("Não foi possível atualizar a produto");

            }
            return await _produtoRepository.AtualizarProduto(produto);

        }

        public async Task<Produto> DeletarProduto(int id)
        {
            if (id != 0)
            {
                return _produtoRepository.DeletarProduto(id);

            }
            throw new NotImplementedException("Não foi possível atualizar o id");
        }
    }
};