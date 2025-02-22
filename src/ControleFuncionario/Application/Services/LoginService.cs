using AutoMapper;
using ControleFuncionario.Domain.Entities;
using ControleFuncionario.Model;
using ControleFuncionario.Application.Services.Interfaces;
using ControleFuncionario.Application.Repository.Interfaces;
using Microsoft.AspNetCore.Identity.Data;

namespace ControleFuncionario.Application.Services
{
    /// <summary>
    /// Serviço responsável pelas operações relacionadas ao login dos usuários.
    /// </summary>
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _LoginRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Construtor do serviço de login.
        /// </summary>
        /// <param name="LoginRepository">Repositório responsável pelo acesso aos dados de login.</param>
        /// <param name="mapper">Instância do AutoMapper para conversão entre DTOs e entidades.</param>
        public LoginService(ILoginRepository LoginRepository, IMapper mapper)
        {
            _LoginRepository = LoginRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Adiciona um novo usuário ao sistema.
        /// </summary>
        /// <param name="loginRequest">Dados do usuário a ser cadastrado.</param>
        /// <returns>O ID do usuário recém-criado.</returns>
        public int Adicionar(LoginDTO loginRequest)
        {
            var usuario = _mapper.Map<Login>(loginRequest);

            // Hash da senha antes de armazenar no banco de dados
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(loginRequest.Senha);

            return _LoginRepository.Adicionar(usuario);
        }

        /// <summary>
        /// Realiza o login do usuário, verificando as credenciais informadas.
        /// </summary>
        /// <param name="loginDTO">Dados do usuário para autenticação.</param>
        /// <returns>Um objeto <see cref="LoginDTO"/> se as credenciais forem válidas; caso contrário, retorna null.</returns>
        public LoginDTO Logar(LoginDTO loginDTO)
        {
            var usuario = _LoginRepository.ObterPorEmail(loginDTO.Email);

            if (usuario == null)
                return null;

            var usuarioDTO = _mapper.Map<LoginDTO>(usuario);

            // Verificação da senha informada com a armazenada no banco de dados
            var verified = BCrypt.Net.BCrypt.Verify(loginDTO.Senha, usuario.Senha);

            return verified ? usuarioDTO : null;
        }
    }
}
