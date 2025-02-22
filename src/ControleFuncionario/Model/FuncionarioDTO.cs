using ControleFuncionario.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

public class FuncionarioDTO
{
    public string id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O sobrenome é obrigatório.")]
    public string Sobrenome { get; set; }

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string Email { get; set; }

    public string Telefone { get; set; }
    public string Cargo { get; set; }
    public string Documento { get; set; }
    public string Gestor { get; set; }
    public string Senha { get; set; }

    [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
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

    public IEnumerable<ValidationResult> ValidacoesFuncionario(string userRole, string requestPermissao)
    {
        if (Enum.TryParse(userRole, out Permissoes userPermission) &&
            Enum.TryParse(requestPermissao, out Permissoes userRequestPermi))
        {
            if (userPermission == Permissoes.funcionario &&
                (userRequestPermi == Permissoes.diretor || userRequestPermi == Permissoes.lider))
            {
                yield return new ValidationResult("Funcionários não podem criar diretores ou líderes.", new[] { nameof(Permissao) });
            }

            if (userPermission == Permissoes.lider && userRequestPermi == Permissoes.diretor)
            {
                yield return new ValidationResult("Lideres não podem criar diretores.", new[] { nameof(Permissao) });
            }
        }

        if (!DateTime.TryParseExact(DtNascimento, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataNascimento))
        {
            yield return new ValidationResult("Data de nascimento inválida. Use o formato 'yyyy-MM-dd'.", new[] { nameof(DtNascimento) });
        }
        else
        {
            int idade = CalcularIdade(dataNascimento);
            if (idade < 18)
            {
                yield return new ValidationResult("O funcionário deve ter pelo menos 18 anos.", new[] { nameof(DtNascimento) });
            }
        }
    }

    private int CalcularIdade(DateTime dataNascimento)
    {
        DateTime hoje = DateTime.Today;
        int idade = hoje.Year - dataNascimento.Year;
        if (dataNascimento.Date > hoje.AddYears(-idade)) idade--;
        return idade;
    }
}
