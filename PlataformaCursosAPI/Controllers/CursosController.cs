using Microsoft.AspNetCore.Mvc;
using PlataformaCursosAPI.Models;

namespace PlataformaCursosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursoController : ControllerBase
    {
        private static List<Curso> cursos = new List<Curso>();

        // Adiciona um curso
        [HttpPost]
        public ActionResult<Curso> PostCurso([FromBody] Curso curso)
        {
            curso.Id = cursos.Count + 1; // Simula a criação de um Id
            cursos.Add(curso);
            return CreatedAtAction(nameof(GetCurso), new { id = curso.Id }, curso);
        }

        // Recupera todos os cursos
        [HttpGet]
        public ActionResult<IEnumerable<Curso>> GetCursos()
        {
            return Ok(cursos);
        }

        // Recupera um curso por ID
        [HttpGet("{id}")]
        public ActionResult<Curso> GetCurso(int id)
        {
            var curso = cursos.FirstOrDefault(c => c.Id == id);
            if (curso == null)
            {
                return NotFound();
            }
            return Ok(curso);
        }

        // Adiciona um aluno a um curso
        [HttpPost("{idCurso}/aluno")]
        public ActionResult AdicionarAlunoAoCurso(int idCurso, [FromBody] Aluno aluno)
        {
            var curso = cursos.FirstOrDefault(c => c.Id == idCurso);
            if (curso == null)
            {
                return NotFound();
            }

            curso.AdicionarAluno(aluno);
            return NoContent();
        }

        // Adiciona um professor a um curso
        [HttpPost("{idCurso}/professor")]
        public ActionResult AdicionarProfessorAoCurso(int idCurso, [FromBody] Professor professor)
        {
            var curso = cursos.FirstOrDefault(c => c.Id == idCurso);
            if (curso == null)
            {
                return NotFound();
            }

            curso.AdicionarProfessor(professor);
            return NoContent();
        }
    }
}
