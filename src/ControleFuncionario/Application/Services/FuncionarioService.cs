using AutoMapper;
using ControleFuncionario.Application.Repository.Interfaces;
using ControleFuncionario.Application.Services.Interfaces;
using ControleFuncionario.Domain.Entities;
using ControleFuncionario.Model;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ControleFuncionario.Application.Services
{
    /// <summary>
    /// Serviço responsável pelas operações relacionadas aos funcionários.
    /// </summary>
    public class FuncionarioService : IFuncionarioService
    {
        private readonly IFuncionarioRepository _FuncionarioRepository;
        private readonly ILoginService _loginService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Construtor do serviço de funcionários.
        /// </summary>
        /// <param name="FuncionarioRepository">Repositório responsável pelo acesso aos dados de funcionários.</param>
        /// <param name="mapper">Instância do AutoMapper para conversão entre DTOs e entidades.</param>
        /// <param name="loginService">Serviço responsável pela gestão de logins.</param>
        public FuncionarioService(IFuncionarioRepository FuncionarioRepository, IMapper mapper, ILoginService loginService)
        {
            _FuncionarioRepository = FuncionarioRepository;
            _loginService = loginService;
            _mapper = mapper;
        }

        /// <summary>
        /// Adiciona um novo funcionário ao sistema.
        /// </summary>
        /// <param name="FuncionarioRequest">Dados do funcionário a ser cadastrado.</param>
        /// <returns>O ID do funcionário recém-criado.</returns>
        public int Adicionar(FuncionarioDTO FuncionarioRequest)
        {
            var func = _mapper.Map<Funcionario>(FuncionarioRequest);
            func.Ativo = true;

            // Cria um login para o funcionário
            _loginService.Adicionar(new LoginDTO
            {
                Email = func.Email,
                Nome = func.Nome,
                Senha = func.Senha,
                Permissao = FuncionarioRequest.Permissao
            });

            return _FuncionarioRepository.Adicionar(func);
        }

        /// <summary>
        /// Obtém os detalhes de um funcionário pelo ID.
        /// </summary>
        /// <param name="id">Identificador do funcionário.</param>
        /// <returns>Objeto <see cref="FuncionarioDTO"/> com os detalhes do funcionário.</returns>
        public FuncionarioDTO Detalhes(int id)
        {
            var func = _FuncionarioRepository.Detalhes(id);
            return _mapper.Map<FuncionarioDTO>(func);
        }

        /// <summary>
        /// Lista todos os funcionários cadastrados.
        /// </summary>
        /// <returns>Lista de objetos <see cref="FuncionarioDTO"/> representando os funcionários.</returns>
        public List<FuncionarioDTO> Listar()
        {
            var funcionarios = _FuncionarioRepository.Listar();
            return _mapper.Map<List<FuncionarioDTO>>(funcionarios);
        }

        /// <summary>
        /// Atualiza os dados de um funcionário.
        /// </summary>
        /// <param name="funcionario">Dados do funcionário a serem atualizados.</param>
        /// <returns>Verdadeiro se a atualização for bem-sucedida; caso contrário, falso.</returns>
        public bool Atualizar(FuncionarioDTO funcionario)
        {
            var func = _mapper.Map<Funcionario>(funcionario);
            return _FuncionarioRepository.Atualizar(func);
        }

        /// <summary>
        /// Exclui um funcionário pelo ID.
        /// </summary>
        /// <param name="id">Identificador do funcionário.</param>
        /// <returns>Verdadeiro se a exclusão for bem-sucedida; caso contrário, falso.</returns>
        public bool Deletar(int id)
        {
            return _FuncionarioRepository.Deletar(id);
        }
    }
}