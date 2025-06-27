/// <summary>
/// Controller responsável por gerenciar as pessoas (usuários) do sistema.
/// </summary>

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaCursosAPI.Data;
using PlataformaCursosAPI.DTOs;
using PlataformaCursosAPI.Helpers;
using PlataformaCursosAPI.Models;
using static PlataformaCursosAPI.Models.Matricula;

[ApiController]
[Route("api/[controller]")]
public class PessoaController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PessoaController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Cadastra uma nova pessoa no sistema.
    /// </summary>
    [HttpPost("cadastrar")]
    public async Task<IActionResult> Cadastrar([FromBody] CadastroDto cadastro)
    {
        if (!Enum.IsDefined(typeof(TipoUsuario), cadastro.TipoUsuario))
            return BadRequest("Tipo de usuário inválido.");

        var tipoUsuario = (TipoUsuario)cadastro.TipoUsuario;

        var usuarioExistente = await _context.Pessoas
            .FirstOrDefaultAsync(u => u.Email == cadastro.Email);

        if (usuarioExistente != null)
        {
            return BadRequest("Já existe um usuário com esse e-mail.");
        }

        var pessoa = new Pessoa
        {
            Nome = cadastro.Nome,
            CPF = cadastro.CPF,
            Email = cadastro.Email,
            Telefone = cadastro.Telefone,
            TipoUsuario = tipoUsuario,
            SenhaHash = SenhaHelper.GerarHash(cadastro.Senha)
        };

        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();

        return Ok(new { mensagem = "Cadastro realizado com sucesso." });
    }

    /// <summary>
    /// Realiza login de um usuário.
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await _context.Pessoas
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        if (user == null)
        {
            return Unauthorized("Usuário não encontrado.");
        }

        bool senhaCorreta = SenhaHelper.VerificarHash(loginDto.Senha, user.SenhaHash);

        if (!senhaCorreta)
        {
            return Unauthorized("Senha inválida.");
        }

        var matricula = await _context.Matriculas
            .Where(m => m.PessoaId == user.Id)
            .Select(m => m.TurmaId)
            .FirstOrDefaultAsync();

        return Ok(new
        {
            Id = user.Id,
            Nome = user.Nome,
            TipoUsuario = user.TipoUsuario,
            TurmaId = matricula
        });
    }

    /// <summary>
    /// Lista todas as pessoas.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ListarPessoas()
    {
        var pessoas = await _context.Pessoas
            .Select(p => new
            {
                p.Id,
                p.Nome,
                p.CPF,
                p.Email,
                p.Telefone,
                p.TipoUsuario
            })
            .ToListAsync();

        return Ok(pessoas);
    }

    /// <summary>
    /// Lista somente alunos.
    /// </summary>
    [HttpGet("alunos")]
    public async Task<IActionResult> ListarAlunos()
    {
        var alunos = await _context.Pessoas
            .Where(p => p.TipoUsuario == TipoUsuario.Aluno)
            .Select(p => new
            {
                p.Id,
                p.Nome,
                p.CPF,
                p.Email,
                p.Telefone,
                p.TipoUsuario
            })
            .ToListAsync();

        return Ok(alunos);
    }

    /// <summary>
    /// Lista somente professores.
    /// </summary>
    [HttpGet("professores")]
    public async Task<IActionResult> ListarProfessores()
    {
        var professores = await _context.Pessoas
            .Where(p => p.TipoUsuario == TipoUsuario.Professor)
            .Select(p => new
            {
                p.Id,
                p.Nome,
                p.CPF,
                p.Email,
                p.Telefone,
                p.TipoUsuario
            })
            .ToListAsync();

        return Ok(professores);
    }

    /// <summary>
    /// Atualiza os dados de uma pessoa existente.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> EditarPessoa(int id, [FromBody] PessoaEdicaoDto dto)
    {
        var pessoa = await _context.Pessoas.FindAsync(id);
        if (pessoa == null)
            return NotFound("Pessoa não encontrada.");

        pessoa.Nome = dto.Nome ?? pessoa.Nome;
        pessoa.CPF = dto.CPF ?? pessoa.CPF;
        pessoa.Email = dto.Email ?? pessoa.Email;
        pessoa.Telefone = dto.Telefone ?? pessoa.Telefone;
        pessoa.TipoUsuario = (TipoUsuario)dto.TipoUsuario;

        if (!string.IsNullOrEmpty(dto.Senha))
        {
            pessoa.SenhaHash = SenhaHelper.GerarHash(dto.Senha);
        }

        await _context.SaveChangesAsync();
        return Ok(new { mensagem = "Pessoa atualizada com sucesso." });
    }

    /// <summary>
    /// Exclui uma pessoa pelo seu ID, removendo também matrículas e vínculos de professor/aluno se existirem.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> ExcluirPessoa(int id)
    {
        var pessoa = await _context.Pessoas.FindAsync(id);
        if (pessoa == null)
            return NotFound("Pessoa não encontrada.");

        // ✅ Cancelar matrículas vinculadas antes
        var matriculas = await _context.Matriculas
            .Where(m => m.PessoaId == id)
            .ToListAsync();

        if (matriculas.Any())
        {
            foreach (var matricula in matriculas)
            {
                // Cancela a matrícula ao invés de excluir diretamente
                matricula.Status = StatusMatricula.Cancelada;
            }
            _context.Matriculas.UpdateRange(matriculas);
        }

        // ✅ Remove registro de Professor se existir
        var professor = await _context.Professores
            .FirstOrDefaultAsync(p => p.PessoaId == id);
        if (professor != null)
        {
            _context.Professores.Remove(professor);
        }

        // ✅ Remove registro de Aluno se existir
        var aluno = await _context.Alunos
            .FirstOrDefaultAsync(a => a.PessoaId == id);
        if (aluno != null)
        {
            _context.Alunos.Remove(aluno);
        }

        // ✅ Salva alterações intermediárias (matrículas/professor/aluno)
        await _context.SaveChangesAsync();

        // ✅ Exclui a pessoa após desvincular tudo
        _context.Pessoas.Remove(pessoa);
        await _context.SaveChangesAsync();

        return NoContent();
    }



}
