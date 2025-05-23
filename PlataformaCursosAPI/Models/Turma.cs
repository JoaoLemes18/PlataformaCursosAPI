using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;


namespace PlataformaCursosAPI.Models
{
    public class Turma
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int Capacidade { get; set; } = 8;

        // Relações
        public int CursoId { get; set; }

        [JsonIgnore]
        [ValidateNever]public Curso Curso { get; set; }

        public int ProfessorId { get; set; }

        [JsonIgnore]

        [ValidateNever]public Pessoa Professor { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
        [JsonIgnore]
        [ValidateNever]
        public ICollection<Material> Materiais { get; set; }

    }
}



