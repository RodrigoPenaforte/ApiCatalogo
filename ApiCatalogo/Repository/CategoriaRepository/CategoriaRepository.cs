using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repository.CategoriaRepository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categoria>> BuscarCategoria()
        {
            return await _context.Categorias.ToListAsync();
        }

        public async Task<IQueryable<Categoria>> BuscarCategoriaPaginado()
        {
           return await Task.FromResult(_context.Categorias.AsQueryable());
        }
        public async Task<IEnumerable<Categoria>> BuscarCategoriaPorProduto(int id)
        {
            var categoriaPorProduto = _context.Categorias.Where(c => c.CategoriaId == id).Include(c => c.Produtos).ToListAsync();
            return await categoriaPorProduto;
        }

        public async Task<Categoria> BuscarCategoriaPorId(int id)
        {
            var categoriaPorId = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);
            return categoriaPorId;
        }

        public async Task<Categoria> AdicionarCategoria(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }

        public async Task<Categoria> AtualizarCategoria(int id, Categoria categoria)
        {
            _context.Update(categoria);
            await _context.SaveChangesAsync();
            return categoria;

        }

        public Categoria DeletarCategoria(int id)
        {
            var categoriaId = _context.Categorias.Find(id);
            _context.Remove(categoriaId);
            _context.SaveChanges();
            return categoriaId;
        }

    }
}