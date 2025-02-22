using ControleFuncionario.Domain.Entities;
using System.Collections.Generic;

namespace ControleFuncionario.Application.Repository.Interfaces
{
    /// <summary>
    /// Interface que define os métodos necessários para realizar operações de persistência de dados relacionadas ao login dos usuários.
    /// Esta interface é usada pelo repositório para manipular os dados de login no banco de dados.
    /// </summary>
    public interface ILoginRepository
    {
        /// <summary>
        /// Adiciona um novo registro de login no banco de dados.
        /// </summary>
        /// <param name="Login">O objeto `Login` contendo as informações de login a ser adicionado.</param>
        /// <returns>O ID do login recém-adicionado.</returns>
        int Adicionar(Login Login);

        /// <summary>
        /// Recupera um registro de login a partir do banco de dados com base no endereço de e-mail.
        /// </summary>
        /// <param name="email">O endereço de e-mail do usuário cujo login será recuperado.</param>
        /// <returns>Um objeto `Login` contendo as informações de login do usuário ou `null` se nenhum login for encontrado com o e-mail fornecido.</returns>
        Login ObterPorEmail(string email);
    }
}
