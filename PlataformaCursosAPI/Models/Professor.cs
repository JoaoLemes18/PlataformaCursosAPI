using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PlataformaCursosAPI.Models
{
    public class Professor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PessoaId { get; set; }

        public string AreaEspecializacao { get; set; }

        [ForeignKey("PessoaId")]
        public Pessoa Pessoa { get; set; }

        public int CursoId { get; set; }
        public Curso Curso
        {
            get; set;
        }
    }
}
