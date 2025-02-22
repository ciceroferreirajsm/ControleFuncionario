using AutoMapper;
using ControleFuncionario.Domain.Entities;
using ControleFuncionario.Model;
using ControleFuncionario.Application.Services.Interfaces;
using ControleFuncionario.Application.Repository.Interfaces;
using Microsoft.AspNetCore.Identity.Data;

namespace ControleFuncionario.Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _LoginRepository;
        private readonly IMapper _mapper;

        public LoginService(ILoginRepository LoginRepository, IMapper mapper)
        {
            _LoginRepository = LoginRepository;
            _mapper = mapper;
        }
        public int Adicionar(LoginDTO loginRequest)
        {
            var usuario = _mapper.Map<Login>(loginRequest);

            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(loginRequest.Senha);

            return _LoginRepository.Adicionar(usuario);
        }

        public LoginDTO Logar(LoginDTO loginDTO)
        {
            var usuario = _LoginRepository.ObterPorEmail(loginDTO.Email);

            if (usuario == null)
                return null;

            var usuarioDTO = _mapper.Map<LoginDTO>(usuario);

            var verified = BCrypt.Net.BCrypt.Verify(loginDTO.Senha, usuario.Senha);

            return verified ? usuarioDTO : null;
        }
    }
}
