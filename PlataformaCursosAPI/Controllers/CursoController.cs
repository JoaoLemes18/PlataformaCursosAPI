using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaCursosAPI.Models;

namespace PlataformaCursosAPI.Controllers
{
    /// <summary>
    /// Controlador para operações relacionadas aos cursos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CursoController : ControllerBase
    {
        private readonly Data.ApplicationDbContext _context;

        /// <summary>
        /// Construtor do CursoController.
        /// </summary>
        /// <param name="context">Contexto do banco de dados.</param>
        public CursoController(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna a lista de todos os cursos.
        /// </summary>
        /// <returns>Lista de cursos.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Curso>>> GetCursos()
        {
            return await _context.Cursos.ToListAsync();
        }

        /// <summary>
        /// Retorna um curso específico pelo ID.
        /// </summary>
        /// <param name="id">ID do curso.</param>
        /// <returns>Curso correspondente ao ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Curso>> GetCurso(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);

            if (curso == null)
            {
                return NotFound();
            }

            return curso;
        }

        /// <summary>
        /// Cria um novo curso.
        /// </summary>
        /// <param name="curso">Dados do curso a ser criado.</param>
        /// <returns>Curso criado com sucesso.</returns>
        [HttpPost]
        public async Task<ActionResult<Curso>> PostCurso(Curso curso)
        {
            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCurso), new { id = curso.Id }, curso);
        }

        /// <summary>
        /// Atualiza um curso existente.
        /// </summary>
        /// <param name="id">ID do curso a ser atualizado.</param>
        /// <param name="curso">Dados atualizados do curso.</param>
        /// <returns>Resposta sem conteúdo se a atualização for bem-sucedida.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurso(int id, Curso curso)
        {
            if (id != curso.Id)
            {
                return BadRequest();
            }

            _context.Entry(curso).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Exclui um curso pelo ID.
        /// </summary>
        /// <param name="id">ID do curso a ser excluído.</param>
        /// <returns>Resposta sem conteúdo se a exclusão for bem-sucedida.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurso(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
