public class Nota
{
    public int Id { get; set; }
    public int MatriculaId { get; set; } // Apenas o ID da matrícula
    public decimal Valor { get; set; }
    public DateTime DataLancamento { get; set; }

    // Apenas IDs
    public int AlunoId { get; set; }
    public int CursoId { get; set; }
    public int ProfessorId { get; set; }
}
