using Microsoft.EntityFrameworkCore;
using PlataformaCursosAPI.Models;

namespace PlataformaCursosAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Professor> Professores { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Material> Materiais { get; set; }

        public DbSet<Turma> Turmas { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<Nota> Notas { get; set; }
        public DbSet<Comunicado> Comunicados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Professor>()
                .HasOne(p => p.Curso)
                .WithMany()
                .HasForeignKey(p => p.CursoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Matricula>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<Matricula>()
                .HasOne(m => m.Pessoa)
                .WithMany()
                .HasForeignKey(m => m.PessoaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Matricula>()
                .HasOne(m => m.Turma)
                .WithMany(t => t.Matriculas)
                .HasForeignKey(m => m.TurmaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Aluno>()
                .HasOne(a => a.Pessoa)
                .WithMany()
                .HasForeignKey(a => a.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Professor>()
                .HasOne(p => p.Pessoa)
                .WithMany()
                .HasForeignKey(p => p.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Material>()
                .HasOne(m => m.Turma)
                .WithMany(t => t.Materiais)
                .HasForeignKey(m => m.TurmaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
