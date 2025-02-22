using ControleFuncionario.Model;
using System.Collections;
using System.Collections.Generic;

namespace ControleFuncionario.Application.Services.Interfaces
{
    public interface ILoginService
    {
        int Adicionar(LoginDTO login);
        LoginDTO Logar(LoginDTO login);
    }

}
