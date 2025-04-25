using System.ComponentModel.DataAnnotations;

public class Nota
{
    [Key]
    public int Id { get; set; }

    public int MatriculaId { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataLancamento { get; set; }

    public int AlunoId { get; set; }
    public int CursoId { get; set; }
    public int ProfessorId { get; set; }
}
