﻿using System.Text.Json.Serialization;
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
        public int AlunoId { get; set; }

        [Required]
        public int CursoId { get; set; }

        [Required]
        [EnumDataType(typeof(StatusMatricula))]
        public StatusMatricula Status { get; set; }

        [Required]
        public DateTime DataMatricula { get; set; } = DateTime.Now;

        // Relações, mas não serializa com JSON
        [ForeignKey("AlunoId")]
        [JsonIgnore]
        [ValidateNever]

        public Aluno Aluno { get; set; }

        [ForeignKey("CursoId")]
        [JsonIgnore]
        [ValidateNever]

        public Curso Curso { get; set; }
    }

    // Enum para status da matrícula
    public enum StatusMatricula
    {
        Ativa = 1,
        Trancada = 2,
        Cancelada = 3
    }
}
