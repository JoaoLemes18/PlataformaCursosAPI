using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaCursosAPI.Data;
using PlataformaCursosAPI.Models;

namespace PlataformaCursosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProfessoresController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/professores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Professor>>> GetProfessores()
        {
            return await _context.Professores.Include(p => p.Curso).ToListAsync();
        }

        // GET: api/professores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Professor>> GetProfessor(int id)
        {
            var professor = await _context.Professores.Include(p => p.Curso)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (professor == null)
            {
                return NotFound();
            }

            return professor;
        }

        // POST: api/professores
        [HttpPost]
        public async Task<ActionResult<Professor>> PostProfessor(Professor professor)
        {
            var curso = await _context.Cursos.FindAsync(professor.CursoId);
            if (curso == null)
            {
                return BadRequest("Curso não encontrado!");
            }

            _context.Professores.Add(professor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProfessor), new { id = professor.Id }, professor);
        }

        // PUT: api/professores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfessor(int id, Professor professor)
        {
            if (id != professor.Id)
            {
                return BadRequest();
            }

            var curso = await _context.Cursos.FindAsync(professor.CursoId);
            if (curso == null)
            {
                return BadRequest("Curso não encontrado!");
            }

            _context.Entry(professor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Professores.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/professores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfessor(int id)
        {
            var professor = await _context.Professores.FindAsync(id);
            if (professor == null)
            {
                return NotFound();
            }

            _context.Professores.Remove(professor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
