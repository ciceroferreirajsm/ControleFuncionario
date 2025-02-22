using ControleFuncionario.Domain.Entities;
using ControleFuncionario.Model;
using System.Collections.Generic;

namespace ControleFuncionario.Application.Services.Interfaces
{
    public interface IFuncionarioService
    {
        int Adicionar(FuncionarioDTO Funcionario);

        List<FuncionarioDTO> Listar();

        FuncionarioDTO Detalhes(int id);
        bool Atualizar(FuncionarioDTO funcionario);
        bool Deletar(int id);
    }

}
