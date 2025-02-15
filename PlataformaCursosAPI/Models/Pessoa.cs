namespace PlataformaCursosAPI.Models
{
    // Classe base para Aluno e Professor
    public abstract class Pessoa
    {
        public required string Nome { get; set; }
        public int Idade { get; set; }

        // Método que pode ser sobrescrito
        public abstract void Apresentar();
    }
}
