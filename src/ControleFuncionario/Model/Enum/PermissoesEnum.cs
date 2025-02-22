using ControleFuncionario.Model;
using System;

namespace ControleFuncionario.Model
{
    /// <summary>
    /// Representa os dados do funcionaroio
    /// </summary>
    public enum Permissoes
    {
        funcionario = 1,
        lider = 2,
        diretor = 3
    }
    public static class EnumExtensions
    {
        public static bool TryGetPermissao(string value, out Permissoes permissao)
        {
            return Enum.TryParse(value, out permissao);
        }
    }
}

