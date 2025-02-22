using Dapper;
using FluentAssertions.Equivalency;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Data.Sqlite;
using Moq;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Numerics;

namespace ControleFuncionario.Infrastructure.Sqlite
{
    public class DatabaseBootstrap : IDatabaseBootstrap
    {
        private readonly DatabaseConfig databaseConfig;

        public DatabaseBootstrap(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public void Setup() 
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            var table = connection.Query<string>("SELECT name FROM sqlite_master WHERE type='table' AND (name = 'Funcionario') OR (name = 'Login') ;");
            var tableName = table.FirstOrDefault();
            if (!string.IsNullOrEmpty(tableName) && tableName == "Funcionario" || tableName == "Login")
                return;

            connection.Execute("CREATE TABLE Funcionario (" +
                                "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                "nome NVARCHAR(100) NULL, " +
                                "sobrenome NVARCHAR(100) NULL, " +
                                "cargo NVARCHAR(100) NULL, " +
                                "email NVARCHAR(255) NULL, " +
                                "telefone NVARCHAR(20) NULL, " +
                                "documento NVARCHAR(50) NULL, " +
                                "gestor NVARCHAR(100) NULL, " +
                                "senha NVARCHAR(255) NULL, " +
                                "dt_nascimento DATE NULL, " +
                                "ativo BIT NULL DEFAULT 1, " +
                                "permissao INT NULL)");


            connection.Execute("CREATE TABLE Login (" +
                    "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "nome NVARCHAR(100) NULL, " +
                    "email NVARCHAR(255) NULL, " +
                    "senha NVARCHAR(255) NULL, " +
                    "permissao INT NULL)");
        }
    }
}
