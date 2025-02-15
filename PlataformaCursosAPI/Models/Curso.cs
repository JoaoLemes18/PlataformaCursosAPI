namespace PlataformaCursosAPI.Models
{
    public class Curso
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Descricao { get; set; }
        public int DuracaoEmHoras { get; set; }

        // Lista de alunos e professores associados ao curso
        public List<Aluno> Alunos { get; set; } = new List<Aluno>();
        public List<Professor> Professores { get; set; } = new List<Professor>();

        // Adiciona um aluno ao curso
        public void AdicionarAluno(Aluno aluno)
        {
            Alunos.Add(aluno);
        }

        // Adiciona um professor ao curso
        public void AdicionarProfessor(Professor professor)
        {
            Professores.Add(professor);
        }

        // Método para exibir informações do curso
        public void ExibirDetalhes()
        {
            Console.WriteLine($"Curso: {Nome}, Descrição: {Descricao}, Duração: {DuracaoEmHoras} horas.");
            Console.WriteLine("Professores do curso:");
            foreach (var professor in Professores)
            {
                professor.Apresentar(); // Chama o método Apresentar de Professor
            }
            Console.WriteLine("Alunos matriculados:");
            foreach (var aluno in Alunos)
            {
                aluno.Apresentar(); // Chama o método Apresentar de Aluno
            }
        }
    }
}
