using ControleFuncionario.Domain.Entities;
using ControleFuncionario.Model;
using System.Collections.Generic;

namespace ControleFuncionario.Application.Repository.Interfaces
{
    /// <summary>
    /// Interface que define os métodos necessários para realizar operações de persistência de dados relacionadas aos funcionários.
    /// Esta interface é usada pelo repositório para manipular os dados dos funcionários no banco de dados.
    /// </summary>
    public interface IFuncionarioRepository
    {
        /// <summary>
        /// Adiciona um novo funcionário ao banco de dados.
        /// </summary>
        /// <param name="Funcionario">O objeto `Funcionario` que contém os dados do funcionário a ser adicionado.</param>
        /// <returns>O ID do funcionário recém-adicionado.</returns>
        int Adicionar(Funcionario Funcionario);

        /// <summary>
        /// Lista todos os funcionários ativos no sistema.
        /// </summary>
        /// <returns>Uma coleção de objetos `Funcionario`, representando todos os funcionários registrados no sistema.</returns>
        IEnumerable<Funcionario> Listar();

        /// <summary>
        /// Recupera os detalhes de um funcionário específico a partir do banco de dados.
        /// </summary>
        /// <param name="id">O ID do funcionário a ser recuperado.</param>
        /// <returns>Um objeto `Funcionario` com os dados detalhados do funcionário, ou `null` se o funcionário não for encontrado.</returns>
        Funcionario Detalhes(int id);

        /// <summary>
        /// Atualiza as informações de um funcionário existente no banco de dados.
        /// </summary>
        /// <param name="funcionario">O objeto `Funcionario` contendo as informações atualizadas do funcionário.</param>
        /// <returns>Retorna `true` se a atualização for bem-sucedida, ou `false` se não for possível atualizar o funcionário.</returns>
        bool Atualizar(Funcionario funcionario);

        /// <summary>
        /// Deleta (ou marca como inativo) um funcionário no banco de dados.
        /// </summary>
        /// <param name="id">O ID do funcionário a ser deletado.</param>
        /// <returns>Retorna `true` se a operação de deleção for bem-sucedida, ou `false` se não for possível deletar o funcionário.</returns>
        bool Deletar(int id);
    }
}
