namespace PlataformaCursosAPI.DTOs
{
    public class CadastroDto
    {
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Senha { get; set; }
        public int TipoUsuario { get; set; }  
    }
}
