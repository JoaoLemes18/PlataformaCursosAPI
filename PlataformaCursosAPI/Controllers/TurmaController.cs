/// <summary>
/// Controller responsável por gerenciar as turmas do sistema.
/// </summary>

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaCursosAPI.Models;
using PlataformaCursosAPI.Data;

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

        /// <summary>
        /// Lista todas as turmas cadastradas.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetTurmas()
        {
            var turmas = await _context.Turmas
                .Include(t => t.Matriculas)
                .Select(t => new
                {
                    t.Id,
                    t.Nome,
                    t.Capacidade,
                    t.CursoId,
                    Matriculados = t.Matriculas.Count()
                })
                .ToListAsync();

            return Ok(turmas);
        }


        /// <summary>
        /// Obtém uma turma pelo seu ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Turma>> Get(int id)
        {
            var turma = await _context.Turmas.FindAsync(id);
            if (turma == null)
                return NotFound();
            return Ok(turma);
        }

        /// <summary>
        /// Cria uma nova turma.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Turma>> Post([FromBody] Turma novaTurma)
        {
            if (string.IsNullOrEmpty(novaTurma.Nome) || novaTurma.CursoId == 0 || novaTurma.ProfessorId == 0)
            {
                return BadRequest("Nome, CursoId e ProfessorId são obrigatórios.");
            }

            // Define capacidade máxima de 8 alunos
            if (novaTurma.Capacidade > 8)
                novaTurma.Capacidade = 8;

            _context.Turmas.Add(novaTurma);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = novaTurma.Id }, novaTurma);
        }

        /// <summary>
        /// Atualiza uma turma existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<Turma>> Put(int id, [FromBody] Turma turmaAtualizada)
        {
            var existingTurma = await _context.Turmas.FindAsync(id);
            if (existingTurma == null)
                return NotFound();

            existingTurma.Nome = turmaAtualizada.Nome ?? existingTurma.Nome;
            existingTurma.Descricao = turmaAtualizada.Descricao ?? existingTurma.Descricao;

            // Aplica capacidade máxima de 8 alunos
            if (turmaAtualizada.Capacidade != 0)
                existingTurma.Capacidade = turmaAtualizada.Capacidade > 8 ? 8 : turmaAtualizada.Capacidade;

            existingTurma.CursoId = turmaAtualizada.CursoId != 0 ? turmaAtualizada.CursoId : existingTurma.CursoId;
            existingTurma.ProfessorId = turmaAtualizada.ProfessorId != 0 ? turmaAtualizada.ProfessorId : existingTurma.ProfessorId;

            _context.Entry(existingTurma).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(existingTurma);
        }

        /// <summary>
        /// Remove uma turma existente pelo seu ID.
        /// </summary>
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
