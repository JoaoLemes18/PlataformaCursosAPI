namespace PlataformaCursosAPI.Models
{
    public class Professor : Pessoa, IIdentificavel
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string AreaEspecializacao { get; set; }

        // Adicionando relacionamento com Curso
        public int CursoId { get; set; }  // Chave estrangeira
        public required Curso Curso { get; set; }  // Relação com a classe Curso

        // Sobrescrevendo o método da classe base
        public override void Apresentar()
        {
            Console.WriteLine($"Olá, sou o(a) professor(a) {Nome}. Minha área de especialização é {AreaEspecializacao}. Meu e-mail é {Email} e leciono o curso de {Curso.Nome}.");
        }

        // Implementando o método da interface IIdentificavel
        public string ObterIdentificacao() => $"Professor: {Nome}, Email: {Email}, Especialização: {AreaEspecializacao}, Curso: {Curso.Nome}";
    }
}
