using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaCursosAPI.Data;
using PlataformaCursosAPI.Models;
using System.Text.Json;

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

        // 📌 Criar uma nova matrícula
        [HttpPost]
        public async Task<IActionResult> CriarMatricula([FromBody] Matricula matricula)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verifica se o aluno existe
            var alunoExiste = await _context.Alunos.AnyAsync(a => a.Id == matricula.AlunoId);
            if (!alunoExiste)
                return BadRequest("O aluno informado não existe.");

            // Verifica se o curso existe
            var cursoExiste = await _context.Cursos.AnyAsync(c => c.Id == matricula.CursoId);
            if (!cursoExiste)
                return BadRequest("O curso informado não existe.");

            // Criando a nova matrícula
            var novaMatricula = new Matricula
            {
                AlunoId = matricula.AlunoId,
                CursoId = matricula.CursoId,
                Status = matricula.Status,
                DataMatricula = DateTime.UtcNow
            };

            _context.Matriculas.Add(novaMatricula);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterMatriculaPorId), new { id = novaMatricula.Id }, novaMatricula);
        }

        // 📌 Buscar matrícula por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterMatriculaPorId(int id)
        {
            var matricula = await _context.Matriculas
                .Include(m => m.Aluno)  // Carrega a entidade Aluno relacionada
                .Include(m => m.Curso)  // Carrega a entidade Curso relacionada
                .Where(m => m.Id == id)
                .Select(m => new
                {
                    m.Id,
                    AlunoNome = m.Aluno.Pessoa.Nome,  // Acessa o Nome através da Pessoa
                    CursoNome = m.Curso.Nome,
                    m.Status,
                    m.DataMatricula
                })
                .FirstOrDefaultAsync();

            if (matricula == null)
                return NotFound("Matrícula não encontrada.");

            return Ok(matricula);
        }

        // 📌 Listar todas as matrículas
        [HttpGet]
        public async Task<IActionResult> ListarMatriculas()
        {
            var matriculas = await _context.Matriculas
                .Include(m => m.Aluno)  // Inclui o Aluno
                .Include(m => m.Curso)  // Inclui o Curso
                .Select(m => new
                {
                    m.Id,
                    AlunoNome = m.Aluno.Pessoa.Nome,  // Acessa o Nome do Aluno através da Pessoa
                    CursoNome = m.Curso.Nome,
                    m.Status,
                    m.DataMatricula
                })
                .ToListAsync();

            return Ok(matriculas);
        }

        // 📌 Excluir matrícula
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirMatricula(int id)
        {
            var matricula = await _context.Matriculas.FindAsync(id);
            if (matricula == null)
                return NotFound("Matrícula não encontrada.");

            _context.Matriculas.Remove(matricula);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 📌 Atualizar matrícula completa
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarMatriculaCompleta(int id, [FromBody] Matricula matriculaAtualizada)
        {
            if (id != matriculaAtualizada.Id)
                return BadRequest("O ID informado não corresponde ao da matrícula.");

            // Verifica se a matrícula existe
            var matriculaExistente = await _context.Matriculas.FindAsync(id);
            if (matriculaExistente == null)
                return NotFound("Matrícula não encontrada.");

            // Verifica se o aluno existe
            var alunoExiste = await _context.Alunos.AnyAsync(a => a.Id == matriculaAtualizada.AlunoId);
            if (!alunoExiste)
                return BadRequest("O aluno informado não existe.");

            // Verifica se o curso existe
            var cursoExiste = await _context.Cursos.AnyAsync(c => c.Id == matriculaAtualizada.CursoId);
            if (!cursoExiste)
                return BadRequest("O curso informado não existe.");

            // Atualiza os dados da matrícula
            matriculaExistente.AlunoId = matriculaAtualizada.AlunoId;
            matriculaExistente.CursoId = matriculaAtualizada.CursoId;
            matriculaExistente.Status = matriculaAtualizada.Status;
            matriculaExistente.DataMatricula = matriculaAtualizada.DataMatricula;

            _context.Matriculas.Update(matriculaExistente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 📌 Atualizar apenas o status da matrícula
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> AtualizarStatusMatricula(int id, [FromBody] JsonElement dados)
        {
            if (!dados.TryGetProperty("status", out JsonElement statusElement) || !statusElement.TryGetInt32(out int novoStatus))
                return BadRequest("O campo 'status' é obrigatório e deve ser um número válido.");

            if (!Enum.IsDefined(typeof(StatusMatricula), novoStatus))
                return BadRequest("O status informado não é válido.");

            var matriculaExistente = await _context.Matriculas.FindAsync(id);
            if (matriculaExistente == null)
                return NotFound("Matrícula não encontrada.");

            matriculaExistente.Status = (StatusMatricula)novoStatus;

            _context.Matriculas.Update(matriculaExistente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
