namespace PlataformaCursosAPI.Models
{
    public class Aluno : Pessoa, IIdentificavel
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public DateTime DataNascimento { get; set; }

        // Nova relação com Curso
        public int CursoId { get; set; }
        public Curso? Curso { get; set; }

        public override void Apresentar()
        {
            Console.WriteLine($"Olá, sou o(a) aluno(a) {Nome}. Tenho {Idade} anos, meu e-mail é {Email} e meu nascimento foi em {DataNascimento.ToShortDateString()}.");
        }

        public string ObterIdentificacao() => $"Aluno: {Nome}, Email: {Email}, Nascimento: {DataNascimento.ToShortDateString()}";
    }
}
