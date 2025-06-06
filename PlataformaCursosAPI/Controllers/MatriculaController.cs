/// <summary>
/// Controller responsável pela gestão das matrículas dos alunos nas turmas.
/// </summary>

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

        /// <summary>
        /// Cria uma nova matrícula para um aluno em uma turma.
        /// </summary>
        /// <param name="matricula">Objeto Matricula contendo PessoaId, TurmaId e Status.</param>
        /// <returns>Retorna Created (201) com os dados da matrícula criada.</returns>
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


        /// <summary>
        /// Obtém os dados de uma matrícula específica pelo seu ID.
        /// </summary>
        /// <param name="id">ID da matrícula.</param>
        /// <returns>Retorna os dados da matrícula ou NotFound se não existir.</returns>
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


        /// <summary>
        /// Lista todas as matrículas cadastradas.
        /// </summary>
        /// <returns>Retorna uma lista com todas as matrículas e seus detalhes.</returns>
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

        /// <summary>
        /// Exclui uma matrícula pelo seu ID.
        /// </summary>
        /// <param name="id">ID da matrícula a ser excluída.</param>
        /// <returns>Retorna NoContent (204) se excluído com sucesso ou NotFound.</returns>
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


        /// <summary>
        /// Atualiza completamente os dados de uma matrícula existente.
        /// </summary>
        /// <param name="id">ID da matrícula a ser atualizada.</param>
        /// <param name="matriculaAtualizada">Objeto Matricula com os novos dados.</param>
        /// <returns>Retorna NoContent (204) se atualizado com sucesso, BadRequest ou NotFound em casos de erro.</returns>
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

        /// <summary>
        /// Atualiza somente o status de uma matrícula existente.
        /// </summary>
        /// <param name="id">ID da matrícula.</param>
        /// <param name="dados">JSON contendo o campo "status" com o novo status.</param>
        /// <returns>Retorna NoContent (204) se atualizado com sucesso, BadRequest ou NotFound em casos de erro.</returns>
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


        /// <summary>
        /// Lista as turmas em que uma pessoa (aluno) está matriculada.
        /// </summary>
        /// <param name="pessoaId">ID da pessoa (deve ser aluno).</param>
        /// <returns>Retorna lista de turmas ou erro caso pessoa não exista ou não seja aluno.</returns>
        [HttpGet("pessoa/{pessoaId}/turmas")]
        public async Task<IActionResult> ListarTurmasDaPessoa(int pessoaId)
        {
            // Verifica se a pessoa existe
            var pessoa = await _context.Pessoas.FindAsync(pessoaId);
            if (pessoa == null)
                return NotFound("Pessoa não encontrada.");

            // TipoUsuario = 1 → Aluno
            if ((int)pessoa.TipoUsuario != 1)
                return BadRequest("A pessoa informada não é um aluno.");

            // Busca as turmas onde essa pessoa está matriculada
            var turmas = await _context.Matriculas
                .Where(m => m.PessoaId == pessoaId)
                .Select(m => new
                {
                    m.Turma.Id,
                    m.Turma.Nome
                })
                .Distinct()
                .ToListAsync();

            return Ok(turmas);
        }

/// <summary>
/// Lista os materiais disponíveis de uma turma específica para um aluno específico.
/// </summary>
/// <param name="pessoaId">ID da pessoa (aluno).</param>
/// <param name="turmaId">ID da turma.</param>
/// <returns>
/// Retorna a lista de materiais da turma se o aluno estiver matriculado.
/// Retorna NotFound se a pessoa não existir.
/// Retorna BadRequest se a pessoa não for do tipo aluno.
/// Retorna Forbid se o aluno não estiver matriculado na turma.
/// </returns>        
[HttpGet("pessoa/{pessoaId}/turma/{turmaId}/materiais")]
        public async Task<IActionResult> ListarMateriaisDaTurmaDoAluno(int pessoaId, int turmaId)
        {
            // Verifica se a pessoa existe e é aluno
            var pessoa = await _context.Pessoas.FindAsync(pessoaId);
            if (pessoa == null)
                return NotFound("Pessoa não encontrada.");

            if ((int)pessoa.TipoUsuario != 1) // 1 = Aluno
                return BadRequest("A pessoa informada não é um aluno.");

            // Verifica se o aluno está matriculado na turma
            var matriculado = await _context.Matriculas
                .AnyAsync(m => m.PessoaId == pessoaId && m.TurmaId == turmaId);

            if (!matriculado)
                return Forbid("Aluno não matriculado nesta turma.");

            // Busca os materiais da turma
            var materiais = await _context.Materiais
                .Where(m => m.TurmaId == turmaId)
                .Select(m => new
                {
                    m.Id,
                    m.Nome,           // nome do material
                    CaminhoArquivo = m.CaminhoArquivo, // caminho do arquivo
                    m.DataEnvio
                })
                .ToListAsync();

            return Ok(materiais);
        }
    }
}
