using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaCursosAPI.Data;
using PlataformaCursosAPI.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaCursosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NotaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Criar Nota
        [HttpPost]
        public async Task<IActionResult> CriarNota([FromBody] Nota nota)
        {
            if (nota == null)
            {
                return BadRequest("Nota não pode ser nula.");
            }

            // Verifica se o aluno existe
            var alunoExistente = await _context.Alunos.FindAsync(nota.AlunoId);
            if (alunoExistente == null)
            {
                return BadRequest("Aluno não encontrado.");
            }

            // Verifica se o curso existe
            var cursoExistente = await _context.Cursos.FindAsync(nota.CursoId);
            if (cursoExistente == null)
            {
                return BadRequest("Curso não encontrado.");
            }

            _context.Notas.Add(nota);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObterPorId), new { id = nota.Id }, nota);
        }

        // Obter Nota por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var nota = await _context.Notas.FindAsync(id);
            if (nota == null) return NotFound();
            return Ok(nota);
        }

        // Obter Notas por Aluno
        [HttpGet("aluno/{alunoId}")]
        public async Task<IActionResult> ObterPorAluno(int alunoId)
        {
            var notas = await _context.Notas
                .Where(n => n.AlunoId == alunoId)
                .ToListAsync();

            if (notas == null || !notas.Any())
            {
                return NotFound("Nenhuma nota encontrada para o aluno.");
            }

            return Ok(notas);
        }

        // Obter Notas por Curso
        [HttpGet("curso/{cursoId}")]
        public async Task<IActionResult> ObterPorCurso(int cursoId)
        {
            var notas = await _context.Notas
                .Where(n => n.CursoId == cursoId)
                .ToListAsync();

            if (notas == null || !notas.Any())
            {
                return NotFound("Nenhuma nota encontrada para o curso.");
            }

            return Ok(notas);
        }
    }
}
