using System.ComponentModel.DataAnnotations;

namespace PlataformaCursosAPI.Models
{
    public class Pessoa
    {
       
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string CPF { get; set; }

        [Required]
        public string Email { get; set; }

        public string Telefone { get; set; }

        [Required]
        public TipoUsuario TipoUsuario { get; set; }  // Verifique se isso existe na sua classe

        [Required]
        public string SenhaHash { get; set; }  // Senha armazenada como hash
    }

    public enum TipoUsuario
    {
        Aluno = 1,
        Professor = 2,
        Coordenador = 3,
        Administrativo = 4,
        Master = 5
    }
}
