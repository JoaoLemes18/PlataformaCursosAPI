using Microsoft.EntityFrameworkCore;
using PlataformaCursosAPI.Models;

namespace PlataformaCursosAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Professor> Professores { get; set; }
        public DbSet<Curso> Cursos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relação 1:N (Um Curso pode ter vários Alunos)
            modelBuilder.Entity<Aluno>()
                .HasOne(a => a.Curso)
                .WithMany(c => c.Alunos)
                .HasForeignKey(a => a.CursoId);

            // Relação 1:1 (Um Professor ensina um Curso)
            modelBuilder.Entity<Professor>()
                .HasOne(p => p.Curso)
                .WithOne()
                .HasForeignKey<Professor>(p => p.CursoId);
        }
    }
}
