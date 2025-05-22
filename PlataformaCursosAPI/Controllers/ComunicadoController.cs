using Microsoft.AspNetCore.Mvc;
using PlataformaCursosAPI.Data;
using PlataformaCursosAPI.DTOs;
using PlataformaCursosAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class ComunicadoController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public ComunicadoController(ApplicationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    // POST api/comunicado/criar-com-imagem
    [HttpPost]
    [Route("CriarComImagem")]
    public async Task<IActionResult> CriarComImagem([FromForm] ComunicadoCreateDto dto)
    {
        try
        {
            string urlImagem = null;

            if (dto.Imagem != null && dto.Imagem.Length > 0)
            {
                var uploads = Path.Combine(_environment.WebRootPath ?? "", "imagens");

                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                var nomeArquivo = Guid.NewGuid().ToString() + Path.GetExtension(dto.Imagem.FileName);
                var caminhoCompleto = Path.Combine(uploads, nomeArquivo);

                using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                {
                    await dto.Imagem.CopyToAsync(stream);
                }

                urlImagem = $"/imagens/{nomeArquivo}";
            }

            var comunicado = new Comunicado
            {
                Titulo = dto.Titulo,
                Mensagem = dto.Mensagem,
                DataInicio = dto.DataInicio,
                DataFim = dto.DataFim,
                UrlImagem = urlImagem
            };

            _context.Comunicados.Add(comunicado);
            await _context.SaveChangesAsync();

            return Ok(comunicado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    // GET api/comunicado/ativos
    [HttpGet("ativos")]
    public async Task<IActionResult> ListarAtivos()
    {
        var agora = DateTime.UtcNow;

        var comunicados = _context.Comunicados
            .Where(c =>
                (c.DataInicio == null || c.DataInicio <= agora) &&
                (c.DataFim == null || c.DataFim >= agora))
            .ToList();

        return Ok(comunicados);
    }
}
