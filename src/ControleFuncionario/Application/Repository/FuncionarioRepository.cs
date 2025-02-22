using ControleFuncionario.Application.Repository.Interfaces;
using ControleFuncionario.Domain.Entities;
using Dapper;
using FluentAssertions.Equivalency;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ControleFuncionario.Application.Repository
{
    /// <summary>
    /// Repositório responsável pelas operações de banco de dados relacionadas a Funcionarios bancários.
    /// Utiliza Dapper para mapear os dados entre o banco de dados e os objetos do domínio.
    /// </summary>
    /// <inheritdoc/>
    public class FuncionarioRepository : IFuncionarioRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Inicializa o repositório com a string de conexão obtida da configuração.
        /// </summary>
        /// <param name="configuration">A configuração usada para recuperar a string de conexão com o banco de dados.</param>
        /// <exception cref="Exception">Lança uma exceção se não for possível obter a string de conexão.</exception>
        /// <inheritdoc/>
        public FuncionarioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Erro ao obter string de conexão.");
        }

        /// <summary>
        /// Adiciona um novo funcionário ao banco de dados.
        /// </summary>
        /// <param name="Funcionario">O objeto de funcionário a ser adicionado.</param>
        /// <returns>O ID do funcionário recém-adicionado.</returns>
        public int Adicionar(Funcionario Funcionario)
        {
            using var dbConnection = new SqlConnection(_connectionString);

            var query = @"
                INSERT INTO Funcionario (nome, sobrenome, email, cargo, telefone, gestor, documento, senha, dt_nascimento, ativo, permissao)
                VALUES (@nome, @sobrenome, @email, @cargo, @telefone, @gestor, @documento, @senha, @dt_nascimento, @ativo, @Permissao);
                SELECT last_insert_rowid();";

            var id = dbConnection.ExecuteScalar<int>(query, new
            {
                @nome = Funcionario.Nome,
                @sobrenome = Funcionario.Sobrenome,
                @email = Funcionario.Email,
                @cargo = Funcionario.Cargo,
                @telefone = Funcionario.Telefone,
                @gestor = Funcionario.Gestor,
                @documento = Funcionario.Documento,
                @senha = Funcionario.Senha,
                @dt_nascimento = Funcionario.DtNascimento,
                @ativo = true,
                Funcionario.Permissao
            });

            return id;
        }

        /// <summary>
        /// Recupera os detalhes de um funcionário específico.
        /// </summary>
        /// <param name="id">O ID do funcionário a ser recuperado.</param>
        /// <returns>O objeto de funcionário com os detalhes do banco de dados.</returns>
        public Funcionario Detalhes(int id)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "SELECT dt_nascimento as DtNascimento, * FROM Funcionario where id = @Id";

            return dbConnection.QueryFirstOrDefault<Funcionario>(query, new { @Id = id });
        }

        /// <summary>
        /// Lista todos os funcionários ativos.
        /// </summary>
        /// <returns>Uma coleção de funcionários ativos.</returns>
        public IEnumerable<Funcionario> Listar()
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "SELECT id as Id, * FROM Funcionario where ativo = 1";

            return dbConnection.Query<Funcionario>(query).ToList();
        }

        /// <summary>
        /// Atualiza os detalhes de um funcionário existente.
        /// </summary>
        /// <param name="funcionario">O objeto de funcionário com as novas informações a serem atualizadas.</param>
        /// <returns>Retorna verdadeiro se a atualização foi bem-sucedida, caso contrário, retorna falso.</returns>
        public bool Atualizar(Funcionario funcionario)
        {
            using var dbConnection = new SqlConnection(_connectionString);

            var query = @"
                UPDATE Funcionario 
                SET nome = @Nome,
                    sobrenome = @Sobrenome,
                    email = @Email,
                    telefone = @Telefone,
                    gestor = @Gestor,
                    documento = @Documento,
                    cargo = @Cargo,
                    senha = @Senha,
                    dt_nascimento = @DtNascimento
                WHERE id = @Id";

            var linhasAfetadas = dbConnection.Execute(query, new
            {
                @Id = funcionario.Id,
                @Nome = funcionario.Nome,
                @Sobrenome = funcionario.Sobrenome,
                @Email = funcionario.Email,
                @Telefone = funcionario.Telefone,
                @Gestor = funcionario.Gestor,
                @Documento = funcionario.Documento,
                @Senha = funcionario.Senha,
                @DtNascimento = funcionario.DtNascimento,
                @Cargo = funcionario.Cargo
            });

            return linhasAfetadas > 0;
        }

        /// <summary>
        /// Deleta (desativa) um funcionário no banco de dados, tornando-o inativo.
        /// </summary>
        /// <param name="id">O ID do funcionário a ser deletado.</param>
        /// <returns>Retorna verdadeiro se o funcionário foi desativado com sucesso, caso contrário, retorna falso.</returns>
        public bool Deletar(int id)
        {
            using var dbConnection = new SqlConnection(_connectionString);

            var query = @"UPDATE Funcionario SET ativo = 0 WHERE id = @Id";

            var linhasAfetadas = dbConnection.Execute(query, new { Id = id });

            return linhasAfetadas > 0;
        }
    }
}
