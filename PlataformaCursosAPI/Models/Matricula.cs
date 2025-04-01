using PlataformaCursosAPI.Models;

public class Matricula
{
    public int Id { get; set; }
    public int AlunoId { get; set; }
    public int CursoId { get; set; }
    public DateTime DataMatricula { get; set; }

    // Relacionamentos
    public Aluno Aluno { get; set; }
    public Curso Curso { get; set; }
}