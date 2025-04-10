namespace PlataformaCursosAPI.Models
{
    public class Professor
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public string Email { get; set; }
        public string AreaEspecializacao { get; set; }

        // Chave estrangeira para Curso
        public int CursoId { get; set; }
        public Curso? Curso { get; set; } // Propriedade de navegação
    }
}
