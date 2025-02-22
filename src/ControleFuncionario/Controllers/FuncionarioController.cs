using ControleFuncionario.Application.Services.Interfaces;
using ControleFuncionario.Domain.Entities;
using ControleFuncionario.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using System;
using System.Linq;
using System.Security.Claims;

namespace ControleFuncionario.Controllers
{
    /// <summary>
    /// Controller Funcionario
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FuncionarioController : ControllerBase
    {
        private readonly IFuncionarioService _FuncionarioService;

        /// <summary>
        /// Construtor Controller Funcionario
        /// </summary>
        public FuncionarioController(IFuncionarioService FuncionarioRepository)
        {
            _FuncionarioService = FuncionarioRepository;
        }

        [HttpPost]
        [Authorize]
        public IResult Registrar([FromBody] FuncionarioDTO request)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (!request.ValidarPermissaoUsuario(userRole, out string userRoleErrorMessage))
                ModelState.AddModelError("Role", userRoleErrorMessage);

            if (!request.ValidarPermissaoRequest(request.Permissao, out string requestPermissaoErrorMessage))
                ModelState.AddModelError("Permissao", requestPermissaoErrorMessage);

            var validationResults = request.ValidarCriacaoFuncionarioComPermissaoSuperior(userRole, request.Permissao);
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


        [HttpGet]
        [Authorize]
        public IResult Listar()
        {
            if (!ModelState.IsValid)
                return Results.BadRequest(ModelState);

            return Results.Ok(_FuncionarioService.Listar());
        }

        [HttpGet("{id}")]
        [Authorize]
        public IResult Detalhes(int id)
        {
            var func = _FuncionarioService.Detalhes(id);

            if (func != null)
                return Results.Ok(func);

            return Results.NotFound(new { message = "Funcionario não localizado." });
        }

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
