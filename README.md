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
- **Docker** para Containerização
- 
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

### **Movimento**

- **POST** `/api/movimento`
  - **Descrição**: Cria um novo movimento bancário (Crédito ou Débito).
  - **Request Body**: 
    ```json
    {
      "idcontacorrente": "string",
      "valor": "decimal",
      "tipomovimento": "C|D",
      "datamovimento": "datetime"
    }
    ```
  - **Respostas**:
    - **201 Created**: Movimento criado com sucesso.
    - **400 Bad Request**: Se algum campo obrigatório estiver ausente ou inválido.
    - **500 Internal Server Error**: Se ocorrer algum erro no servidor.

---

- **GET** `/api/movimento/{contaCorrenteId}`
  - **Descrição**: Obtém todos os movimentos de uma conta corrente específica.
  - **Parâmetros**:
    - **contaCorrenteId** (path): O ID da conta corrente.
  - **Respostas**:
    - **200 OK**: Retorna uma lista de movimentos da conta corrente solicitada.
    - **404 Not Found**: Se a conta corrente não for encontrada.
    - **500 Internal Server Error**: Se ocorrer algum erro no servidor.

---

### **Conta Corrente**

- **GET** `/api/contacorrente/{contaCorrenteId}`
  - **Descrição**: Obtém uma conta corrente pelo seu ID.
  - **Parâmetros**:
    - **contaCorrenteId** (path): O ID da conta corrente.
  - **Respostas**:
    - **200 OK**: Retorna os detalhes da conta corrente.
    - **404 Not Found**: Se a conta corrente não for encontrada.
    - **500 Internal Server Error**: Se ocorrer algum erro no servidor.

---

## **Estrutura do Projeto**

O projeto está organizado de maneira a separar as responsabilidades em camadas distintas:

1. **Controllers**: Contém os controladores que gerenciam as requisições HTTP.
2. **Application**: Contém os serviços e repositórios, onde a lógica de negócios é implementada.
3. **Domain**: Contém as entidades que representam o domínio da aplicação.
4. **Infrastructure**: Contém a implementação de acesso ao banco de dados e a configuração de dependências.

## **Banco de Dados**

A aplicação utiliza um banco de dados **SQLite** para persistência de dados. O banco de dados contém as seguintes tabelas principais:

- **movimento**: Contém os registros de movimentações bancárias (Créditos e Débitos).
- **contacorrente**: Contém as informações das contas correntes.
- **idempotencia**: Controla as requisições e respostas idempotentes.

### **Entidades**

#### **Movimento**
A entidade `Movimento` representa uma movimentação bancária. As propriedades são:

- **idmovimento**: Identificador único da movimentação.
- **idcontacorrente**: ID da conta corrente associada à movimentação.
- **valor**: O valor da movimentação.
- **tipomovimento**: O tipo da movimentação (C para crédito e D para débito).
- **datamovimento**: A data e hora em que a movimentação foi realizada.

#### **ContaCorrente**
A entidade `ContaCorrente` representa uma conta corrente bancária. As propriedades incluem:

- **idcontacorrente**: Identificador único da conta corrente.
- **saldo**: O saldo atual da conta.

#### **Idempotencia**
A entidade `Idempotencia` representa a controle de requisições e respostas idempotentes. As propriedades incluem:

- **id**: Identificador único da entrada de idempotência.
- **requisicao**: O conteúdo da requisição original.
- **resposta**: O conteúdo da resposta da requisição.
- **chave_idempotencia**: A chave associada para identificar a requisição idempotente.

## **Rodando em Desenvolvimento**

1. **Rodar a API Localmente**:
   - Utilize o comando `dotnet run` para rodar a aplicação em ambiente de desenvolvimento local.
   
2. **Configuração do Banco de Dados**:
   - O banco de dados será configurado automaticamente na primeira execução. Não é necessário criar o banco manualmente.

3. **Testes**:
   - O projeto utiliza **FluentAssertions** para facilitar a escrita de testes unitários. É recomendado que testes sejam feitos para garantir que todas as funcionalidades estejam funcionando conforme esperado.

# ControleFuncionario
