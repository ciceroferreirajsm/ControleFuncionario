using ControleFuncionario.Application.Repository.Interfaces;
using ControleFuncionario.Domain.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ControleFuncionario.Application.Repository
{
    /// <summary>
    /// Repositório responsável pelas operações de banco de dados relacionadas a Logins.
    /// Utiliza Dapper para mapear os dados entre o banco de dados e os objetos do domínio.
    /// </summary>
    public class LoginRepository : ILoginRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Inicializa o repositório com a string de conexão obtida da configuração.
        /// </summary>
        /// <param name="configuration">A configuração usada para recuperar a string de conexão com o banco de dados.</param>
        /// <exception cref="Exception">Lança uma exceção se não for possível obter a string de conexão.</exception>
        public LoginRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? throw new Exception("Erro ao obter string de conexão.");
        }

        /// <summary>
        /// Adiciona um novo login ao banco de dados.
        /// </summary>
        /// <param name="login">Objeto contendo as informações do login a ser adicionado.</param>
        /// <returns>O ID do login recém-criado.</returns>
        public int Adicionar(Login login)
        {
            using var dbConnection = new SqlConnection(_connectionString);

            var query = @"USE Meubanco
                INSERT INTO dbo.Login (nome, email, senha, permissao)
                VALUES (@Nome, @Email, @Senha, @Permissao);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var id = dbConnection.ExecuteScalar<int>(query, new
            {
                @Nome = login.Nome,
                @Email = login.Email,
                @Senha = login.Senha,
                @Permissao = login.Permissao
            });

            return id;
        }

        /// <summary>
        /// Obtém um login com base no email informado.
        /// </summary>
        /// <param name="email">Email do login a ser pesquisado.</param>
        /// <returns>Objeto <see cref="Login"/> correspondente ao email, ou null se não encontrado.</returns>
        public Login ObterPorEmail(string email)
        {
            using var dbConnection = new SqlConnection(_connectionString);

            var query = @"USE Meubanco
                SELECT * FROM dbo.Login WHERE email = @Email";

            return dbConnection.QueryFirstOrDefault<Login>(query, new { @Email = email });
        }
    }
}