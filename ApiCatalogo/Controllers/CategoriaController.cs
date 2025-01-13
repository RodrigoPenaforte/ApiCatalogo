using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ApiCatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<Categoria>>> BuscarCategoria()
        {

            var fitlrarCategoria = _context.Categorias.AsNoTracking();

            if (fitlrarCategoria is null)
            {
                return NotFound("Não foi encontrado nenhuma categoria");
            }


            return await fitlrarCategoria.ToListAsync();
        }

        [HttpGet("CategoriaProduto")]

        public async Task<ActionResult<IEnumerable<Categoria>>> BuscarCategoriaPorPrduto()
        {
            var fitlrarCategoriaPorProduto = _context.Categorias.Include(p => p.Produtos).Where( c => c.CategoriaId <=5).ToListAsync();

            if (fitlrarCategoriaPorProduto is null)
            {
                return NotFound("Não foi encontrado nenhuma categoria");
            }

            return await fitlrarCategoriaPorProduto;

        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]

        public async Task<ActionResult<Categoria>> BuscarCategoriaPorId(int id)
        {

            var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);

            if (categoria is null)
            {
                return NotFound("Não foi encontrado categoria!!");
            }

            return Ok(categoria);
        }

        [HttpPost]

        public async Task<ActionResult<Categoria>> AdicionarCategoria(Categoria categoria)
        {
            if (categoria is null)
            {
                return BadRequest("Não foi possível criar uma categoria");
            }
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("id")]
        public async Task<ActionResult<Categoria>> AtualizarCategoria(int id, Categoria categoria)
        {

            if (id != categoria.CategoriaId)
            {
                return NotFound("Não foi possível atualizar a categoria");
            }

            _context.Update(categoria);
            await _context.SaveChangesAsync();

            return categoria;
        }

        [HttpDelete("id")]

        public async Task<ActionResult<Categoria>> DeletarCategoria(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);

            if (categoria is null)
            {
                return NotFound("Categoria não pode ser excluída");
            }

            _context.Remove(categoria);
            await _context.SaveChangesAsync();

            return Ok("Foi excluído com sucesso...");

        }

    }
}