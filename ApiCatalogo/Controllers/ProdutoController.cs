using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ApiCatalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutoController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> BuscarProduto()
        {

            var filtrarProduto = _context.Produtos;

            if (filtrarProduto is null)
            {
                return NotFound("Produto Vazio");
            }
            return await filtrarProduto.ToListAsync();

        }


        [HttpGet("{id}", Name = "ObterProduto")]

        public ActionResult<Produto> BuscarProdutoId(int id)
        {
            var buscarIdProduto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            if (buscarIdProduto is null)
            {
                return NotFound("Id não foi encontrado..");
            }

            return buscarIdProduto;
        }


        [HttpPost]

        public ActionResult AdicionarProduto(Produto produto)
        {
            if (produto is null)
                return BadRequest();

            _context.Produtos.Add(produto);
            _context.SaveChanges();
            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);

        }

        [HttpPut("{id}")]

        public async Task<ActionResult<Produto>> AtualizarProduto(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest("Produto não foi atualizado");
            }

            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();

            return Ok(produto);

        }

        [HttpDelete("{id}")]

        public async Task<ActionResult<Produto>> ExcluirProduto(int id)
        {
            var selecionarProduto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            if(selecionarProduto is null)
                return NotFound("Produto não pode ser encontrado e deletado");
                
            _context.Produtos.Remove(selecionarProduto);
            _context.SaveChanges();

            return Ok();

        }

    }
}