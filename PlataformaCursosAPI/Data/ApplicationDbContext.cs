using Microsoft.EntityFrameworkCore;
using PlataformaCursosAPI.Models;

namespace PlataformaCursosAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Professor> Professores { get; set; }
        public DbSet<Curso> Cursos { get; set; }


    }
}