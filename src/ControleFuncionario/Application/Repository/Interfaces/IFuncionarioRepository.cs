using ControleFuncionario.Domain.Entities;
using ControleFuncionario.Model;
using System.Collections.Generic;

namespace ControleFuncionario.Application.Repository.Interfaces
{

    public interface IFuncionarioRepository
    {
        int Adicionar(Funcionario Funcionario);

        IEnumerable<Funcionario> Listar();

        Funcionario Detalhes(int id);
        bool Atualizar(Funcionario funcionario);
        bool Deletar(int id);
    }
}
