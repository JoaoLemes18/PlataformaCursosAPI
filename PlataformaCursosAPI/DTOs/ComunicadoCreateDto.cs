namespace PlataformaCursosAPI.DTOs
{
    public class ComunicadoCreateDto
    {
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public IFormFile Imagem { get; set; }
    }
}
