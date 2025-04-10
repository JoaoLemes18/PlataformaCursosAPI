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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Professor>()
                .HasOne(p => p.Curso)
                .WithMany() // Um Curso pode ter vários Professores
                .HasForeignKey(p => p.CursoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
