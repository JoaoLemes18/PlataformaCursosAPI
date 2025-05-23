using System;

namespace PlataformaCursosAPI.Models
{
    public class Material
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CaminhoArquivo { get; set; } // caminho do arquivo armazenado
        public DateTime DataEnvio { get; set; }

        public int TurmaId { get; set; }
        public Turma Turma { get; set; }

      
    }
}
