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
    public class FuncionarioService : IFuncionarioService
    {
        private readonly IFuncionarioRepository _FuncionarioRepository;
        private readonly ILoginService _loginService;
        private readonly IMapper _mapper;

        public FuncionarioService(IFuncionarioRepository FuncionarioRepository, IMapper mapper, ILoginService loginService)
        {
            _FuncionarioRepository = FuncionarioRepository;
            _loginService = loginService;
            _mapper = mapper;
        }
        public int Adicionar(FuncionarioDTO FuncionarioRequest)
        {
            var func = _mapper.Map<Funcionario>(FuncionarioRequest);
            func.Ativo = true;
            _loginService.Adicionar(new LoginDTO
            {
                Email = func.Email,
                Nome = func.Nome,
                Senha = func.Senha,
                Permissao = FuncionarioRequest.Permissao
            });
            return _FuncionarioRepository.Adicionar(func);
        }
        public FuncionarioDTO Detalhes(int id)
        {
            var func = _FuncionarioRepository.Detalhes(id);

            return _mapper.Map<FuncionarioDTO>(func);
        }

        public List<FuncionarioDTO> Listar()
        {
            var funcionarios = _FuncionarioRepository.Listar();

            return _mapper.Map<List<FuncionarioDTO>>(funcionarios);
        }

        public bool Atualizar(FuncionarioDTO funcionario)
        {
            var func = _mapper.Map<Funcionario>(funcionario);
            return _FuncionarioRepository.Atualizar(func);
        }

        public bool Deletar(int id)
        {
            return _FuncionarioRepository.Deletar(id);
        }
    }
}
