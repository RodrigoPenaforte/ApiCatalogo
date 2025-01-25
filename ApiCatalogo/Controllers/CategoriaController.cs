using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Services;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ApiCatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly CategoriaService _categoriaService;
        private readonly ILogger<CategoriaController> _logger;

        public CategoriaController(CategoriaService categoriaService, ILogger<CategoriaController> logger)
        {
            _categoriaService = categoriaService;
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult> BuscarCategoria()
        {
            var categoria = await _categoriaService.BuscarTodos();
            return Ok(categoria);
        }


        [HttpGet("CategoriaProduto")]
        public async Task<ActionResult<IEnumerable<Categoria>>> BuscarCategoriaPorPrduto()
        {
            var categoriaPorProduto = await _categoriaService.BuscarCategoriaPorProduto();
            return Ok(categoriaPorProduto);
        }


        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public async Task<ActionResult<Categoria>> BuscarCategoriaPorId(int id)
        {
            var categoriaPorId = await _categoriaService.BuscarCategoriaPorId(id);
            return Ok(categoriaPorId);

        }

        [HttpPost]

        public async Task<ActionResult<Categoria>> AdicionarCategoria(Categoria categoria)
        {
            var categoriaAdicionar = await _categoriaService.AdicionarCategoria(categoria);

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoriaAdicionar);
        }

        [HttpPut("id")]
        public async Task<ActionResult<Categoria>> AtualizarCategoria( int id, Categoria categoria)
        {
            return await _categoriaService.AtualizarCategoria(id, categoria);
        }

        [HttpDelete("id")]

        public async Task<ActionResult<Categoria>> DeletarCategoria(int id)
        {
           return await _categoriaService.DeletarCategoria(id);

        }

    }
}