using ControleFuncionario.Application.Services.Interfaces;
using ControleFuncionario.Domain.Entities;
using ControleFuncionario.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;

namespace ControleFuncionario.Controllers
{
    /// <summary>
    /// Controller responsável pelas operações relacionadas aos Funcionários.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FuncionarioController : ControllerBase
    {
        private readonly IFuncionarioService _FuncionarioService;

        /// <summary>
        /// Construtor do Controller Funcionario.
        /// Inicializa a instância do serviço de funcionário.
        /// </summary>
        /// <param name="FuncionarioRepository">Instância do serviço `IFuncionarioService` que manipula os dados dos funcionários.</param>
        public FuncionarioController(IFuncionarioService FuncionarioRepository)
        {
            _FuncionarioService = FuncionarioRepository;
        }

        /// <summary>
        /// Método para registrar um novo funcionário.
        /// O método valida as permissões do usuário e do request, e, em seguida, adiciona o funcionário.
        /// </summary>
        /// <param name="request">Objeto `FuncionarioDTO` contendo os dados do funcionário a ser registrado.</param>
        /// <returns>Retorna o ID do funcionário recém-adicionado ou uma mensagem de erro.</returns>
        [HttpPost]
        public IResult Registrar([FromBody] FuncionarioDTO request)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (!request.ValidarPermissaoUsuario(userRole, out string userRoleErrorMessage))
                ModelState.AddModelError("Role", userRoleErrorMessage);

            if (!request.ValidarPermissaoRequest(request.Permissao, out string requestPermissaoErrorMessage))
                ModelState.AddModelError("Permissao", requestPermissaoErrorMessage);

            var validationResults = request.ValidacoesFuncionario(userRole, request.Permissao);
            foreach (var validationResult in validationResults)
            {
                ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }

            if (!ModelState.IsValid)
                return Results.BadRequest(ModelState);

            try
            {
                var id = _FuncionarioService.Adicionar(request);
                return Results.Ok(new { id = id });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = $"Erro ao adicionar funcionário. {ex.Message}" });
            }
        }

        /// <summary>
        /// Método para listar todos os funcionários ativos.
        /// </summary>
        /// <returns>Lista de funcionários ativos ou uma mensagem de erro.</returns>
        [HttpGet]
        [Authorize]
        public IResult Listar()
        {
            if (!ModelState.IsValid)
                return Results.BadRequest(ModelState);

            return Results.Ok(_FuncionarioService.Listar());
        }

        /// <summary>
        /// Método para obter os detalhes de um funcionário com base no ID.
        /// </summary>
        /// <param name="id">ID do funcionário.</param>
        /// <returns>Detalhes do funcionário ou mensagem de erro caso não seja encontrado.</returns>
        [HttpGet("{id}")]
        [Authorize]
        public IResult Detalhes(int id)
        {
            var func = _FuncionarioService.Detalhes(id);

            if (func != null)
                return Results.Ok(func);

            return Results.NotFound(new { message = "Funcionario não localizado." });
        }

        /// <summary>
        /// Método para atualizar os dados de um funcionário.
        /// </summary>
        /// <param name="id">ID do funcionário a ser atualizado.</param>
        /// <param name="request">Objeto `FuncionarioDTO` com os novos dados.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [HttpPut("{id}")]
        [Authorize]
        public IResult Atualizar(int id, [FromBody] FuncionarioDTO request)
        {
            if (!ModelState.IsValid)
                return Results.BadRequest(ModelState);

            try
            {
                bool atualizado = _FuncionarioService.Atualizar(request);

                if (atualizado)
                {
                    return Results.Ok(new { message = "Funcionário atualizado com sucesso." });
                }
                else
                {
                    return Results.NotFound(new { message = "Funcionário não encontrado." });
                }
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = "Erro ao atualizar funcionário." });
            }
        }

        /// <summary>
        /// Método para deletar um funcionário com base no ID.
        /// </summary>
        /// <param name="id">ID do funcionário a ser deletado.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [HttpDelete("{id}")]
        [Authorize]
        public IResult Deletar(int id)
        {
            if (!ModelState.IsValid)
                return Results.BadRequest(ModelState);

            try
            {
                bool deletado = _FuncionarioService.Deletar(id);

                if (deletado)
                {
                    return Results.Ok(new { message = "Funcionário deletado com sucesso." });
                }
                else
                {
                    return Results.NotFound(new { message = "Funcionário não encontrado." });
                }
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = "Erro ao deletar funcionário." });
            }
        }
    }
}
