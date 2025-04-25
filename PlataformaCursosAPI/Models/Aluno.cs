using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PlataformaCursosAPI.Models
{
    public class Aluno
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PessoaId { get; set; }

        public DateTime DataMatricula { get; set; } = DateTime.Now;
        public string Status { get; set; }

        [ForeignKey("PessoaId")]
        public Pessoa Pessoa { get; set; }
    }
}