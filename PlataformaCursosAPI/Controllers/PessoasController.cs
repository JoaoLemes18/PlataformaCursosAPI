/// <summary>
/// Controller responsável por gerenciar as pessoas (usuários) do sistema.
/// </summary>

using Microsoft.AspNetCore.Mvc;
using PlataformaCursosAPI.Data;
using PlataformaCursosAPI.Models;
using PlataformaCursosAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using PlataformaCursosAPI.Helpers;

[ApiController]
[Route("api/[controller]")]
public class PessoaController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PessoaController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Rota para cadastro de pessoa
    /// <summary>
    /// Cadastra uma nova pessoa no sistema.
    /// </summary>
    /// <param name="cadastro">DTO com dados para cadastro.</param>
    /// <returns>Mensagem de sucesso ou erro.</returns>
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
    /// <param name="loginDto">DTO contendo Email e Senha.</param>
    /// <returns>Dados do usuário e turma, ou erro se falha na autenticação.</returns>
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

        // Aqui você busca a matrícula do usuário para pegar a turma
        var matricula = await _context.Matriculas
            .Where(m => m.PessoaId == user.Id)
            .Select(m => m.TurmaId)
            .FirstOrDefaultAsync();

        return Ok(new
        {
            Id = user.Id,
            Nome = user.Nome,
            TipoUsuario = user.TipoUsuario,
            TurmaId = matricula  // pode ser 0 se não estiver matriculado em nenhuma turma
        });
    }

    // Rota para listar todas as pessoas
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
    /// Lista somente as pessoas do tipo aluno.
    /// </summary>
    /// <returns>Lista de alunos.</returns>
    // Rota para listar somente alunos
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
    /// Lista somente as pessoas do tipo professor.
    /// </summary>
    /// <returns>Lista de professores.</returns>
    // Rota para listar somente professores
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
}
