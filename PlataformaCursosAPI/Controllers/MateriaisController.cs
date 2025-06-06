using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaCursosAPI.Data;
using PlataformaCursosAPI.Models;

namespace PlataformaCursosAPI.Controllers
{
    /// <summary>
    /// Controlador para gerenciamento de materiais (upload, listagem e exclusão).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MateriaisController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// Construtor do MateriaisController.
        /// </summary>
        /// <param name="context">Contexto do banco de dados.</param>
        /// <param name="env">Ambiente web para gerenciar caminhos físicos.</param>
        public MateriaisController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        /// <summary>
        /// Realiza o upload de um material e salva o arquivo no servidor.
        /// </summary>
        /// <param name="dto">Dados do material e arquivo enviado.</param>
        /// <returns>Mensagem de sucesso e ID do material criado.</returns>
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

        /// <summary>
        /// Lista todos os materiais associados a uma turma específica.
        /// </summary>
        /// <param name="turmaId">ID da turma.</param>
        /// <returns>Lista de materiais da turma.</returns>
        [HttpGet("turma/{turmaId}")]
        public async Task<IActionResult> ListarPorTurma(int turmaId)
        {
            var materiais = await _context.Materiais
                .Where(m => m.TurmaId == turmaId)
                .ToListAsync();

            return Ok(materiais);
        }

        /// <summary>
        /// Exclui um material pelo seu ID e remove o arquivo do servidor.
        /// </summary>
        /// <param name="id">ID do material a ser excluído.</param>
        /// <returns>Resposta sem conteúdo se a exclusão for bem-sucedida.</returns>
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
