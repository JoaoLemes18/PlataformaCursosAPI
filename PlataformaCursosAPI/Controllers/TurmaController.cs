using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaCursosAPI.Models;
using PlataformaCursosAPI.Data; // Ajuste conforme sua pasta de Data (DbContext)

namespace PlataformaCursosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TurmaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TurmaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET api/turma
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Turma>>> Get()
        {
            var turmas = await _context.Turmas.ToListAsync();
            return Ok(turmas);
        }

        // GET api/turma/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Turma>> Get(int id)
        {
            var turma = await _context.Turmas.FindAsync(id);
            if (turma == null)
                return NotFound();
            return Ok(turma);
        }

        // POST api/turma
        [HttpPost]
        public async Task<ActionResult<Turma>> Post([FromBody] Turma novaTurma)
        {
            if (string.IsNullOrEmpty(novaTurma.Nome) || novaTurma.CursoId == 0 || novaTurma.ProfessorId == 0)
            {
                return BadRequest("Nome, CursoId e ProfessorId são obrigatórios.");
            }

            _context.Turmas.Add(novaTurma);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = novaTurma.Id }, novaTurma);
        }

        // PUT api/turma/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<Turma>> Put(int id, [FromBody] Turma turmaAtualizada)
        {
            var existingTurma = await _context.Turmas.FindAsync(id);
            if (existingTurma == null)
                return NotFound();

            existingTurma.Nome = turmaAtualizada.Nome ?? existingTurma.Nome;
            existingTurma.Descricao = turmaAtualizada.Descricao ?? existingTurma.Descricao;
            existingTurma.Capacidade = turmaAtualizada.Capacidade != 0 ? turmaAtualizada.Capacidade : existingTurma.Capacidade;
            existingTurma.CursoId = turmaAtualizada.CursoId != 0 ? turmaAtualizada.CursoId : existingTurma.CursoId;
            existingTurma.ProfessorId = turmaAtualizada.ProfessorId != 0 ? turmaAtualizada.ProfessorId : existingTurma.ProfessorId;

            _context.Entry(existingTurma).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(existingTurma);
        }

        // DELETE api/turma/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existingTurma = await _context.Turmas.FindAsync(id);
            if (existingTurma == null)
                return NotFound();

            _context.Turmas.Remove(existingTurma);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
