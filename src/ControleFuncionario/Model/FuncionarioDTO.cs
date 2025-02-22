using ControleFuncionario.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

public class FuncionarioDTO
{
    public string id { get; set; }
    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    public string email { get; set; }
    public string Telefone { get; set; }
    public string Cargo { get; set; }
    public string Documento { get; set; }
    public string Gestor { get; set; }
    public string Senha { get; set; }
    public string DtNascimento { get; set; }
    public bool Ativo { get; set; }
    public string Permissao { get; set; }

   
    public bool ValidarPermissaoUsuario(string userRole, out string errorMessage)
    {
        if (Enum.TryParse(userRole, out Permissoes userPermission))
        {
            errorMessage = string.Empty;
            return true;
        }

        errorMessage = "Permissão inválida do usuário.";
        return false;
    }

    public bool ValidarPermissaoRequest(string permissaoRequest, out string errorMessage)
    {
        if (Enum.TryParse(permissaoRequest, out Permissoes userRequestPermi))
        {
            errorMessage = string.Empty;
            return true;
        }

        errorMessage = "Permissão inválida do funcionário.";
        return false;
    }

    public IEnumerable<ValidationResult> ValidarCriacaoFuncionarioComPermissaoSuperior(string userRole, string requestPermissao)
    {
        if (Enum.TryParse(userRole, out Permissoes userPermission) &&
            Enum.TryParse(requestPermissao, out Permissoes userRequestPermi))
        {
            if (userPermission == Permissoes.funcionario &&
                (userRequestPermi == Permissoes.diretor || userRequestPermi == Permissoes.lider))
            {
                yield return new ValidationResult("Funcionários não podem criar diretores ou líderes.", new[] { nameof(Permissao) });
            }
        }
    }
}
