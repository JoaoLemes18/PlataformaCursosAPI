using Microsoft.AspNetCore.Mvc;
using PlataformaCursosAPI.Models;

namespace PlataformaCursosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunoController : ControllerBase
    {
        private static List<Aluno> alunos = new List<Aluno>();

        [HttpGet]
        public ActionResult<IEnumerable<Aluno>> GetAlunos()
        {
            return Ok(alunos);
        }

        [HttpGet("{id}")]
        public ActionResult<Aluno> GetAluno(int id)
        {
            var aluno = alunos.FirstOrDefault(a => a.Id == id);
            if (aluno == null)
            {
                return NotFound();
            }
            return Ok(aluno);
        }

        [HttpPost]
        public ActionResult<Aluno> PostAluno([FromBody] Aluno aluno)
        {
            aluno.Id = alunos.Count + 1; // Simula a criação de um Id
            alunos.Add(aluno);
            return CreatedAtAction(nameof(GetAluno), new { id = aluno.Id }, aluno);
        }

        [HttpPut("{id}")]
        public ActionResult PutAluno(int id, [FromBody] Aluno aluno)
        {
            var alunoExistente = alunos.FirstOrDefault(a => a.Id == id);
            if (alunoExistente == null)
            {
                return NotFound();
            }

            alunoExistente.Nome = aluno.Nome;
            alunoExistente.Email = aluno.Email;
            alunoExistente.DataNascimento = aluno.DataNascimento;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteAluno(int id)
        {
            var aluno = alunos.FirstOrDefault(a => a.Id == id);
            if (aluno == null)
            {
                return NotFound();
            }

            alunos.Remove(aluno);
            return NoContent();
        }
    }
}
