using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaCursosAPI.Data;
using PlataformaCursosAPI.Models;
using System.Text.Json;
using static PlataformaCursosAPI.Models.Matricula;

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

            // Verifica se a pessoa existe e é do tipo Aluno (TipoUsuario = 1)
            var alunoExiste = await _context.Pessoas.AnyAsync(p => p.Id == matricula.PessoaId && p.TipoUsuario == TipoUsuario.Aluno);

            if (!alunoExiste)
                return BadRequest("O aluno informado não existe.");

            // Verifica se a turma existe
            var turma = await _context.Turmas
                .Include(t => t.Curso)
                .FirstOrDefaultAsync(t => t.Id == matricula.TurmaId);
            if (turma == null)
                return BadRequest("A turma informada não existe.");

            var novaMatricula = new Matricula
            {
                PessoaId = matricula.PessoaId,
                TurmaId = matricula.TurmaId,
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
                .Include(m => m.Pessoa)
                .Include(m => m.Turma)
                    .ThenInclude(t => t.Curso)
                .Where(m => m.Id == id)
                .Select(m => new
                {
                    m.Id,
                    AlunoNome = m.Pessoa.Nome,
                    TurmaNome = m.Turma.Nome,
                    CursoNomeDaTurma = m.Turma.Curso.Nome,
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
                .Include(m => m.Pessoa)
                .Include(m => m.Turma)
                    .ThenInclude(t => t.Curso)
                .Select(m => new
                {
                    m.Id,
                    AlunoNome = m.Pessoa.Nome,
                    TurmaNome = m.Turma.Nome,
                    CursoNomeDaTurma = m.Turma.Curso.Nome,
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

            var matriculaExistente = await _context.Matriculas.FindAsync(id);
            if (matriculaExistente == null)
                return NotFound("Matrícula não encontrada.");

            // Verifica se a pessoa existe e é do tipo Aluno
            var alunoExiste = await _context.Pessoas.AnyAsync(p => p.Id == matriculaAtualizada.PessoaId && p.TipoUsuario == TipoUsuario.Aluno);
            if (!alunoExiste)
                return BadRequest("O aluno informado não existe.");

            // Verifica se a turma existe
            var turmaExiste = await _context.Turmas.AnyAsync(t => t.Id == matriculaAtualizada.TurmaId);
            if (!turmaExiste)
                return BadRequest("A turma informada não existe.");

            // Atualiza os dados da matrícula
            matriculaExistente.PessoaId = matriculaAtualizada.PessoaId;
            matriculaExistente.TurmaId = matriculaAtualizada.TurmaId;
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
