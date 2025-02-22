# Controle de Funcionários e Autenticação

Este projeto é uma aplicação backend desenvolvida para gerenciar o cadastro de funcionários e autenticação de usuários. Ele inclui funcionalidades de registro, login e gerenciamento de funcionários com controle de permissões e segurança usando JWT (JSON Web Tokens).

## Funcionalidades

- **Cadastro de Funcionários**: Permite adicionar, listar, detalhar, atualizar e excluir funcionários no sistema.
- **Autenticação de Usuários**: Registra novos usuários e autentica usuários existentes com base em suas credenciais. A autenticação é realizada usando JWT.
- **Controle de Permissões**: A lógica de permissões garante que apenas usuários com permissões adequadas possam realizar determinadas ações, como cadastrar ou atualizar funcionários.

## Tecnologias Utilizadas

- **ASP.NET Core**: Framework para construção de APIs RESTful.
- **JWT (JSON Web Tokens)**: Para autenticação e controle de sessões.
- **Dapper**: Para comunicação com banco de dados.
- **Swagger**: Para documentação interativa da API.
- **AutoMapper**: Para mapeamento de entidades e DTOs (Data Transfer Objects).

## Endpoints da API

### **FuncionarioController**
1. **POST /api/funcionario** - Registrar um novo funcionário (Requer autenticação)
2. **GET /api/funcionario** - Listar todos os funcionários (Requer autenticação)
3. **GET /api/funcionario/{id}** - Detalhar um funcionário específico (Requer autenticação)
4. **PUT /api/funcionario/{id}** - Atualizar informações de um funcionário (Requer autenticação)
5. **DELETE /api/funcionario/{id}** - Excluir um funcionário (Requer autenticação)

### **LoginController**
1. **POST /api/login/registrar** - Registrar um novo usuário de login
2. **POST /api/login/logar** - Realizar login de um usuário e gerar um token JWT

## Como Rodar o Projeto Localmente

### Requisitos
- .NET 6 ou superior
- Banco de dados configurado para o Dapper (ex: SQL Server)
- Configuração no `appsettings.json` para o JWT (chave secreta e detalhes de emissão)

### Passos para rodar
1. Clone o repositório:
   ```bash
   git clone https://github.com/seuusuario/controle-funcionario.git
   cd controle-funcionario
