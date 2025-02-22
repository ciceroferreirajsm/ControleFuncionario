using ControleFuncionario.Domain.Entities;
using System.Collections.Generic;

namespace ControleFuncionario.Application.Repository.Interfaces
{

    public interface ILoginRepository
    {
        int Adicionar(Login Login);
        Login ObterPorEmail(string email);
    }
}
