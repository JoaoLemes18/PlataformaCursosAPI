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
        /// <returns>Lista de turmas.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Turma>>> Get()
        {
            var turmas = await _context.Turmas.ToListAsync();
            return Ok(turmas);
        }

        /// <summary>
        /// Obtém uma turma pelo seu ID.
        /// </summary>
        /// <param name="id">ID da turma.</param>
        /// <returns>Dados da turma ou NotFound.</returns>
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
        /// <param name="novaTurma">Objeto Turma com os dados.</param>
        /// <returns>Turma criada ou erro de validação.</returns>
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
        /// <summary>
        /// Atualiza uma turma existente.
        /// </summary>
        /// <param name="id">ID da turma.</param>
        /// <param name="turmaAtualizada">Dados atualizados da turma.</param>
        /// <returns>Turma atualizada ou erro.</returns>
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


        /// <summary>
        /// Remove uma turma existente pelo seu ID.
        /// </summary>
        /// <param name="id">ID da turma a ser removida.</param>
        /// <returns>
        /// Retorna NoContent se a exclusão for bem sucedida.
        /// Retorna NotFound se a turma não for encontrada.
        /// </returns>
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
