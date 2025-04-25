namespace PlataformaCursosAPI.DTOs
{
    public class CadastroDto
    {
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Senha { get; set; }
        public int TipoUsuario { get; set; }  // O tipo de usuário será um inteiro (1, 2, 3, ...)
    }
}
