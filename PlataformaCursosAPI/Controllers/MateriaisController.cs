using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaCursosAPI.Data;
using PlataformaCursosAPI.Models;

namespace PlataformaCursosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MateriaisController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public MateriaisController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadMaterial([FromForm] MaterialUploadDto dto)
        {
            if (dto.Arquivo == null || dto.Arquivo.Length == 0)
                return BadRequest("Arquivo não enviado.");

            // Defina onde vai salvar o arquivo (pasta materiais, por exemplo)
            var imagensPath = Path.Combine(_env.ContentRootPath, "materiais");
            if (!Directory.Exists(imagensPath))
                Directory.CreateDirectory(imagensPath);

            // Gere um nome único para evitar conflitos
            var fileName = $"{Guid.NewGuid()}_{dto.Arquivo.FileName}";
            var filePath = Path.Combine(imagensPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Arquivo.CopyToAsync(stream);
            }

            // Cria o registro no banco
            var material = new Material
            {
                Nome = dto.Nome,
                CaminhoArquivo = $"/materiais/{fileName}",
                DataEnvio = DateTime.UtcNow,
                TurmaId = dto.TurmaId
            };

            _context.Materiais.Add(material);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Upload realizado com sucesso", material.Id });
        }



        [HttpGet("turma/{turmaId}")]
        public async Task<IActionResult> ListarPorTurma(int turmaId)
        {
            var materiais = await _context.Materiais
                .Where(m => m.TurmaId == turmaId)
                .ToListAsync();

            return Ok(materiais);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var material = await _context.Materiais.FindAsync(id);
            if (material == null) return NotFound();

            var caminhoFisico = Path.Combine(_env.WebRootPath, material.CaminhoArquivo.TrimStart('/'));
            if (System.IO.File.Exists(caminhoFisico))
            {
                System.IO.File.Delete(caminhoFisico);
            }

            _context.Materiais.Remove(material);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
