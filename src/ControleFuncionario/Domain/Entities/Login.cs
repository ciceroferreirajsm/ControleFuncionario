using ControleFuncionario.Model;
using System;

namespace ControleFuncionario.Domain.Entities
{
    public class Login
    {
        public int id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public Permissoes Permissao { get; set; }
    }
}
