using ControleFuncionario.Model;
using System.Collections;
using System.Collections.Generic;

namespace ControleFuncionario.Application.Services.Interfaces
{
    /// <summary>
    /// Interface que define os métodos necessários para manipular as operações de login no sistema.
    /// Essa interface é utilizada pelos serviços da aplicação para implementar a lógica de autenticação de usuários.
    /// </summary>
    public interface ILoginService
    {
        /// <summary>
        /// Adiciona um novo registro de login no sistema.
        /// </summary>
        /// <param name="login">O objeto de login a ser adicionado, representado pelo DTO (Data Transfer Object).</param>
        /// <returns>O ID do login recém-adicionado.</returns>
        int Adicionar(LoginDTO login);

        /// <summary>
        /// Realiza o processo de login de um usuário no sistema.
        /// </summary>
        /// <param name="login">O objeto de login contendo as credenciais do usuário, representado pelo DTO.</param>
        /// <returns>Um objeto `LoginDTO` com as informações do usuário autenticado, ou `null` se a autenticação falhar.</returns>
        LoginDTO Logar(LoginDTO login);
    }
}
