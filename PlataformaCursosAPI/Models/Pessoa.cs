namespace PlataformaCursosAPI.Models
{
    //classe base para Aluno e Professor
    public abstract class Pessoa
    {
        public required string Nome { get; set; }
        public int Idade { get; set; }

        //método que pode ser sobrescrito
        public abstract void Apresentar();
    }
}
