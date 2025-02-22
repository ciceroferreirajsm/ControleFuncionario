using ControleFuncionario.Application.Repository.Interfaces;
using ControleFuncionario.Domain.Entities;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ControleFuncionario.Application.Repository
{
    /// <summary>
    /// Repositório responsável pelas operações de banco de dados relacionadas a Logins bancários.
    /// Utiliza Dapper para mapear os dados entre o banco de dados e os objetos do domínio.
    /// </summary>
    /// <inheritdoc/>
    public class LoginRepository : ILoginRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Inicializa o repositório com a string de conexão obtida da configuração.
        /// </summary>
        /// <param name="configuration">A configuração usada para recuperar a string de conexão com o banco de dados.</param>
        /// <exception cref="Exception">Lança uma exceção se não for possível obter a string de conexão.</exception>
        /// <inheritdoc/>
        public LoginRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetSection("DatabaseName").Value ?? throw new Exception("Erro ao obter string de conexão.");
        }

        public int Adicionar(Login Login)
        {
            using var dbConnection = new SqliteConnection(_connectionString);

            var query = @"
                INSERT INTO Login (nome, email, senha, permissao)
                VALUES (@Nome, @Email, @Senha, @Permissao);
                SELECT last_insert_rowid();";

            var id = dbConnection.ExecuteScalar<int>(query, new
            {
                @Nome = Login.Nome,
                @Email = Login.Email,
                @Senha = Login.Senha,
                @Permissao = Login.Permissao
            });

            return id;
        }

        public Login ObterPorEmail(string email)
        {
            using var dbConnection = new SqliteConnection(_connectionString);

            var query = @"
                SELECT * FROM Login where email = @Email";

            return dbConnection.QueryFirstOrDefault<Login>(query, new
            {
                @Email = email
            });
        }
    }
}
