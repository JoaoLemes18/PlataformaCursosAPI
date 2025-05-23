using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using System.ComponentModel.DataAnnotations.Schema;

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
