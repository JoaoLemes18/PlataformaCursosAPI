namespace PlataformaCursosAPI.Models
{
    public class Comunicado
    { public int Id { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string? UrlImagem { get; set; }
    }
}
