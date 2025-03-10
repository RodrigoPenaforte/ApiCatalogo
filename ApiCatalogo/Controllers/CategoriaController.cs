using ApiCatalogo.DTOs.Categorias;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Services.CategoriaService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly CategoriaService _categoriaService;
        private readonly ILogger<CategoriaController> _logger;
        private readonly IMapper _mapper;

        public CategoriaController(CategoriaService categoriaService, ILogger<CategoriaController> logger, IMapper mapper)
        {
            _categoriaService = categoriaService;
            _logger = logger;
            _mapper = mapper;
        }


        [HttpGet]
        [Authorize(Policy = "UserOnly")]
        public async Task<ActionResult<CategoriaDTO>> BuscarCategoria()
        {
            var categoria = await _categoriaService.BuscarTodos();
            return Ok(categoria);
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet("CategoriaProduto")]
        public async Task<ActionResult<IEnumerable<CategoriaOutputDTO>>> BuscarCategoriaPorProduto(int id)
        {
            var categoriaPorProduto = await _categoriaService.BuscarCategoriaPorProduto(id);
            var categoriaDto = _mapper.Map<IEnumerable<CategoriaOutputDTO>>(categoriaPorProduto);
            if (categoriaDto == null)
            {
                _logger.LogError("Não foi possível encontrar Categoria por produtos");
            }
            return Ok(categoriaDto);
        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet("paginados")]
        public async Task<ActionResult<PagedModel<CategoriaDTO>>> BuscarCategoriasPaginados(int pagina = 1, int tamanhoPagina = 5)
        {
            var pagedCategorias = await _categoriaService.BuscarCategoriasPaginados(pagina, tamanhoPagina);

            var pagedCategoriaDto = new PagedModel<CategoriaDTO>
            {
                PaginaAtual = pagedCategorias.PaginaAtual,
                PaginaTamanho = pagedCategorias.PaginaTamanho,
                TotalItens = pagedCategorias.TotalItens,
                Itens = _mapper.Map<IList<CategoriaDTO>>(pagedCategorias.Itens)
            };

            return Ok(pagedCategoriaDto);

        }

        [Authorize(Policy = "UserOnly")]
        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> BuscarCategoriaPorId(int id)
        {
            var categoriaPorId = await _categoriaService.BuscarCategoriaPorId(id);

            //Destino é o CategoriaDTO e a origem é categoriaPorId
            var categoriaDto = _mapper.Map<CategoriaDTO>(categoriaPorId);
            if (categoriaDto is null)
            {
                _logger.LogError("Não foi possível encontrar o id de categoria");
            }

            return Ok(categoriaDto);

        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult<CategoriaOutputDTO>> AdicionarCategoria(CategoriaInputDTO categoriaInputDto)
        {

            var categoriaMapeada = _mapper.Map<Categoria>(categoriaInputDto);
            var categoriaAdicionar = await _categoriaService.AdicionarCategoria(categoriaMapeada);

            if (categoriaAdicionar is null)
            {
                _logger.LogError("Não foi possível adicionar uma categoria");
            }

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaAdicionar?.CategoriaId }, categoriaAdicionar);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("id")]
        public async Task<ActionResult<CategoriaOutputDTO>> AtualizarCategoria(int id, CategoriaInputDTO categoriaInput)
        {

            var categoriaMapeada = _mapper.Map<Categoria>(categoriaInput);

            var categoriaAtualizada = await _categoriaService.AtualizarCategoria(id, categoriaMapeada);

            if (categoriaAtualizada is null)
            {
                _logger.LogError("Não foi possível atualizar categoria");
            }

            var categoriaOutPut = _mapper.Map<CategoriaOutputDTO>(categoriaAtualizada);

            return Ok(categoriaOutPut);
        }
        [Authorize(Policy = "SuperAdminOnly")]
        [HttpDelete("id")]
        public async Task<ActionResult<CategoriaDTO>> DeletarCategoria(int id)
        {
            var categoriaDeletado = await _categoriaService.DeletarCategoria(id);

            return _mapper.Map<CategoriaDTO>(categoriaDeletado);
        }

    }
}