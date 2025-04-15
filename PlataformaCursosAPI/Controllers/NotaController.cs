using Microsoft.AspNetCore.Mvc;
using PlataformaCursosAPI.Data;
using PlataformaCursosAPI.Models;

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

        [HttpPost]
        public IActionResult CriarNota([FromBody] Nota nota)
        {
            _context.Nota.Add(nota);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterPorId), new { id = nota.Id }, nota);
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            var nota = _context.Nota.Find(id);
            if (nota == null) return NotFound();
            return Ok(nota);
        }

        [HttpGet("aluno/{alunoId}")]
        public IActionResult ObterPorAluno(int alunoId)
        {
            var notas = _context.Nota
                .Where(n => n.AlunoId == alunoId)
                .ToList();
            return Ok(notas);
        }

        [HttpGet("curso/{cursoId}")]
        public IActionResult ObterPorCurso(int cursoId)
        {
            var notas = _context.Nota
                .Where(n => n.CursoId == cursoId)
                .ToList();
            return Ok(notas);
        }
    }
}
