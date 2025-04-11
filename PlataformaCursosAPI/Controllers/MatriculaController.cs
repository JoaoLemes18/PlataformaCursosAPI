using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaCursosAPI.Data;
using PlataformaCursosAPI.Models;
using System.Threading.Tasks;
using System.Linq;

namespace PlataformaCursosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatriculaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MatriculaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/matricula
        [HttpPost]
        public async Task<IActionResult> CriarMatricula([FromBody] Matricula matricula)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Criando uma nova instância sem validar Aluno e Curso
            var novaMatricula = new Matricula
            {
                AlunoId = matricula.AlunoId,
                CursoId = matricula.CursoId,
                Status = matricula.Status,
                DataMatricula = DateTime.UtcNow
            };

            _context.Matricula.Add(novaMatricula);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterMatriculaPorId), new { id = novaMatricula.Id }, novaMatricula);
        }

        // GET: api/matricula/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterMatriculaPorId(int id)
        {
            var matricula = await _context.Matricula
                .Where(m => m.Id == id)
                .Select(m => new
                {
                    m.Id,
                    Aluno = _context.Alunos.Where(a => a.Id == m.AlunoId).Select(a => a.Nome).FirstOrDefault(),
                    Curso = _context.Cursos.Where(c => c.Id == m.CursoId).Select(c => c.Nome).FirstOrDefault(),
                    m.Status,
                    m.DataMatricula
                })
                .FirstOrDefaultAsync();

            if (matricula == null)
            {
                return NotFound();
            }

            return Ok(matricula);
        }

        // GET: api/matricula
        [HttpGet]
        public async Task<IActionResult> ListarMatriculas()
        {
            var matriculas = await _context.Matricula
                .Select(m => new
                {
                    m.Id,
                    Aluno = _context.Alunos.Where(a => a.Id == m.AlunoId).Select(a => a.Nome).FirstOrDefault(),
                    Curso = _context.Cursos.Where(c => c.Id == m.CursoId).Select(c => c.Nome).FirstOrDefault(),
                    m.Status,
                    m.DataMatricula
                })
                .ToListAsync();

            return Ok(matriculas);
        }

        // DELETE: api/matricula/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirMatricula(int id)
        {
            var matricula = await _context.Matricula.FindAsync(id);
            if (matricula == null)
            {
                return NotFound();
            }

            _context.Matricula.Remove(matricula);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/matricula/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarMatricula(int id, [FromBody] Matricula matriculaAtualizada)
        {
            if (matriculaAtualizada == null || id != matriculaAtualizada.Id)
            {
                return BadRequest("Dados inválidos.");
            }

            var matriculaExistente = await _context.Matricula.FindAsync(id);
            if (matriculaExistente == null)
            {
                return NotFound("Matrícula não encontrada.");
            }

            matriculaExistente.Status = matriculaAtualizada.Status;

            _context.Matricula.Update(matriculaExistente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
