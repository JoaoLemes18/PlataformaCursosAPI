namespace PlataformaCursosAPI.Models
{
    // Aluno herda de Pessoa e implementa IIdentificavel
    public class Aluno : Pessoa, IIdentificavel
    {
        // Propriedades específicas de Aluno
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }

        // Sobrescrevendo o método da classe base
        public override void Apresentar()
        {
            Console.WriteLine($"Olá, sou o(a) aluno(a) {Nome}. Tenho {Idade} anos, meu e-mail é {Email} e meu nascimento foi em {DataNascimento.ToShortDateString()}.");
        }

        // Implementando o método da interface IIdentificavel
        public string ObterIdentificacao() => $"Aluno: {Nome}, Email: {Email}, Nascimento: {DataNascimento.ToShortDateString()}";
    }
}
