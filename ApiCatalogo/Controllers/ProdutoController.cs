using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogo.Context;
using ApiCatalogo.DTOs.Produtos;
using ApiCatalogo.Models;
using ApiCatalogo.Services.ProdutoService;
using AutoMapper;
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
        private readonly ILogger<ProdutoController> _logger;
        private readonly IMapper _mapper;


        public ProdutoController(ProdutoService produtoService, ILogger<ProdutoController> logger, IMapper mapper)
        {
            _produtoService = produtoService;
            _logger = logger;
            _mapper = mapper;

        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> BuscarProduto()
        {

            var produtos = await _produtoService.BuscarProduto();

            var produtoDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            if (produtoDto is null)
            {
                _logger.LogError(" Os Produtos não foram encontrado");
            }
            return Ok(produtoDto);

        }


        [HttpGet("{id}", Name = "ObterProduto")]

        public async Task<ActionResult<ProdutoDTO>> BuscarProdutoId(int id)
        {
            var buscarProdutoId = await _produtoService.BuscarProdutoId(id);

            var produtoDto = _mapper.Map<ProdutoDTO>(buscarProdutoId);
            if (produtoDto is null)
            {
                _logger.LogError("Não foi possível achar o produto pelo id");
            }

            return Ok(produtoDto);

        }


        [HttpPost]

        public async Task<ActionResult<ProdutoOutputDTO>> AdicionarProduto(ProdutoInputDTO produtoDto)
        {
            var produtoMapeado = _mapper.Map<Produto>(produtoDto);
            var adicionarProduto = await _produtoService.AdicionarProduto(produtoMapeado);
            if (adicionarProduto is null)
                _logger.LogError("Produto não pode ser adicionado");

            return new CreatedAtRouteResult("ObterProduto", new { id = adicionarProduto?.ProdutoId }, adicionarProduto);

        }

        [HttpPut]

        public async Task<ActionResult<ProdutoOutputDTO>> AtualizarProduto(ProdutoInputDTO produtoDto)
        {
            var produtoMapeado = _mapper.Map<Produto>(produtoDto);
            var atualizarProduto = await _produtoService.AtualizarProduto(produtoMapeado);

            if (atualizarProduto is null)
                _logger.LogError("Não foi possível atualizar o produto");

            return Ok(atualizarProduto);

        }

        [HttpDelete("{id}")]

        public async Task<ActionResult<ProdutoDTO>> ExcluirProduto(int id)
        {
            var produtoDeletado =  await _produtoService.DeletarProduto(id);
            return _mapper.Map<ProdutoDTO>(produtoDeletado);
        }

    }
}