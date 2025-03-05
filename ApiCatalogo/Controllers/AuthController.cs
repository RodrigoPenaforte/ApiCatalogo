using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApiCatalogo.DTOs.LoginModel;
using ApiCatalogo.DTOs.RegistroModel;
using ApiCatalogo.DTOs.Response;
using ApiCatalogo.DTOs.TokenModel;
using ApiCatalogo.Models.ApplicationUser;
using ApiCatalogo.Services.TokenService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ITokenService tokenService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var usuario = await _userManager.FindByNameAsync(model.NomeUsuario!);

            if (usuario is not null && await _userManager.CheckPasswordAsync(usuario, model.Senha!))
            {

                var userRoles = await _userManager.GetRolesAsync(usuario);

                var authClaims = new List<Claim>{
                    new Claim(ClaimTypes.Name, usuario.UserName!),
                    new Claim(ClaimTypes.Email, usuario.Email!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var roles in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, roles));

                }
                var token = _tokenService.GenerateAccessToken(authClaims);
                var refreshToken = _tokenService.GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);

                usuario.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);

                usuario.RefreshToken = refreshToken;

                await _userManager.UpdateAsync(usuario);

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo,

                });

            }

            return Unauthorized();

        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] RegistroModel model)
        {
            var usuarioExiste = await _userManager.FindByNameAsync(model.NomeUsuario!);

            if (usuarioExiste is not null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response
                {
                    Status = "Error",
                    Mensagem = "Usuário já existe!"
                });
            }

            ApplicationUser usuario = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.NomeUsuario
            };

            var resultado = await _userManager.CreateAsync(usuario, model.Senha!);

            if (!resultado.Succeeded)
            {
                var mensagensErro = new Dictionary<string, string>
                {
                    { "must have at least one uppercase", "A senha deve conter pelo menos uma letra maiúscula." },
                    { "must have at least one non alphanumeric character", "A senha deve conter pelo menos um caractere especial (!, @, #, etc.)." },
                    { "must have at least one digit", "A senha deve conter pelo menos um número." }
                };

                var erroEncontrado = resultado.Errors
                    .Select(e => mensagensErro.FirstOrDefault(m => e.Description.Contains(m.Key)).Value)
                    .FirstOrDefault(m => m != null);

                if (erroEncontrado is not null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new Response
                    {
                        Status = "Error",
                        Mensagem = $"Usuário {usuario.UserName} não foi criado! {erroEncontrado}"
                    });
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Error",
                    Mensagem = "Usuário não foi criado! Verifique os requisitos da senha."
                });
            }

            return Ok(new Response { Status = "Success", Mensagem = "Usuário criado com sucesso!" });
        }



        [HttpPost]
        [Route("refresh-token")]

        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Resposta do cliente inválida");
            }

            string? acessoToken = tokenModel.AcessoToken ?? throw new ArgumentNullException(nameof(tokenModel));
            string? tokenAtualizar = tokenModel.AtualizarToken ?? throw new ArgumentNullException(nameof(tokenModel));

            var principal = _tokenService.GetPrincipalFromExpiredToken(acessoToken!);

            if (principal is null)
            {
                return BadRequest("Acesso ao token inválido, Não pode atualizar");
            }
            string nomeUsuario = principal.Identity.Name;

            var usuario = await _userManager.FindByNameAsync(nomeUsuario!);

            if (usuario is null || usuario.RefreshToken != tokenAtualizar || usuario.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Acesso ao token inválido, Não pode atualizar");

            }

            var novoAcessoToken = _tokenService.GenerateAccessToken(principal.Claims.ToList());

            var novoTokenAtualizado = _tokenService.GenerateRefreshToken();

            usuario.RefreshToken = novoTokenAtualizado;

            await _userManager.UpdateAsync(usuario);

            return new ObjectResult(new
            {
                acessoToken = new JwtSecurityTokenHandler().WriteToken(novoAcessoToken),
                tokenAtualizar = novoAcessoToken
            });

        }

        [Authorize]
        [HttpPost]
        [Route("revogar/{nomeUsuario}")]
        public async Task<IActionResult> Revogar(string nomeUsuario)
        {

            var authenticatedUser = await _userManager.GetUserAsync(User);

            if (authenticatedUser == null)
            {
                return Unauthorized();
            }

            // Verifique se o usuário autenticado é o mesmo que está tentando revogar o token
            bool isSelfRevoking = authenticatedUser.UserName == nomeUsuario;

            // Verifique se o usuário autenticado é um Super Admin
            bool isSuperAdmin = await _userManager.IsInRoleAsync(authenticatedUser, "Admin");

            // Permita a revogação apenas se o usuário estiver revogando seu próprio token ou se for um Super Admin
            if (!isSelfRevoking && !isSuperAdmin)
            {
                return Forbid();
            }
            var usuario = await _userManager.FindByNameAsync(nomeUsuario);

            if (usuario is null)
            {
                return NotFound(new { Mensagem = "Usuário não encontrado!" });
            }

            usuario.RefreshToken = null;

            await _userManager.UpdateAsync(usuario);

            return Ok(new { Mensagem = "A sessão do usuário foi revogada com sucesso!" });
        }

        // Criação de roles:
        [HttpPost]
        [Route("CriandoRoles")]

        public async Task<IActionResult> CriandoRoles(string roleNome)
        {
            var roleExiste = await _roleManager.RoleExistsAsync(roleNome);

            if (!roleExiste)
            {
                var roleResultado = await _roleManager.CreateAsync(new IdentityRole(roleNome));
                if (roleResultado.Succeeded)
                {
                    _logger.LogInformation(1, "Roles adicionado");
                    return StatusCode(StatusCodes.Status200OK, new Response
                    {
                        Status = "Success",
                        Mensagem = $"Role{roleNome} adicionado com sucesso"
                    });

                }
                else
                {
                    _logger.LogInformation(2, "Error");
                    return StatusCode(StatusCodes.Status400BadRequest, new Response
                    {
                        Status = "Error",
                        Mensagem = $"Problema ao adicionar uma nova role"
                    });

                }
            }

            return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Mensagem = $"Role já existe" });
        }


        [HttpPost]
        [Route("AdicionarUsuarioParaRole")]

        public async Task<IActionResult> AdicionarUsuarioParaRole(string email, string roleNome)
        {

            var usuario = await _userManager.FindByEmailAsync(email);

            if (usuario is not null)
            {
                var resultado = await _userManager.AddToRoleAsync(usuario, roleNome);
                if (resultado.Succeeded)
                {
                    _logger.LogInformation(1, $"{usuario.Email} adicionado o usuario {roleNome} para role  ");
                    return StatusCode(StatusCodes.Status200OK, new Response
                    {
                        Status = "Success",
                        Mensagem = $"Email do Usuário {usuario.Email} adicionado com sucesso a role para o {roleNome}"
                    });
                }
                else
                {
                    _logger.LogInformation(1, $"Error: Não foi possível adicionar usuário ao {usuario.Email} para o {roleNome}");
                    return StatusCode(StatusCodes.Status400BadRequest, new Response
                    {
                        Status = "Error",
                        Mensagem = $"Error: Não foi possível adicionar usuário ao {usuario.Email} para o {roleNome}"
                    });
                }
            }

            return BadRequest(new { error = "Não foi possível criar um usuario Role" });
        }
    }
}