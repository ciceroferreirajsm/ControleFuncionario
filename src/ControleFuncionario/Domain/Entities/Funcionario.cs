using ControleFuncionario.Model;
using System;

namespace ControleFuncionario.Domain.Entities
{
    public class Funcionario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Cargo { get; set; }
        public string Documento { get; set; }
        public string Gestor { get; set; }
        public string Senha { get; set; }
        public DateTime DtNascimento { get; set; }
        public bool Ativo { get; set; }
        public Permissoes Permissao { get; set; }
    }
}
