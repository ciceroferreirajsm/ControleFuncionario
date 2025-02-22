using ControleFuncionario.Domain.Entities;
using ControleFuncionario.Model;
using System.Collections.Generic;

namespace ControleFuncionario.Application.Services.Interfaces
{
    /// <summary>
    /// Interface que define os métodos necessários para manipular os dados dos funcionários.
    /// A interface é utilizada pelos serviços da aplicação para implementar a lógica de negócios.
    /// </summary>
    public interface IFuncionarioService
    {
        /// <summary>
        /// Adiciona um novo funcionário no sistema.
        /// </summary>
        /// <param name="Funcionario">O objeto de funcionário a ser adicionado, representado pelo DTO (Data Transfer Object).</param>
        /// <returns>O ID do funcionário recém-adicionado.</returns>
        int Adicionar(FuncionarioDTO Funcionario);

        /// <summary>
        /// Lista todos os funcionários do sistema.
        /// </summary>
        /// <returns>Uma lista de objetos do tipo `FuncionarioDTO`, que representam os funcionários no sistema.</returns>
        List<FuncionarioDTO> Listar();

        /// <summary>
        /// Recupera os detalhes de um funcionário específico.
        /// </summary>
        /// <param name="id">O ID do funcionário a ser recuperado.</param>
        /// <returns>O objeto `FuncionarioDTO` com os detalhes do funcionário ou `null` se o funcionário não for encontrado.</returns>
        FuncionarioDTO Detalhes(int id);

        /// <summary>
        /// Atualiza as informações de um funcionário no sistema.
        /// </summary>
        /// <param name="funcionario">O objeto `FuncionarioDTO` com as novas informações do funcionário a ser atualizado.</param>
        /// <returns>Retorna `true` se a atualização foi bem-sucedida, ou `false` se não foi possível atualizar.</returns>
        bool Atualizar(FuncionarioDTO funcionario);

        /// <summary>
        /// Deleta um funcionário do sistema, tornando-o inativo.
        /// </summary>
        /// <param name="id">O ID do funcionário a ser deletado.</param>
        /// <returns>Retorna `true` se o funcionário foi deletado com sucesso, ou `false` se não foi possível deletá-lo.</returns>
        bool Deletar(int id);
    }
}
