using ControleFuncionario.Application.Services.Interfaces;
using ControleFuncionario.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ControleLogin.Controllers
{
    /// <summary>
    /// Controller responsável pelas operações de login e registro de usuários.
    /// Permite o registro de novos usuários e a autenticação de usuários existentes para gerar tokens JWT.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _LoginService;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Construtor que inicializa o controlador com o serviço de login e a configuração do sistema.
        /// </summary>
        /// <param name="loginService">Serviço responsável pela lógica de login.</param>
        /// <param name="configuration">Configuração da aplicação, usada para ler parâmetros de autenticação.</param>
        public LoginController(ILoginService loginService, IConfiguration configuration)
        {
            _LoginService = loginService;
            _configuration = configuration;
        }

        /// <summary>
        /// Registra um novo usuário no sistema.
        /// </summary>
        /// <param name="request">Objeto `LoginDTO` contendo as informações do novo usuário.</param>
        /// <returns>Resultado da operação de registro, com o ID do novo usuário ou mensagem de erro.</returns>
        [HttpPost("registrar")]
        public IActionResult Registrar([FromBody] LoginDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var id = _LoginService.Adicionar(request);
                return Ok(new { id = id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao adicionar usuário." });
            }
        }

        /// <summary>
        /// Realiza a autenticação de um usuário e gera um token JWT.
        /// </summary>
        /// <param name="request">Objeto `LoginDTO` contendo as credenciais do usuário.</param>
        /// <returns>Resultado da operação de login, com o token JWT gerado ou mensagem de erro.</returns>
        [HttpPost("logar")]
        public IActionResult Login([FromBody] LoginDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = _LoginService.Logar(request);
            if (usuario == null)
                return Unauthorized(new { message = "Credenciais inválidas." });

            var token = GerarToken(usuario);
            return Ok(new { token, usuario.Permissao });
        }

        /// <summary>
        /// Gera um token JWT para o usuário autenticado.
        /// </summary>
        /// <param name="usuario">Objeto `LoginDTO` contendo as informações do usuário autenticado.</param>
        /// <returns>Token JWT gerado para o usuário.</returns>
        private string GerarToken(LoginDTO usuario)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Role, usuario.Permissao)
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
