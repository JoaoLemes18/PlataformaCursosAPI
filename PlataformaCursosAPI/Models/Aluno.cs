using System.Text.Json.Serialization;

namespace PlataformaCursosAPI.Models
{
    public class Aluno
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public int Idade { get; set; }
        [JsonIgnore]
        public List<Matricula>? Matriculas { get; set; }
    }
}