using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repository.ProdutoRepository
{
    public class ProdutoRepository : IProdutoRepository
    {
        public readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Produto>> BuscarProduto()
        {
            var buscarProduto = _context.Produtos;
            return await buscarProduto.ToListAsync();
        }

        public async Task<Produto> BuscarProdutoId(int id)
        {
            var buscarProdutoId = await _context.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);
            return buscarProdutoId;
        }
        public async Task<Produto> AdicionarProduto(Produto produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return produto;
        }

        public async Task<Produto> AtualizarProduto(Produto produto)
        {
            _context.Update(produto);
            await _context.SaveChangesAsync();
            return produto;
        }


        public Produto DeletarProduto(int id)
        {
            var produtoId = _context.Produtos.Find(id);
            _context.Remove(produtoId);
            _context.SaveChanges();
            return produtoId;
        }

        public async Task<PagedModel<Produto>> BuscarProdutosPaginados(int pagina, int tamanhoPagina)
        {
            var query = _context.Produtos.AsQueryable();

            var totalItens = await query.CountAsync();

            var itens = await query
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();

            return new PagedModel<Produto>
            {
                PaginaAtual = pagina,
                PaginaTamanho = tamanhoPagina,
                TotalItens = totalItens,
                Itens = itens
            };
        }

    }
}