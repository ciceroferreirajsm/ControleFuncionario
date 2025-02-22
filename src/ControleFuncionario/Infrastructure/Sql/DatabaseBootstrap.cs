using Dapper;
using Microsoft.Data.SqlClient;

public class DatabaseSetup
{
    private readonly string _connectionString;
    private readonly string _databaseName = "MeuBanco";

    public DatabaseSetup(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Setup()
    {
        using (var masterConnection = new SqlConnection(_connectionString))
        {
            masterConnection.Open();

            var databaseExists = masterConnection.ExecuteScalar<int>(
                "SELECT COUNT(*) FROM sys.databases WHERE name = @DatabaseName",
                new { DatabaseName = _databaseName });

            if (databaseExists == 0)
            {
                masterConnection.Execute($"CREATE DATABASE {_databaseName}");
            }
        }

        var dbConnectionString = $"{_connectionString};Database={_databaseName};";
        using (var dbConnection = new SqlConnection(dbConnectionString))
        {
            dbConnection.Open();

            var tableExists = dbConnection.ExecuteScalar<int>(
                "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME IN ('Funcionario', 'Login')");

            if (tableExists == 0)
            {
                dbConnection.Execute(@"
                    CREATE TABLE Funcionario (
                        id INT IDENTITY(1,1) PRIMARY KEY,
                        nome NVARCHAR(100) NULL,
                        sobrenome NVARCHAR(100) NULL,
                        cargo NVARCHAR(100) NULL,
                        email NVARCHAR(255) NULL,
                        telefone NVARCHAR(20) NULL,
                        documento NVARCHAR(50) NULL,
                        gestor NVARCHAR(100) NULL,
                        senha NVARCHAR(255) NULL,
                        dt_nascimento DATE NULL,
                        ativo BIT NOT NULL DEFAULT 1,
                        permissao INT NULL
                    );

                    CREATE TABLE Login (
                        id INT IDENTITY(1,1) PRIMARY KEY,
                        nome NVARCHAR(100) NULL,
                        email NVARCHAR(255) NULL,
                        senha NVARCHAR(255) NULL,
                        permissao INT NULL
                    );
                ");
            }
        }
    }
}
