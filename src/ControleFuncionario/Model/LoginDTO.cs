using System;

namespace ControleFuncionario.Model
{
    /// <summary>
    /// Representa os dados do funcionaroio
    /// </summary>
    public class LoginDTO
    {
        public int id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Permissao { get; set; }
    }
}
