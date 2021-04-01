using curso.api.Business.Entities;
using curso.api.Infrastructure.Data.Mappings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace curso.api.Infrastructure.Data
{
    public class CursoDbContext : DbContext//classe de configuração de contexto para conexão com o entity framework, o banco de dados
    {
        public CursoDbContext(DbContextOptions<CursoDbContext> options) : base(options)//construtor, quando instanciar essa classe leva essas opções do entity framework junto
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)//especifica as classes que o db vai usar
        {
            modelBuilder.ApplyConfiguration(new CursoMapping());
            modelBuilder.ApplyConfiguration(new UsuarioMapping());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Curso> Curso { get; set; }
    }
}
