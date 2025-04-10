using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaCursosAPI.Models;
using PlataformaCursosAPI.Data;

namespace PlataformaCursosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProfessorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/professor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Professor>>> GetProfessores()
        {
            return await _context.Professores
                .Include(p => p.Curso) // Carrega os dados do curso associado
                .ToListAsync();
        }

        // GET: api/professor/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Professor>> GetProfessor(int id)
        {
            var professor = await _context.Professores
                .Include(p => p.Curso) // Carrega o curso associado ao professor
                .FirstOrDefaultAsync(p => p.Id == id);

            if (professor == null)
            {
                return NotFound();
            }

            return professor;
        }

        // POST: api/professor
        [HttpPost]
        public async Task<ActionResult<Professor>> PostProfessor(Professor professor)
        {
            // Verifica se o CursoId informado existe
            var cursoExistente = await _context.Cursos.FindAsync(professor.CursoId);
            if (cursoExistente == null)
            {
                return BadRequest("Curso não encontrado.");
            }

            _context.Professores.Add(professor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProfessor), new { id = professor.Id }, professor);
        }

        // PUT: api/professor/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfessor(int id, Professor professor)
        {
            if (id != professor.Id)
            {
                return BadRequest();
            }

            _context.Entry(professor).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/professor/{id}
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
