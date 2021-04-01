using curso.api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace curso.api.Configurations
{
    public class DbFactoryDbContext : IDesignTimeDbContextFactory<CursoDbContext>//classe criada por conta do entityfrmw.tools
    {
        public CursoDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CursoDbContext>();//db config
            optionsBuilder.UseSqlServer("");
            CursoDbContext contexto = new CursoDbContext(optionsBuilder.Options);//pede um db configuration no construtor, assinalado acima

            return contexto;
        }
    }
}
