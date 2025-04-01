using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PlataformaCursosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatriculaController : ControllerBase
    {
        private readonly Data.ApplicationDbContext _context;

        public MatriculaController(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // 📌 1️⃣ Cadastrar Matrícula
        [HttpPost]
        public async Task<ActionResult<Matricula>> CadastrarMatricula([FromBody] Matricula matricula)
        {
            if (matricula.DataMatricula == DateTime.MinValue)
            {
                return BadRequest("Data da matrícula inválida.");
            }

            var aluno = await _context.Alunos.FindAsync(matricula.AlunoId);
            var curso = await _context.Cursos.FindAsync(matricula.CursoId);

            if (aluno == null || curso == null)
            {
                return BadRequest("Aluno ou curso não encontrados.");
            }

            _context.Matriculas.Add(matricula);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMatricula), new { id = matricula.Id }, matricula);
        }

        // 📌 2️⃣ Listar Todas as Matrículas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Matricula>>> GetMatriculas()
        {
            var matriculas = await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .ToListAsync();

            return Ok(matriculas);
        }

        // 📌 3️⃣ Buscar Matrícula por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Matricula>> GetMatricula(int id)
        {
            var matricula = await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (matricula == null)
            {
                return NotFound("Matrícula não encontrada.");
            }

            return Ok(matricula);
        }

        // 📌 4️⃣ Listar Matrículas de um Aluno Específico
        [HttpGet("aluno/{alunoId}")]
        public async Task<ActionResult<IEnumerable<Matricula>>> GetMatriculasPorAluno(int alunoId)
        {
            var aluno = await _context.Alunos.FindAsync(alunoId);
            if (aluno == null)
            {
                return NotFound("Aluno não encontrado.");
            }

            var matriculas = await _context.Matriculas
                .Where(m => m.AlunoId == alunoId)
                .Include(m => m.Curso)
                .ToListAsync();

            return Ok(matriculas);
        }

        // 📌 5️⃣ Listar Alunos Matriculados em um Curso
        [HttpGet("curso/{cursoId}")]
        public async Task<ActionResult<IEnumerable<Matricula>>> GetAlunosPorCurso(int cursoId)
        {
            var curso = await _context.Cursos.FindAsync(cursoId);
            if (curso == null)
            {
                return NotFound("Curso não encontrado.");
            }

            var matriculas = await _context.Matriculas
                .Where(m => m.CursoId == cursoId)
                .Include(m => m.Aluno)
                .ToListAsync();

            return Ok(matriculas);
        }

        // 📌 6️⃣ Excluir uma Matrícula
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatricula(int id)
        {
            var matricula = await _context.Matriculas.FindAsync(id);
            if (matricula == null)
            {
                return NotFound("Matrícula não encontrada.");
            }

            _context.Matriculas.Remove(matricula);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}