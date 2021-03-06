using ControleAluno.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleAluno.Repositories
{
    public class ControleAlunoDbContext : DbContext
    {
        public ControleAlunoDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Aluno> Aluno { get; set; }
        public DbSet<Disciplina> Disciplina { get; set; }
        public DbSet<Nota> Nota { get; set; }

    }
}
