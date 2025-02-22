# **Movimentação API**

## **Descrição**
A **Movimentação API** é uma API para o gerenciamento de movimentações bancárias. Ela permite a criação de registros de movimentações (créditos e débitos), consulta de saldo de contas correntes e o gerenciamento de idempotência de requisições. O projeto utiliza o banco de dados SQLite e segue a arquitetura de Camadas.

## **Tecnologias Utilizadas**
- **.NET 6.0**
- **Swagger/OpenAPI** para documentação da API
- **SQLite** como banco de dados relacional
- **FluentAssertions** para testes de asserções
- **MediatR** para manipulação de comandos e consultas
- **Dapper** para acesso ao banco de dados

## **Instalação e Configuração**

### **Pré-requisitos**
- **.NET SDK 6.0** ou superior
- Editor de código (Recomendado: [Visual Studio](https://visualstudio.microsoft.com/) ou [Visual Studio Code](https://code.visualstudio.com/))

### **Passos para Configuração**

1. **Clonar o Repositório**
   Clone este repositório para o seu computador local:

   ```bash
   git clone https://github.com/seu-usuario/MovimentacaoAPI.git
   cd MovimentacaoAPI
