namespace PlataformaCursosAPI.Models
{
    // Professor herda de Pessoa e implementa IIdentificavel
    public class Professor : Pessoa, IIdentificavel
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string AreaEspecializacao { get; set; }

        // Sobrescrevendo o método da classe base
        public override void Apresentar()
        {
            Console.WriteLine($"Olá, sou o(a) professor(a) {Nome}. Minha área de especialização é {AreaEspecializacao}. Meu e-mail é {Email}.");
        }

        // Implementando o método da interface IIdentificavel
        public string ObterIdentificacao() => $"Professor: {Nome}, Email: {Email}, Especialização: {AreaEspecializacao}";
    }
}
