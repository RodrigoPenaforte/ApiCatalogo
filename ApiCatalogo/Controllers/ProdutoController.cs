using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Services.ProdutoService;
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

        private readonly ProdutoService _produtoService;

        public ProdutoController(ProdutoService produtoService)
        {
            _produtoService = produtoService;

        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> BuscarProduto()
        {

            var buscarTodosOsProdutos = await _produtoService.BuscarProduto();
            return Ok(buscarTodosOsProdutos);

        }


        [HttpGet("{id}", Name = "ObterProduto")]

        public async Task<ActionResult<Produto>> BuscarProdutoId(int id)
        {
            return await _produtoService.BuscarProdutoId(id);
        }


        [HttpPost]

        public async Task<ActionResult> AdicionarProduto(Produto produto)
        {
            var adicionarProduto = await _produtoService.AdicionarProduto(produto);
            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, adicionarProduto);

        }

        [HttpPut]

        public async Task<ActionResult<Produto>> AtualizarProduto(Produto produto)
        {
            var atualizar = await _produtoService.AtualizarProduto(produto);
            return Ok(atualizar);

        }

        [HttpDelete("{id}")]

        public async Task<ActionResult<Produto>> ExcluirProduto(int id)
        {
            return await _produtoService.DeletarProduto(id);
        }

    }
}