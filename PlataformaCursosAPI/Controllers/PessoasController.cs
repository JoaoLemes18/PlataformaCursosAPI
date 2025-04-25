using Microsoft.AspNetCore.Mvc;
using PlataformaCursosAPI.Data;
using PlataformaCursosAPI.Models;
using PlataformaCursosAPI.Helpers;
using PlataformaCursosAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class PessoaController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly string _secretKey = "GJ2s2r8A$kL3vWjS8r1+zFg6X9Hj8Pcz2$Psfm7aZ4Z5VvXjH6KwV5yGb8NzHr3O";

    public PessoaController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("cadastrar")]
    public async Task<IActionResult> Cadastrar([FromBody] CadastroDto cadastro)
    {
        if (!Enum.IsDefined(typeof(TipoUsuario), cadastro.TipoUsuario))
            return BadRequest("Tipo de usuário inválido.");

        var tipoUsuario = (TipoUsuario)cadastro.TipoUsuario;

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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        var pessoa = await _context.Pessoas
            .Where(p => p.Email == login.Email)
            .FirstOrDefaultAsync();

        if (pessoa == null || !SenhaHelper.VerificarSenha(login.Senha, pessoa.SenhaHash))
            return Unauthorized("Email ou senha inválidos.");

        var token = GerarToken(pessoa);
        return Ok(new { token });
    }

    private string GerarToken(Pessoa pessoa)
    {
        var chaveSecreta = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credenciais = new SigningCredentials(chaveSecreta, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, pessoa.Nome),
            new Claim(ClaimTypes.Email, pessoa.Email),
            new Claim(ClaimTypes.Role, pessoa.TipoUsuario.ToString())
        };

        var tokenDescriptor = new JwtSecurityToken(
            issuer: "PlataformaCursosAPI",
            audience: "PlataformaCursosAPI",
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credenciais
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
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
}
