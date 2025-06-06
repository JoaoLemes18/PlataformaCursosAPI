using System.Text.Json.Serialization;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PlataformaCursosAPI.Models
{
    public class Matricula
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PessoaId { get; set; }  

        [Required]
        public int TurmaId { get; set; }

        [Required]
        [EnumDataType(typeof(StatusMatricula))]
        public StatusMatricula Status { get; set; } = StatusMatricula.Ativa;

        [Required]
        public DateTime DataMatricula { get; set; } = DateTime.Now;

        [ForeignKey("PessoaId")]
        [JsonIgnore]
        [ValidateNever]
        public Pessoa Pessoa { get; set; } 

        [ForeignKey("TurmaId")]
        [JsonIgnore]
        [ValidateNever]
        public Turma Turma { get; set; }

        public enum StatusMatricula
        {
            Ativa = 1,
            Trancada = 2,
            Cancelada = 3
        }
    }
}
